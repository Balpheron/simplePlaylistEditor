using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using PlaylistEditor.Schedule;

namespace PlaylistEditor.Schedule
{
    public class ScheduleManager
    {
        private string? xmlPath;
        private List<ScheduleItem> channelSchedule = new List<ScheduleItem>();
        private CancellationTokenSource? cancellationTokenSource;
        private ScheduleLoader loader;
        // структура для отображения пункта программы передач
        // содержит имя, дату начала и конца передачи, категорию и описание - если есть
        public struct ScheduleItem
        {
            public string Name { get; private set; }
            public string Description { get; private set; }
            public string Category { get; private set; }
            public string StartTime { get; private set; }
            public string EndTime { get; private set; }

            public ScheduleItem(string name, string startTime, string endTime, string? category, string? description)
            {
                Name = name;

                if (category == null)
                    Category = "Без категории";
                else Category = category;

                if (description == null)
                    Description = "Без описания";
                else Description = description;

                StartTime = startTime;
                EndTime = endTime;

            }

        }
        public delegate void StatusReport(string message);
        private event StatusReport OnStatusReport;
        

        public ScheduleManager(StatusReport reporter)
        {
            loader = new ScheduleLoader(RecieveLoaderMessage);
            OnStatusReport += reporter;
        }

        // пытаемся в загрузчике проверить, нужно ли загружать расписание, и получаем
        // путь к архиву
        public async Task<string> ScheduleCheck()
        {
            try
            {
                xmlPath = await loader.ScheduleCheck();
                return "Программа передач успешно загружена";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // 
        public async Task<string> LoadChannelSchedule(string channelName)
        {
            if (!Configurator.currentConfig.loadSchedule)
                return "";

            if (xmlPath == null)
                return "Не найден архив программы передач";

            channelName = channelName.Replace("\r", "");
            channelName = channelName.ToUpper();
            
            string operationResult = "";

            // с помощью xmlreader пробегаемся по файлу с программой передач, не загружая его в память
            // сначала будет искать id канала по его имени, а после по найденому значению получим программу передач для канала;
            // предполагается, что в начале документа собраны сведения о каналах в одном блоке;
            // в следующем блоке находятся программы передач
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
            
            CancellationToken token = cancellationTokenSource.Token;
            string? channelId = await GetChannelId(xmlPath, channelName, token);

            if (channelId == null) 
            {
                operationResult = $"Канал с таким названием не найден в списке";
                return operationResult;
            }

            // получаем список из элементов программы передач
            channelSchedule = await GetScheduleList(xmlPath, channelId, token);

            if (channelSchedule.Count < 1)
            {
                operationResult = $"Программа передач для выбранного канала не найдена";
                return operationResult;
            }

            operationResult = "Программа передач загружена";
            return operationResult;
        }

        // возвращает ID канала по его имени; если канала не найдено, возвращает null
        async Task<string?> GetChannelId(string xmlPath, string channelName, CancellationToken token)
        {
            string? operationResult = null;
            // настройки ридера
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            settings.IgnoreWhitespace = true;
            settings.Async = true;
            // создаем xmlreader для файла
            using (XmlReader reader = XmlReader.Create(xmlPath, settings))
            {
                // перемещаемся к первому узлу, являющемуся контентом
                await reader.MoveToContentAsync();
                bool found = false;
                // проходим до конца документа по одному узлу
                while (!reader.EOF && !found)
                {
                    if (token.IsCancellationRequested)
                        return "Операция прервана";
                    // проверяем все узлы с именем channel
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "channel")
                    {
                        // получаем элемент из текущей строки
                        XElement? el = await XElement.ReadFromAsync(reader, token) as XElement;
                        if (el != null)
                        {

                            // проверяем все дочерние элементы - если значение какого-то совпадает с именем канала,
                            // возвращаем первый атрибут - id;
                            // это делается потому, что несколько одноименных элементов могут содержать вариации имени канала
                            foreach (var item in el.Elements())
                            {
                                string checkItem = item.Value.ToUpper();
                                if (checkItem == channelName)
                                {
                                    operationResult = el.FirstAttribute?.Value;
                                    // указываем, что поиск завершен
                                    found = true;
                                    break;
                                }
                            }

                        }

                    }
                    else
                    {
                        await reader.ReadAsync();
                    }
                }
            }
            return operationResult;
        }

        // получаем программу передач для найденного ID
        async Task<List<ScheduleItem>> GetScheduleList(string xmlPath, string channelId, CancellationToken token)
        {
            List<ScheduleItem> scheduleList = new List<ScheduleItem>();
            // настройки ридера
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            settings.IgnoreWhitespace = true;
            settings.Async = true;
            // создаем xmlreader для файла
            using (XmlReader reader = XmlReader.Create(xmlPath, settings))
            {
                // перемещаемся к первому узлу, являющемуся контентом
                await reader.MoveToContentAsync();
                bool found = false; // отметка о том, что найден нужный блок информации
                // проходим до конца документа по одному узлу
                while (!reader.EOF)
                {
                    if (token.IsCancellationRequested)
                        break;

                    // проверяем все узлы с именем programme
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "programme")
                    {
                        // получаем элемент из текущей строки
                        XElement? el = await XElement.ReadFromAsync(reader, token) as XElement;
                        if (el != null)
                        {
                            // если мы в процессе считывания программы передач из элементов с нужным id, и
                            // текущий элемент имеет другой id - прерываем процесс чтения, так как считаем, 
                            // что программа передач для одного канала сгруппирована в один блок
                            if (el.Attribute("channel")?.Value != channelId)
                            {
                                if (found)
                                    break;
                                continue;
                            }
                            else
                            {
                                found = true; // делаем отметку о том, что мы нашли нужный блок
                                // получаем все нужные данные из элемента
                                string? startTime = el.Attribute("start")?.Value;
                                string? endTime = el.Attribute("stop")?.Value;
                                string? name = el.Element("title")?.Value;
                                string? description = el.Element("desc")?.Value;
                                string? category = el.Element("category")?.Value;
                                // добавляем структуру в список
                                scheduleList.Add(new ScheduleItem(name, startTime, endTime, category, description));
                            }
                         
                        }

                    }
                    else
                    {
                        await reader.ReadAsync();
                    }
                }
            }
            return scheduleList;
        }

        // итератор для перебора элементов программы передач
        public IEnumerator<ScheduleItem> GetEnumerator()
        {
            foreach (var item in channelSchedule)
            {
                yield return item;
            }
        }

        // получаем конкретный элемент из списка
        public virtual ScheduleItem? GetScheduleItemData(int index)
        {
            try
            {
                return channelSchedule[index];
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        // получаем элемент из списка, который транслируется в данный момент 
        public virtual ScheduleItem? GetCurrentItem()
        {
            DateTime now = DateTime.Now;

            if (channelSchedule.Count < 1)
                return null;

            // проверям список с конца, так как таким образом зачастую будет ближе дойти до текущего дня
            for (int i = channelSchedule.Count - 1; i > 0; i--)
            {
                DateTime startTime = DateTime.Now; DateTime.TryParseExact(channelSchedule[i].StartTime, Syntax.dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out startTime);
                DateTime endTime = DateTime.Now; DateTime.TryParseExact(channelSchedule[i].EndTime, Syntax.dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out endTime);
                if (now < endTime && now > startTime)
                    return channelSchedule[i];
            }
            return null;
        }

        // получение сообщений от загрузчика
        private void RecieveLoaderMessage(string message)
        {
            OnStatusReport.Invoke(message);
        }

    }
}

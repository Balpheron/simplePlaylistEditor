using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace PlaylistEditor.Schedule
{
    public class ScheduleLoader
    {
        private string currentSchedulePath;
        private bool inProgress = false; // происходит ли процесс скачки
        public delegate void ReportStatus(string message);
        private event ReportStatus OnReportStatus;
        private Form1 main;

        public ScheduleLoader(ReportStatus reporter)
        {
            if (Configurator.currentConfig?.schedulePath != null)
                currentSchedulePath = Configurator.currentConfig.schedulePath;
            else currentSchedulePath = "";
            OnReportStatus += reporter;
            main = Form1.instance;
        }



        // проверяем на необходимость скачать архив с программой передач и распаковать его;
        // делаем такую проверку при запуске программы и при сохранении настроек
        public async Task<string> ScheduleCheck()
        {
            if (!Configurator.currentConfig.loadSchedule)
                throw new Exception("");

            if (inProgress)
                throw new Exception("Идет процесс загрузки архива"); ;

            inProgress = true;

            // путь к файлу с программой передач
            string? filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            filePath += @"\schedule.xml";
            
            bool needToDownload = false;
            
            // если указан путь к архиву, но в папке отсутствует распакованный файл с расписанием
            if (Configurator.currentConfig?.schedulePath != null && !File.Exists(filePath))
                needToDownload = true;
            // если изменилась ссылка на архив
            else if (currentSchedulePath != Configurator.currentConfig?.schedulePath)
                needToDownload = true;
            // если прошло больше 24 часов с момента последнего скачивания архива
            else
            {
                TimeSpan passedTime = DateTime.Now - Configurator.currentConfig.lastCheckDate;
                if (passedTime.TotalHours >= 24)
                    needToDownload = true;
            }
            // устанавливает новую ссылку на архив
            currentSchedulePath = Configurator.currentConfig?.schedulePath;
            // запускает процесс скачивания и распаковки архива с программой передач
            if (needToDownload)
            {
                try
                {
                    string result;
                    result = await GetSchedule(currentSchedulePath);
                    // если все прошло успешно - отмечаем дату последней загрузки расписания
                    Configurator.currentConfig.lastCheckDate = DateTime.Now;
                    Configurator.WriteConfig();
                    inProgress = false;
                    return $"{filePath}";
                }
                catch (Exception ex)
                {
                    inProgress = false;
                    throw new Exception(ex.Message);
                }
            }

            inProgress = false;
            throw new Exception($"Программа передач не нуждается в обновлении (последнее обновление - {Configurator.currentConfig.lastCheckDate.ToString("g")})");
        }

        // получаем архив с программой передач по указанной ссылке
        // и сохраняем его в корень приложения
        private async Task<string> GetSchedule(string url)
        {
            try
            {
                OnReportStatus.Invoke("Загружаем архив с сайта");
                // получаем из заголовка размер архива
                HttpResponseMessage response = await WebManager.client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                long lenght = long.Parse(response.Content.Headers.First(h => h.Key.Equals("Content-Length")).Value.First());
                // получаем поток
                var stream = await WebManager.client.GetStreamAsync(url);
                OnReportStatus.Invoke("Распаковываем архив");
                lenght *= 9; // средний коэффициент сжатия архива
                await DecompressZip(stream, lenght);
                return "Программа передач обновлена";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // распаковываем архив с программой передач 
        private async Task DecompressZip(Stream fileStream, long lenght)
        {
            // находим корневую директорию приложения
            string? filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            bool trackStatus = true;
            // производим распаковку указанного файла архива (ранее скачанного по ссылке)
            // из корня в другой xml-файл также в корне
            await Task.Run(() =>
            {
                // стрим записи файла
                using FileStream outputFileStream = File.Create(@$"{filePath}\schedule.xml");
                using var decompressor = new GZipStream(fileStream, CompressionMode.Decompress);

                // дополнительный поток, который показывает прогресс процессом скачивания и распаковки
                new Task(new Action(() => { TrackStream(outputFileStream, lenght, ref trackStatus); })).Start();
                decompressor.CopyTo(outputFileStream);
                trackStatus = false;
            });
        }

        private void TrackStream(FileStream stream, double targetLenght, ref bool status)
        {
            int prevValue = 0;
            while (status)
            {
                int curValue = (int)Math.Round(100 * stream.Position / targetLenght);
                if (curValue != prevValue)
                main.BeginInvoke(new Action(() => main.ProgressUpdate()));
                prevValue = curValue;
                Thread.Sleep(100);
            }
        }

    }
}

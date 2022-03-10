using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PlaylistEditor
{
    public static class Configurator
    {
        public static Config currentConfig;
        internal static Form1 mainForm;

        public static void ReadConfig()
        {
            currentConfig = new Config();
            // считываем конфигурацию из файла в папке с программой
            string? filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            filePath += @"\config.txt";
            string fileText;
            try
            {
                fileText = System.IO.File.ReadAllText(filePath); //считываем текст из файла
            }
            catch (Exception)
            {
                // если файла конфигурации нет, создаем стандартную конфигурацию
                CreateDefaultConfig();
                return;
            }
            // считываем время проверки канала
            string regexPattern = "Timeout:" + @".*";
            Regex regex = new Regex(regexPattern);
            string curData = regex.Match(fileText).Value;
            curData = curData.Replace("Timeout:", "");
            int timeout;
            int.TryParse(curData, out timeout);
            currentConfig.CheckTimeoutTime = timeout;
            
            // считываем массив нестандартных параметров
            regexPattern = "CustomValues:" + @".*";
            regex = new Regex(regexPattern);
            curData = regex.Match(fileText).Value;
            curData = curData.Replace("CustomValues:", "");
            // сначала разделяем текст на отдельные пары ключ_параметр
            string[] values = curData.Split(';');
            // теперь добавляем каждую пару в словарь
            currentConfig.customValues = new Dictionary<string, string>();
            foreach (var item in values)
            {
                string[] curVal = item.Split('_');
                try
                {
                    currentConfig.customValues.Add(curVal[0], curVal[1]);
                }
                catch (Exception)
                {
                    CreateDefaultConfig();
                    return;
                }
                
            }
        }        

        static void CreateDefaultConfig()
        {
            currentConfig.CheckTimeoutTime = 5;
            currentConfig.customValues = new Dictionary<string, string>() { { "ID для EPG", "tvg-id"},
            { "Длительность архива", "tvg-rec"},
            };
        }

        
        public static void WriteConfig()
        {
            string? filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            filePath += @"\config.txt";
            // создаем текстовый файл со значениями настроек
            string fileText = "";
            fileText += $"Timeout:{currentConfig.CheckTimeoutTime}\n";
            fileText += $"CustomValues:";
            foreach (var item in currentConfig.customValues)
            {
                fileText += $"{item.Key}_{item.Value};";
                
            }
            fileText = fileText.Remove(fileText.Length - 1);
            System.IO.File.WriteAllText(filePath, fileText);
            //mainForm.ShowErrorWindow(fileText);

        }
    }
}

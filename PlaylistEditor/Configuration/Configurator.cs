using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace PlaylistEditor
{
    public static class Configurator
    {
        public static Config? currentConfig;
        
        // считываем конфигурацию из текстового файла
        public static void ReadConfig()
        {
            // создаем конфигурацию по умолчанию
            currentConfig = new Config();
            // находим файл в папке с программой
            string? filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            filePath += @"\config.json";
            string fileText;
            try
            {
                fileText = System.IO.File.ReadAllText(filePath); //считываем текст из файла
            }
            catch (Exception)
            {
                // если файла конфигурации нет, то создаем файл с конфигурацией по умолчанию
                WriteConfig();
                return;
            }
            currentConfig = JsonConvert.DeserializeObject<Config>(fileText);
        }        


        // сохраняем конфигурацию в json-файл
        public static void WriteConfig()
        {
            if (currentConfig == null)
                return;

            string fileText = JsonConvert.SerializeObject(currentConfig);
            // путь сохранения - корневая директория приложения
            string? filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            filePath += @"\config.json";
            // создаем файл со значениями настроек
            System.IO.File.WriteAllText(filePath, fileText);
        }
    }
}

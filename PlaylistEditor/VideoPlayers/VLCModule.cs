using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PlaylistEditor.VideoPlayers
{
    internal static class VLCModule
    {
        static Process? currentProcess;

        // запускаем внешний VLC player и пытаемся воспроизвести видео по ссылке
        internal static void PlayChannel(string url)
        {
            // из конфигурации берем путь к файлу плеера
            string? path = Configurator.currentConfig.VLCPath;
            if (path == null || !File.Exists(path))
                throw new Exception("Не указан путь к VLC player");
            // убираем лишние символы в url
            url = url.Replace("\r", "");

            if (url.Length < 1)
                throw new ArgumentException("Неверная ссылка");


            try
            {
                // запускаем процес и убираем ранее запущенный из приложения плеер, если необходимо
                if (currentProcess != null)
                    currentProcess.Kill();
                currentProcess = Process.Start(Configurator.currentConfig.VLCPath, url);
            }
            catch (Exception)
            {
                throw new Exception("Не удалось запустить VLC player");
            }
        }

    }
}

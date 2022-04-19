using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistEditor
{
    internal static class WebManager
    {
        private static int maxTasks = 10;
        internal static int MaxTasks { get { return maxTasks; } set { if (maxTasks >= 1 && maxTasks <= 1000) maxTasks = value; else maxTasks = 1; } }
        internal static readonly HttpClient client = new HttpClient();
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(maxTasks, maxTasks); // семафор для ограничения числа одновременных потоков с запросами
        private static bool processStatus;
        internal static Form1 main;

        // пытаемся получить картику по адресу 
        internal static async Task<Image?> LoadImage(string logoLink)
        {
            try
            {
                // осуществляем запрос и получаем поток
                HttpResponseMessage response = await WebManager.client.GetAsync(logoLink);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                return Image.FromStream(stream);

            }
            catch (Exception)
            {
                return null;
            }
        }

        // задание максимального количества поток проверки доступности канала
        // пока в интерфейсе не реализовано
        internal static void SetMaxTasks(string value)
        {
            int intValue;
            int.TryParse(value, out intValue);
            MaxTasks = intValue;
        }

        // объединяет два массива координат в один
        private static int[] CoordinatesMerger(int[] firstCoords, int[] secondCoords)
        {
            int[] merged = new int[firstCoords.Length + secondCoords.Length];
            int index = 0;
            foreach (var item in firstCoords)
                merged[index++] = item;
            foreach (var item in secondCoords)
                merged[index++] = item;
            return merged;
        }

        internal static async Task CheckGroupOfChannels(int[] coords)
        {
            INodeData? currentElement = PlaylistManager.GetInstance().GetObject(coords);

            if (currentElement == null)
                return;

            semaphoreSlim = new SemaphoreSlim(maxTasks, maxTasks);
            processStatus = true;
            // если выбранный узел представляет собой перечисляемую коллекцию
            if (currentElement is System.Collections.IEnumerable numerable)
            {
                var tasks = new List<Task>();
                // перебираем координаты всех каналов выбранного элемента
                foreach (int[] item in numerable)
                {
                    // соединяем координаты выбранного с узла с координатами перебираемых элементов,
                    // чтобы получить полные координаты
                    int[] nodeCoords = CoordinatesMerger(coords, item);
                    // собираем все задачи в один список
                    var task =  CheackAvailability(nodeCoords, true);
                    tasks.Add(task);

                }
                // запускаем список задач
                await Task.WhenAll(tasks);

            } 
            
            // если выделенный узел - просто канал (не коллекция узлов), проверяем только его
            else
                await Task.Run(() => CheackAvailability(coords, true));
            
        }

        internal static void CancelProcess()
        {
            processStatus = false;
        }

        // посылаем запрос по ссылке, указанной в описании выбранного канала
        // в случаем недачного запроса или истекшего времени ожидания считаем канал недоступным
        internal static async Task<string> CheackAvailability(int[] coords, bool markChecked)
        {
            // ждем свободное место, которое обозначается семафором
            await semaphoreSlim.WaitAsync();

            if (processStatus == false)
                return "Операция прервана";

            TreeNode? node = null;
            try
            {
                // раскрашиваем узел, если нужно: в серый, когда идет процесс;
                // зеленый - канал отвечает на запрос, красный - нет
                if (markChecked)
                {
                    node = TreeManager.GetNode(coords);
                    main.BeginInvoke(new Action (() => main.MarkNode(node, Form1.NodeStatus.InProgress)));
                     //node.BackColor = Color.DarkGray;
                    //node.ForeColor = Color.White;
                }

                Channel? channel = (Channel)PlaylistManager.GetInstance().GetObject(coords);

                if (channel == null)
                    return "No channel exists";

                string checkPath = channel.ChannelPath;
                // осуществляем запрос и получаем поток
                using var cts = new CancellationTokenSource();
                // максимальное время на запрос
                int timeout = Configurator.currentConfig.CheckTimeoutTime;

                cts.CancelAfter(TimeSpan.FromSeconds(timeout));

                HttpResponseMessage response = await client.GetAsync(checkPath, cts.Token);
                
                response.EnsureSuccessStatusCode();
                // string data = await response.Content.ReadAsStringAsync();
                // показываем сообщение
                
            }
            catch (Exception ex)
            {
                // если ошибка прерывания задачи или процесса, считаем, что канал
                // передает поток, и называем его условно доступным
                if (ex is TaskCanceledException || ex is OperationCanceledException)
                {
                    if (node != null)
                        main.BeginInvoke(new Action(() => main.MarkNode(node, Form1.NodeStatus.PartialyAvailable)));
                    semaphoreSlim.Release();
                    return "Канал условно доступен";
                }
                // если запрос неудачен, считаем, что канала недоступен
                if (node != null)
                    main.BeginInvoke(new Action(() => main.MarkNode(node, Form1.NodeStatus.NotAvailable)));
                semaphoreSlim.Release();
                //return ex.Message;
                return "Канал НЕдоступен";
            }
            if (node != null)
                main.BeginInvoke(new Action(() => main.MarkNode(node, Form1.NodeStatus.Available)));
            semaphoreSlim.Release();
            return "Канал доступен";
            
        }
    }
}

using System.Net;


namespace PlaylistEditor
{
    public partial class Form1 : Form
    {
        PlaylistManager playlistManager;

        public delegate void PlaylistGenerator();
        public PlaylistGenerator playlistGenerator;

        public delegate void PlaylistClick();
        event PlaylistClick OnPlaylistClick;

        public delegate void GroupClick(int groupIndex);
        event GroupClick OnGroupClick;

        public delegate void ChannelClick(int groupIndex, int channelIndex);
        event ChannelClick OnChannelClick;

        public delegate void ChannelSaveData();
        event ChannelSaveData OnChannelSaveData;

        bool dragTime;

        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        private List<Label> customValuesLabels = new List<Label>();
        private List<TextBox> customValuesText = new List<TextBox>();

        TreeNode? bufferNode;

        public Form1()
        {
            InitializeComponent();
            //добавляем обработчик ошибок при генерации плейлистов
            playlistManager = new PlaylistManager(this);
            playlistManager.AddListener(ShowErrorMessage);
            //добавляем обработчики событий нажатий на плейлист, канал или группу
            OnPlaylistClick += VisualizePlaylist;
            OnGroupClick += VisualizeGroup;
            OnChannelClick += VisualizeChannel;
        }

        //вывод сообщений об ошибках
        void ShowErrorMessage(string message)
        {
            textBox1.Text += $"{message} \n";
        }

        public void ShowErrorWindow(string message)
        {
            string caption = "Ошибка!";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(message, caption, buttons);
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            // добавляем обработчик события нажатия кнопки для каждого текстового поля группы сведений о канале, чтобы активировать кнопку сохранения
            foreach (Control ctrl in this.channelGroup.Controls)
            {
                if (ctrl is TextBox tBox)
                {
                    (tBox).KeyDown += TextChangedChecker;
                }
            }

            // тултипы
            toolTip1.SetToolTip(this.deleteButton, "Удалить элемент");
            toolTip1.SetToolTip(this.newGroupButton, "Добавить элемент");
            toolTip1.SetToolTip(this.OpenListButton, "Открыть плейлист");
            toolTip1.SetToolTip(this.saveButton, "Сохранить плейлист");
            toolTip1.SetToolTip(this.checkAllButton, "Проверить выбранный элемент");
            toolTip1.SetToolTip(this.stopCheckingButton, "Остановить текущую проверку");
            toolTip1.SetToolTip(this.copyButton, "Копировать элемент");
            toolTip1.SetToolTip(this.pasteButton, "Вставить элемент");
            // считываем конфигурацию и создаем строки под нестандартные параметры
            Configurator.mainForm = this;
            Configurator.ReadConfig();
            CustomValuesUI();
        }

        private async void OpenListButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string fileName = openFileDialog1.FileName;
            string fileText = System.IO.File.ReadAllText(fileName); //считываем текст из файла

            //запускае параллельную задачу, в которой получаем плейлист и генерируем на его основе дерево
            Task generationProcess = new Task(() => {
                playlistGenerator = GeneratePlaylistTree;
                playlistManager.playlists.Add(playlistManager.GeneratePlaylist(ref fileText, fileName)); //создаем объект плейлиста из полученных данных
                playlistManager.CurrentPlaylistIndex = playlistManager.playlists.Count - 1; // созданный плейлист становится текущим плейлистом
                playlistManager.GeneratePlaylistTree();   
                
            });

           generationProcess.Start();            

        }

        public void GeneratePlaylistTree()
        {
            int currentPlaylist = playlistManager.CurrentPlaylistIndex;
            //создаем основной узел плейлиста
            AddPlaylist(playlistManager.playlists[currentPlaylist].Name);
            //перебирая в двойном цикле все категории и каналы, добавляем их узлы в дерево
            for (int i = 0; i < playlistManager.playlists[currentPlaylist].groupsList.Count; i++)
            {
                AddGroup(playlistManager.playlists[currentPlaylist].groupsList[i].Name);
                for (int k = 0; k < playlistManager.playlists[currentPlaylist].groupsList[i].channelsList.Count; k++)
                {
                    AddChannel(playlistManager.playlists[currentPlaylist].groupsList[i].channelsList[k].Name, i);
                }
            }

        }

        //удаляем дерево старого плейлиста и создаем узел плейлиста
        public void AddPlaylist(string name)
        {
            treeView1.Nodes.Add(name);
        }

        //генерация узла существующей группы
        public void AddGroup(string name)
        {
            // playlistManager.currentPlaylist?.FindGroup("", true);
            treeView1.Nodes[playlistManager.CurrentPlaylistIndex].Nodes.Add(name);
        }

        //генерация узла существующего канала для указанной группы
        public void AddChannel(string name, int groupIndex)
        {
            treeView1.Nodes[playlistManager.CurrentPlaylistIndex].Nodes[groupIndex].Nodes.Add(name);
        }

        // визуализация узла канала
        async public void VisualizeChannel(int groupIndex, int channelIndex)
        {

            //делаем неактивной кнопку сохранения изменений
            saveChannel.Enabled = false;
            //отображаем группу интерфейсных элементов, хранящих информацию о канале
            channelGroup.Visible = true;

            playlistManager.CurrentPlaylist.CurrentChannelIndex = channelIndex;
            playlistManager.CurrentPlaylist.CurrentGroupIndex = groupIndex;
            Channel currentChannel = playlistManager.CurrentPlaylist.CurrentChannel;

            //заполняем текстовые строки данными канала
            channelNameText.Text = currentChannel.Name;
            logoLinkText.Text = currentChannel.LogoPath;
            channelLinkText.Text = currentChannel.ChannelPath;
            groupText.Text = currentChannel.GroupName;
            durationText.Text = currentChannel.ChannelDuration;
            // дополнительные строки
            customValuesText[^1].Text = currentChannel.additionalData;
            // строки нестандартных параметров
            for (int i = 0; i < currentChannel.customData.Length; i++)
            {
                customValuesText[i].Text = currentChannel.customData[i];
            }
            //пытаемся загрузить изображение по ссылке
            try
            {
               logoPicture.Image = await Task.Run(() => LoadLogo(currentChannel.LogoPath));
            }
            catch (Exception)
            {
                logoPicture.Image = null;
                ShowErrorMessage("Cannot load logo");
            }
        }

        public void VisualizeGroup(int groupIndex)
        {
            channelGroup.Visible = false;
        }

        public void VisualizePlaylist()
        {
            channelGroup.Visible = false;
        }

                
        //загрузка изображений по ссылке
        async Task<Image> LoadLogo(string logoLink)
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
                // в случае неудачи очищаем логотип
                ShowErrorMessage($"Failed to load image: {logoLink}");
                return null;
                
            }
        }

        private void TextChangedChecker(object sender, EventArgs e)
        {
            saveChannel.Enabled = true;
        }
        
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
           
        }

        // отображаем информацию по каналу при нажатии на его узел в дереве
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (dragTime)
                return;

            //в зависимости от выбранного узла заполняем необходимые текстовые поля
            switch (e.Node?.Level)
            {
                case 0: playlistManager.CurrentPlaylistIndex = e.Node.Index; OnPlaylistClick?.Invoke(); break;
                case 1: playlistManager.CurrentPlaylistIndex = e.Node.Parent.Index; OnGroupClick?.Invoke(e.Node.Index); break;
                case 2: playlistManager.CurrentPlaylistIndex = e.Node.Parent.Parent.Index; OnChannelClick?.Invoke(e.Node.Parent.Index, e.Node.Index); break;
                default: break;
            }
                       
        }

        //редактирование имени через дерево
        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    switch (e.Node?.Level)
                    {
                        //если редактируется название плейлиста, то просто меняем его в соответствующем объекте
                        case 0: playlistManager.CurrentPlaylist.Name = e.Label; break;
                        //если редактируется группа, то помимо изменения его названия в объекте также меняем ее название во всех каналах
                        case 1: playlistManager.CurrentPlaylist.groupsList[e.Node.Parent.Index].Name = e.Label; GroupRename(e.Node.Index, e.Label); break;
                        //если редактируется канал, меняем его название в объекте
                        case 2: playlistManager.CurrentPlaylist.groupsList[e.Node.Parent.Index].channelsList[e.Node.Index].Name = e.Label; channelNameText.Text = e.Label; break;
                        default: break;
                    }

                    e.Node?.EndEdit(false);
                }
                else
                {
                    // отменяем редактирование в случае, если название остается пустым;
                    // выдаем сообщение об ошибке и заново запускаем редактирование
                    e.CancelEdit = true;
                    ShowErrorMessage("Название не может быть пустым");
                    e.Node.BeginEdit();
                }
            }
        }

        //изменение имени группы для всех каналов в переименовываемой группе
        private void GroupRename(int groupIndex, string newGroupName)
        {
            for (int i = 0; i < playlistManager.CurrentPlaylist.groupsList[groupIndex].channelsList.Count; i++)
                playlistManager.CurrentPlaylist.groupsList[groupIndex].channelsList[i].GroupName = newGroupName;
            
        }

        // сохраняем в объект информацию из всех текстовых строк
        async private void SaveChannelData()
        {
            // создаем новый объект с данными
            Channel currentChannel = new Channel(channelLinkText.Text, channelNameText.Text, logoLinkText.Text, groupText.Text, durationText.Text);
            // дополнительные строки
            currentChannel.additionalData = customValuesText[^1].Text;
            // строки нестандартных параметров
            currentChannel.customData = new string[customValuesText.Count - 1];
            for (int i = 0; i < currentChannel.customData.Length; i++)
            {
                ShowErrorMessage(customValuesText[i].Text);
                currentChannel.customData[i] = customValuesText[i].Text;
            }
            int newGroupIndex = -1;
            //выполняем необходимые проверки и получаем индекс группы, если нужен перенос между ними
            try
            {
               newGroupIndex =  playlistManager.CurrentPlaylist.SaveChannel(currentChannel);
            }
            catch (Exception e)
            {
                ShowErrorWindow(e.Message);
                return;
            }

            //в случае успеха изменяем элементы дерева: сначала имя узла
            treeView1.SelectedNode.Text = channelNameText.Text;
            //создаем новую группу, если необходимо
            if(newGroupIndex != -1)
            {
                // проверяем, нужно ли создавать новый узел группы, или берем существующий
                TreeNode? newGroupNode;
                try
                {
                    newGroupNode = treeView1.Nodes[playlistManager.CurrentPlaylistIndex].Nodes[newGroupIndex];
                }
                catch (Exception)
                {
                    AddGroup(groupText.Text);
                    newGroupNode = treeView1.Nodes[playlistManager.CurrentPlaylistIndex].Nodes[newGroupIndex];
                }
                // удаляем старый узел и запоминаем его
                TreeNode oldNode = treeView1.SelectedNode;
                treeView1.Nodes.Remove(treeView1.SelectedNode);
                try
                {
                    treeView1.Nodes[playlistManager.CurrentPlaylistIndex].Nodes[newGroupIndex].Nodes.Add(oldNode);
                }
                catch (Exception e)
                {
                    ShowErrorMessage(e.Message);
                }
                // и выделяем новый нод
                treeView1.SelectedNode = oldNode;
                
            }
            //если переноса в другую группу нет, корректируем значения в некоторых строках интерфейса
            else
            {
                // загружаем новый логотип
                try
                {
                    logoPicture.Image = await Task.Run(() => LoadLogo(currentChannel.LogoPath));
                }
                catch (Exception)
                {
                    logoPicture.Image = null;
                    ShowErrorMessage("Cannot load logo");
                }
                // изменяем в строке длительности текущий текст на текст в правильном формате
                durationText.Text = currentChannel.ChannelDuration;
            }

            //событие
            OnChannelSaveData?.Invoke();
        }

        private void saveChannel_Click(object sender, EventArgs e)
        {
            SaveChannelData();
            
        }

        //загрузка логотипа по новой ссылке
        private void reloadLogo_Click(object sender, EventArgs e)
        {
            ReloadLogo();
        }

        async void ReloadLogo()
        {
            try
            {
                logoPicture.Image = await Task.Run(() => LoadLogo(logoLinkText.Text));
            }
            catch (Exception)
            {
                logoPicture.Image = null;
                ShowErrorMessage("Cannot load logo");
            }
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //перемещаем выбранный узел
            if (e.Button == MouseButtons.Left)
            {
                dragTime = true;
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            // получаем координаты указателя в дереве
            Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));

            // получаем узел в точке дропа
            TreeNode? targetNode = treeView1.GetNodeAt(targetPoint);

            // получаем перетаскиваемый узел
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            // выбираем узел по координатам положения указателя
            if (draggedNode.Level >= targetNode?.Level)
                treeView1.SelectedNode = targetNode;
        }

        /* перетаскивание узлов
        перемещать узлы можно или внутри своего уровня, или внутри уровня на единицу выше
        при перемещении внутри своего уровня изменяется порядок ячеек
        при перемещении на уровене выше происходит перемещение узла в другую категорию 
        */
        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
             // получаем координаты узла, над которым находится мышь
            Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));

            // получаем узел в точке дропа
            TreeNode targetNode = treeView1.GetNodeAt(targetPoint);

            // получаем перетаскиваемый узел
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            if (targetNode == null)
                return;

            // убеждаемся, что перетаскиваемый и выбранный узел - не одно и то же
            if (!draggedNode.Equals(targetNode) && e.Effect == DragDropEffects.Move)
            {
                PasteNode(ref draggedNode, ref targetNode, true);
                dragTime = false;
            }
        }

        void PasteNode(ref TreeNode draggedNode, ref TreeNode targetNode, bool removeDragged)
        {
            // в зависимости от уровня глубины переносимого нода 
            switch (draggedNode.Level)
            {
                // плейлист - ничего
                case 0: return;
                // группа
                case 1:
                    {
                        // в зависимости от глубины целевого узла
                        switch (targetNode.Level)
                        {
                            // если это не родной плейлист, добавляем в конец этого плейлиста текущую группу
                            case 0:
                                {
                                    if (targetNode.Index != draggedNode.Parent.Index || !removeDragged)
                                    {
                                        // перемещенме объектов в коллекции
                                        playlistManager.MoveElement(draggedNode.Index, draggedNode.Parent.Index, targetNode.Index, removeDragged);
                                        MoveNode(ref draggedNode, ref targetNode, removeDragged);
                                    }
                                    else return;
                                    break;
                                }
                            // если это другая группа, то ставим переносимую группа на место целевой группы
                            case 1:
                                {
                                    // перемещенме объектов в коллекции

                                    SwapNodes(ref draggedNode, ref targetNode, 1, removeDragged);
                                    break;
                                }
                            default: return;
                        }
                        break;
                    }
                // канал
                case 2:
                    {
                        // в зависимости от глубины целевого узла
                        switch (targetNode.Level)
                        {
                            // если это не родная категория, добавляем в конец этой категории текущий канал
                            case 1:
                                {
                                    if (targetNode.Index != draggedNode.Parent.Index || !removeDragged)
                                    {
                                        // перемещенме объектов в коллекции
                                        playlistManager.MoveElement(draggedNode.Index, (draggedNode.Parent.Parent.Index, draggedNode.Parent.Index), (targetNode.Parent.Index, targetNode.Index), removeDragged);
                                        MoveNode(ref draggedNode, ref targetNode, removeDragged);
                                    }
                                    else return;
                                    break;
                                }
                            // если это другой канал, то ставим переносимую группа на место целевой группы
                            case 2:
                                {
                                    // перемещенме объектов в коллекции
                                    SwapNodes(ref draggedNode, ref targetNode, 2, removeDragged);
                                    break;
                                }
                            default: return;
                        }
                        break;
                    }

            }
        }

            void SwapNodes(ref TreeNode draggedNode, ref TreeNode targetNode, int level, bool removeDragged)
        {
            // перемещение узлов
            TreeNode clondeNode = (TreeNode)draggedNode.Clone();
            int[] oldNode = GetCoordinates(draggedNode);
            // если узел перетаскивается снизу, он становится на место выделенного узла, а выделенный смещается вниз
            if (draggedNode.Index > targetNode.Index || draggedNode.Parent.Index > targetNode.Parent.Index)
                targetNode.Parent.Nodes.Insert(targetNode.Index, clondeNode);
            // если сверху, то перестаскиваемый узел становится снизу выделенного
            else targetNode.Parent.Nodes.Insert(targetNode.Index + 1, clondeNode);
            //удаляем перетаскиваемый узел и выделяем вставленную копию
            if (removeDragged)
            draggedNode.Remove();

            
            int[] newNode = GetCoordinates(clondeNode);

            if (level == 2)
            playlistManager.MoveElement(oldNode, newNode, removeDragged);
            else playlistManager.MoveElement(true, oldNode, newNode, removeDragged);
            treeView1.SelectedNode = clondeNode;
        }

        void MoveNode(ref TreeNode draggedNode, ref TreeNode targetNode, bool removeDragged) 
        {
            // перемещение узлов
            // если нужно, то удаляем исходный узел для перемещения
            if (removeDragged)
            {
                draggedNode.Remove();
                targetNode.Nodes.Add(draggedNode);
            }
            else
            {
                TreeNode clone = (TreeNode)draggedNode.Clone();
                targetNode.Nodes.Add(clone);
            }
        }

        // удаляем выбранный узел и информацию о нем
        public void DeleteNode()
        {
            if (treeView1.SelectedNode == null)
                throw new Exception("No node selected");
            // создаем массив в длинной, равной глубине выбранного узла
            TreeNode curNode = treeView1.SelectedNode;
            int[] coordinates = GetCoordinates(curNode);
            // удаляем объект из коллекции
            playlistManager.DeleteElement(coordinates);
            // удаляем узел
            treeView1.SelectedNode.Remove();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            DeleteNode();
        }

        // обработка нажатый клавиш в дереве
        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                try
                {
                    DeleteNode();
                }
                catch (Exception ex)
                {

                    ShowErrorMessage(ex.Message);
                }
                
            }
        }

        public void NewElement()
        {
            TreeNode? selected = treeView1.SelectedNode;
            string newGroupName = "";
            try
            {
                // если хоть какой-то узел выделен, добавляем элемент в зависимости от глубины
                if (selected != null) 
                {
                    switch (selected.Level)
                    {
                        case 0: newGroupName = playlistManager.AddGroup(selected.Index); break;
                        case 1: newGroupName = playlistManager.AddChannel((selected.Parent.Index, selected.Index)); break;
                        default: return;

                    }
                 
                }
                // создаем узел с полученным именем
                selected?.Nodes.Add(newGroupName);
            }
            catch (Exception e)
            {
                ShowErrorMessage(e.Message);
            }
            
            
        }

        public async Task SavePlaylist()
        {
            try
            {
                // определяем индекс выбранного плейлиста или присваем индекс по умолчанию
                int playlistIndex = 0;
                if (treeView1.SelectedNode != null)
                    playlistIndex = GetCoordinates(treeView1.SelectedNode)[0];

                saveFileDialog1.FileName = playlistManager.playlists[playlistIndex].Name;

                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                // получаем путь к сохраняемому файлу и получаем строку для записи в файл в отдельной задаче
                string fileName = saveFileDialog1.FileName;

                Task generationProcess = new Task(() => {
                    string fileContent = playlistManager.SavePlaylistAsFile(playlistIndex, fileName);
                    System.IO.File.WriteAllText(fileName, fileContent);
                });

                generationProcess.Start();
                await generationProcess;
            }
            catch (Exception e)
            {
                ShowErrorWindow(e.Message);
            }

            ShowErrorMessage("Save Successfull");
        }

        private void newGroupButton_Click(object sender, EventArgs e)
        {
            NewElement();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SavePlaylist();
        }

        // получение массива из индексов узла и всех его родительских узлов
        int[] GetCoordinates(TreeNode targetNode)
        {
            // создаем массив в длинной, равной глубине выбранного узла
            int[] coordinates = new int[targetNode.Level + 1];
            // записываем последовательно индексы узла и его родительских узлов
            int curStep = targetNode.Level;
            while (curStep >= 0)
            {
                coordinates[curStep] = targetNode.Index;
                try
                {
                    targetNode = targetNode.Parent;
                }
                catch (Exception)
                {
                    break;
                }
                curStep--;
            }

            return coordinates;
        }

        // проверяем выбранный канал на доступность
        // просто посылаем запрос на указанный адрес, и, если он успешен, считаем канал доступным
        private async void checkButton_Click(object sender, EventArgs e)
        {
            Channel currentChannel = playlistManager.CurrentPlaylist.CurrentChannel;
            CheckChannel(currentChannel.ChannelPath, true);
        }

        async Task<bool> CheckChannel(string checkPath, bool showMessage)
        {
            try
            {
                // осуществляем запрос и получаем поток
                using var cts = new CancellationTokenSource();
                // максимальное время на запрос
                int timeout = Configurator.currentConfig.CheckTimeoutTime;
                
                cts.CancelAfter(TimeSpan.FromSeconds(timeout));

                HttpResponseMessage response = await WebManager.client.GetAsync(checkPath, cts.Token);
                response.EnsureSuccessStatusCode();
                // string data = await response.Content.ReadAsStringAsync();
                // показываем сообщение
                if (showMessage)
                    ShowErrorWindow("Канал доступен");
            }
            catch (Exception)
            {
                // показываем сообщение
                if (showMessage)
                    ShowErrorWindow("Канал НЕдоступен");
                // если запрос неудачен, считаем, что канала недоступен
                return false;
            }

            return true;
        }

        // проверка каналов в дереве
        async Task CheckChannelGlobal(int[] coords)
        {
            try
            {
               
                // текущий канал для проверки временно отмечается серым
                treeView1.Nodes[coords[0]].Nodes[coords[1]].Nodes[coords[2]].BackColor = Color.Gray;

                bool status;
                var processor = CheckChannel(playlistManager.playlists[coords[0]].groupsList[coords[1]].channelsList[coords[2]].ChannelPath, false);
                status = await processor;
                // перекрашиваем в нужный цвет в зависимости от результата
                if (status)
                    treeView1.Nodes[coords[0]].Nodes[coords[1]].Nodes[coords[2]].BackColor = Color.Green;
                else treeView1.Nodes[coords[0]].Nodes[coords[1]].Nodes[coords[2]].BackColor = Color.Red;
            }
            catch (Exception e)
            {
                ShowErrorMessage(e.Message);
            }
            
        }

        private async void checkAllButton_Click(object sender, EventArgs e)
        {
            // останавливаем идущий процесс
            StopChecking();
            CancellationToken token = cancelTokenSource.Token;
            var processor = CheckAllChannels(token);
            await processor;
        }

        void StopChecking()
        {
            cancelTokenSource.Cancel();
            cancelTokenSource.Dispose();
            cancelTokenSource = new CancellationTokenSource();
        }

        async Task CheckAllChannels(CancellationToken token)
        {
            // определяем объект для проверки
            int[] checkCoords;
            if (treeView1.SelectedNode != null)
                checkCoords = GetCoordinates(treeView1.SelectedNode);
            // если никакой узел не выбран, проверяем первый плейлист по списку
            else checkCoords = new int[] { 0 };
            
            // проверяем выделенные объекты
            switch (checkCoords.Length)
            {
                // канал
                case 3:
                    var processor = CheckChannelGlobal(checkCoords);
                    
                    await processor;
                    break;
                // группу каналов
                case 2:
                    for (int k = 0; k < playlistManager.playlists[checkCoords[0]].groupsList[checkCoords[1]].channelsList.Count; k++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            ShowErrorMessage("Проверка остановлена");
                            return;
                        }

                        processor = CheckChannelGlobal(new int[] {checkCoords[0], checkCoords[1], k});
                        await processor;
                    }
                    break;
                // весь плейлист
                case 1:
                    for (int i = 0; i < playlistManager.playlists[checkCoords[0]].groupsList.Count; i++)
                    {
                        for (int k = 0; k < playlistManager.playlists[checkCoords[0]].groupsList[i].channelsList.Count; k++)
                        {
                            if (token.IsCancellationRequested)
                            {
                                ShowErrorMessage("Проверка остановлена");
                                return;
                            }

                            processor = CheckChannelGlobal(new int[] { checkCoords[0], i, k });
                            await processor;
                        }
                    }
                    break;
            }
            // проверяем доступность выбранного объекта

        }

        private void button1_Click(object sender, EventArgs e)
        {
            StopChecking();
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm(this);
            settingsForm.Show();
        }

        internal void CustomValuesUI()
        {
            // удаляем старые элементы
            foreach (var item in customValuesLabels)
            {
                channelGroup.Controls.Remove(item);
            }

            foreach (var item in customValuesText)
            {
                channelGroup.Controls.Remove(item);
            }

            int verticalPosition = 280;

            // добавляем новые элементы
            TextBox curText;
            Label curLabel;
            foreach (var item in Configurator.currentConfig.customValues)
            {
                // сначала текстовое поле
                curText = new TextBox();
                curText.Size = new Size(368, 23);
                curText.Location = new Point(162, verticalPosition);
                curText.Anchor = (AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top);
                customValuesText.Add(curText);
                channelGroup.Controls.Add(customValuesText[^1]);
                // название параметра
                curLabel = new Label();
                curLabel.Location = new Point(28, verticalPosition);
                curLabel.Text = item.Key;
                customValuesLabels.Add(curLabel);
                channelGroup.Controls.Add(customValuesLabels[^1]);
                
                verticalPosition += 30;
            }

            // добавляем отдельные параметр, описывающий дополнительные строки
            // сначала текстовое поле
            curText = new TextBox();
            curText.Size = new Size(368, 46);
            curText.Location = new Point(162, verticalPosition);
            curText.Multiline = true;
            curText.Anchor = (AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top);
            customValuesText.Add(curText);
            channelGroup.Controls.Add(customValuesText[^1]);
            // название параметра
            curLabel = new Label();
            curLabel.Location = new Point(28, verticalPosition);
            curLabel.Text = "Дополнительно";
            customValuesLabels.Add(curLabel);
            channelGroup.Controls.Add(customValuesLabels[^1]);

            foreach (Control ctrl in customValuesText)
            {
                
                    ctrl.KeyDown += TextChangedChecker;
                
            }
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            CopyNode();
        }

        public void CopyNode()
        {
            if (treeView1.SelectedNode == null)
                return;
            // кнопка вставки становится активной
            pasteButton.Enabled = true;
            if (treeView1.SelectedNode.Level != 0)
                bufferNode = treeView1.SelectedNode;
        }

        private void pasteButton_Click(object sender, EventArgs e)
        {
           PasteCopiedNode();
        }

        public void PasteCopiedNode()
        {
            if (bufferNode == null)
                return;

            if (treeView1.SelectedNode == null)
                return;

            TreeNode seleceted = treeView1.SelectedNode;
            PasteNode(ref bufferNode, ref seleceted, false);

            for (int i = 0; i < playlistManager.CurrentPlaylist.groupsList.Count; i++)
            {
                ShowErrorMessage($"{playlistManager.CurrentPlaylist.groupsList[i].Name} ");
            }
        }
    }

}
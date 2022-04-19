using System.Net;
using PlaylistEditor.Schedule;
using PlaylistEditor.VideoPlayers;
using PlaylistEditor.Search;

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

        private List<Label> customValuesLabels = new List<Label>();
        private List<TextBox> customValuesText = new List<TextBox>();

        TreeNode? bufferNode;

        internal static Form1 instance;
        internal TreeView MainTree { get { return treeView1; }}

        ISearcher searcher;
        string previousSearchText = "";
        ScheduleManager scheduleManager;

        internal enum NodeStatus
        {
            InProgress = 1,
            Available,
            PartialyAvailable,
            NotAvailable
        };

        public Form1()
        {
            InitializeComponent();
            //добавляем обработчик ошибок при генерации плейлистов
            playlistManager = new PlaylistManager(this);
            playlistManager.AddListener(ShowErrorMessage);
            //добавляем обработчики событий нажатий на плейлист, канал или группу
            OnPlaylistClick += VisualizePlaylist;
            OnGroupClick += VisualizeGroup;
            
            playlistManager.OnError += ShowErrorMessage;
            searcher = new BaseSearcher();
        }

        //вывод сообщений об ошибках
        void ShowErrorMessage(string message)
        {
            if (message == "")
                return;
            textBox1.Text += $"{DateTime.Now.ToString("T")}: {message}\r\n";
        }

        public void ShowErrorWindow(string message)
        {
            string caption = "Ошибка!";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(message, caption, buttons);
        }

        
        private async void Form1_Load(object sender, EventArgs e)
        {
            instance = this;
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
            toolTip1.SetToolTip(this.nextSearchResult, "Следующий результат");
            toolTip1.SetToolTip(this.previousSearchResult, "Предыдущий результат");
            toolTip1.SetToolTip(this.scheduleButton, "Показать расписание");
            // считываем конфигурацию и создаем строки под нестандартные параметры
            Configurator.ReadConfig();
            CustomValuesUI();
            // получаем иконки
            treeView1.ImageList = Icons.GetIconsList();
            WebManager.main = this;
            // проверяем состояние архива программы передач
            scheduleManager = new ScheduleManager(ShowErrorMessage);
            await LoadScheduleArchive();
        }

        internal async Task LoadScheduleArchive()
        {
            if (!Configurator.currentConfig.loadSchedule)
                return;

            loadingProgressBar.Value = 0;
            ShowErrorMessage("Проверка архива программы передач");
            string status = await scheduleManager.ScheduleCheck();
            loadingProgressBar.Value = loadingProgressBar.Maximum;
            ShowErrorMessage(status);
            new Task(HideProgressBar).Start();
        }

        public void ProgressUpdate()
        {
            if (!loadingProgressBar.Enabled)
                loadingProgressBar.Enabled = true;
            loadingProgressBar.PerformStep();
        }

        void HideProgressBar()
        {
            Thread.Sleep(2000);
            loadingProgressBar.Enabled = false;
        }

        private async void OpenListButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string fileName = openFileDialog1.FileName;
            string fileText = System.IO.File.ReadAllText(fileName); //считываем текст из файла

            //запускае параллельную задачу, в которой получаем плейлист и генерируем на его основе дерево
            await Task.Run(() => {
                playlistGenerator = GeneratePlaylistTree;
                playlistManager.playlists.Add(playlistManager.GeneratePlaylist(ref fileText, fileName)); //создаем объект плейлиста из полученных данных
                playlistManager.CurrentPlaylistIndex = playlistManager.playlists.Count - 1; // созданный плейлист становится текущим плейлистом
                playlistManager.GeneratePlaylistTree();   
                
            });
        

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
            TreeNode playlistNode = new TreeNode(name);
            playlistNode.ImageIndex = playlistNode.SelectedImageIndex = 5;
            treeView1.Nodes.Add(playlistNode);
        }

        //генерация узла существующей группы
        public void AddGroup(string name)
        {
            TreeNode groupNode = new TreeNode(name);
            groupNode.ImageIndex = groupNode.SelectedImageIndex = 6;
            treeView1.Nodes[playlistManager.CurrentPlaylistIndex].Nodes.Add(groupNode);
        }

        //генерация узла существующего канала для указанной группы
        public void AddChannel(string name, int groupIndex)
        {
            treeView1.Nodes[playlistManager.CurrentPlaylistIndex].Nodes[groupIndex].Nodes.Add(name);
        }

        // визуализация узла канала
        async public Task VisualizeChannel(int groupIndex, int channelIndex)
        {
            channelsCount.Text = "";
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
            //пытаемся загрузить изображение по ссылке и расписание в разных потоках
            var logoTask = LoadImage(currentChannel.LogoPath);
            var scheduleTask = LoadSchedule(currentChannel.Name);
            await logoTask;
            await scheduleTask;
            
        }

        async Task LoadImage(string logoPath)
        {
            try
            {
                logoPicture.Image = await Task.Run(() => WebManager.LoadImage(logoPath));
            }
            catch (Exception)
            {
                logoPicture.Image = null;
                ShowErrorMessage("Cannot load logo");
            }
        }

        async Task LoadSchedule(string channelName)
        {
            string msg = "";
            msg = await scheduleManager.LoadChannelSchedule(channelName);
            ShowErrorMessage(msg);
            // и находящийся в данный момент в эфире элемент
            ScheduleManager.ScheduleItem? curItem = scheduleManager.GetCurrentItem();
            if (curItem == null)
                currentItemLabel.Text = "";
            else currentItemLabel.Text = $"Сейчас в эфире: {curItem?.Name}";
        }

        public void VisualizeGroup(int groupIndex)
        {
            channelGroup.Visible = false;
            channelsCount.Text = treeView1.SelectedNode.Nodes.Count.ToString();
        }

        public void VisualizePlaylist()
        {
            channelGroup.Visible = false;
            int totalCount = 0;
            foreach (TreeNode item in treeView1.SelectedNode.Nodes)
            {
                totalCount += item.Nodes.Count;
            }
            channelsCount.Text = totalCount.ToString();
        }

        private void TextChangedChecker(object sender, EventArgs e)
        {
            saveChannel.Enabled = true;
        }
        
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
           
        }

        // отображаем информацию по каналу при нажатии на его узел в дереве
        private async void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (dragTime)
                return;

            //в зависимости от выбранного узла заполняем необходимые текстовые поля
            switch (e.Node?.Level)
            {
                case 0: playlistManager.CurrentPlaylistIndex = e.Node.Index; OnPlaylistClick?.Invoke(); break;
                case 1: playlistManager.CurrentPlaylistIndex = e.Node.Parent.Index; OnGroupClick?.Invoke(e.Node.Index); break;
                case 2: playlistManager.CurrentPlaylistIndex = e.Node.Parent.Parent.Index; await VisualizeChannel(e.Node.Parent.Index, e.Node.Index); break;
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
                    TreeManager.RenameElement(e.Node, e.Label);
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
                    logoPicture.Image = await Task.Run(() =>  WebManager.LoadImage(currentChannel.LogoPath));
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
                logoPicture.Image = await Task.Run(() => WebManager.LoadImage(logoLinkText.Text));
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
                dragTime = true; // включаем запрет на визуализацию информации об узлах
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
            // не будут выбираться узлы, которые ниже уровне перетаскиваемого
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
            dragTime = false;
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
                string success = TreeManager.PasteNode(ref draggedNode, ref targetNode, true);
            }
        }
                 

        // удаляем выбранный узел и информацию о нем
        public void DeleteNode()
        {
            if (treeView1.SelectedNode == null)
                return;
            TreeManager.DeleteNodeData(treeView1.SelectedNode);
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

        // добавление элемента
        public void NewElement()
        {
            TreeNode? selected = treeView1.SelectedNode;
            TreeManager.AddElement(selected);            
        }

        public async Task SavePlaylist()
        {
            try
            {
                // определяем индекс выбранного плейлиста или присваем индекс по умолчанию
                int playlistIndex = 0;
                if (treeView1.SelectedNode != null)
                    playlistIndex = TreeManager.GetCoordinates(treeView1.SelectedNode)[0];

                saveFileDialog1.FileName = playlistManager.playlists[playlistIndex].Name;

                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                // получаем путь к сохраняемому файлу и получаем строку для записи в файл в отдельной задаче
                string fileName = saveFileDialog1.FileName;

                Task generationProcess = new Task(() => {
                    string fileContent = playlistManager.SavePlaylistAsFile(playlistIndex);
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

        private async void saveButton_Click(object sender, EventArgs e)
        {
            await SavePlaylist();
        }


        // проверяем выбранный канал на доступность
        // просто посылаем запрос на указанный адрес, и, если он успешен, считаем канал доступным
        private async void checkButton_Click(object sender, EventArgs e)
        {
            int[] coords = TreeManager.GetCoordinates(treeView1.SelectedNode);
            string status = await Task.Run(() => WebManager.CheackAvailability(coords, true));
            ShowErrorWindow(status);
        }

        // проверяем все каналы выбранного элемента дерева
        private async void checkAllButton_Click(object sender, EventArgs e)
        {
            checkAllButton.Enabled = false;

            // получаем координаты выбранного узла; если ничего не выбрано,
            // используем первый плейлист из списка, если он есть
            int[] coords;
            if (treeView1.SelectedNode != null)
            {
                coords = TreeManager.GetCoordinates(treeView1.SelectedNode);
                treeView1.SelectedNode.ExpandAll();
            }
            else coords = new int[] { 0 };

            // запускаем основной поток проверки доступности 
            try
            {
                await Task.Run(() => WebManager.CheckGroupOfChannels(coords));
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
            finally
            {
                checkAllButton.Enabled = true; // делаем кнопку доступной по окончании
            }
            
        }

        void StopChecking()
        {
            WebManager.CancelProcess();
            checkAllButton.Enabled = true;
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
            TreeManager.PasteNode(ref bufferNode, ref seleceted, false);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void searchText_TextChanged(object sender, EventArgs e)
        {
            
            // получаем изначальную область поиска - или выделенный узел,
            // или все дерево, если ничего не выделено или выделен узел последнего уровня
            TreeNode? selected = treeView1.SelectedNode;
            TreeNodeCollection selectedCollection;
            if (selected == null || selected.Level == 2)
                selectedCollection = treeView1.Nodes;
            else selectedCollection = selected.Nodes;

            // получаем в поисковике список конечных элементов
            await searcher.InitializeSearch(selectedCollection);
            // и отбираем нужные
            int searched;
            searched = await searcher.Search(searchText.Text);
            // меняем текст под строкой поиска, чтобы указать число найденных элементов
            searchCount.Text = $"Найдено: 0/{searched}";
            previousSearchText = searched.ToString();
        }

        private void nextSearchResult_Click(object sender, EventArgs e)
        {
            TreeNode? result = searcher.Next;
            ShowSearchResult(result);
        }

        private void previousSearchResult_Click(object sender, EventArgs e)
        {
            TreeNode? result = searcher.Previous;
            ShowSearchResult(result);
        }

        private void ShowSearchResult(TreeNode? result)
        {
            if (result == null)
                return;

            treeView1.SelectedNode = result;
            searchCount.Text = $"Найдено: {searcher.CurrentIndex}/{previousSearchText}";
        }

        // устанавливает изображение для узла в зависимости от его доступности
        internal void MarkNode(TreeNode node, NodeStatus status)
        {
            node.ImageIndex = node.SelectedImageIndex = (int)status;
        }

        private void playChannel_Click(object sender, EventArgs e)
        {
            try
            {
                VLCModule.PlayChannel(channelLinkText.Text);
            }
            catch (Exception ex)
            {
                ShowErrorWindow(ex.Message);
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                ShowErrorMessage($"|{System.Convert.ToByte(channelLinkText.Text[^1])}|");
                VLCModule.PlayChannel(channelLinkText.Text);
            }
            catch (Exception ex)
            {
                ShowErrorWindow(ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ScheduleList scheduleList = new ScheduleList(this, scheduleManager);
            scheduleList.Show();
        }
    }

}
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

        public Form1()
        {
            InitializeComponent();
            //��������� ���������� ������ ��� ��������� ����������
            playlistManager = new PlaylistManager(this);
            playlistManager.AddListener(ShowErrorMessage);
            //��������� ����������� ������� ������� �� ��������, ����� ��� ������
            OnPlaylistClick += VisualizePlaylist;
            OnGroupClick += VisualizeGroup;
            OnChannelClick += VisualizeChannel;
        }

        //����� ��������� �� �������
        void ShowErrorMessage(string message)
        {
            textBox1.Text += $"{message} \n";
        }

        public void ShowErrorWindow(string message)
        {
            string caption = "������!";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(message, caption, buttons);
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            // ��������� ���������� ������� ������� ������ ��� ������� ���������� ���� ������ �������� � ������, ����� ������������ ������ ����������
            foreach (Control ctrl in this.channelGroup.Controls)
            {
                if (ctrl is TextBox tBox)
                {
                    (tBox).KeyDown += TextChangedChecker;
                }
            }

            // �������
            toolTip1.SetToolTip(this.deleteButton, "������� �������");
            toolTip1.SetToolTip(this.newGroupButton, "�������� �������");
            toolTip1.SetToolTip(this.OpenListButton, "������� ��������");
            toolTip1.SetToolTip(this.saveButton, "��������� ��������");
            toolTip1.SetToolTip(this.checkAllButton, "��������� ��� ������");
        }

        private async void OpenListButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string fileName = openFileDialog1.FileName;
            string fileText = System.IO.File.ReadAllText(fileName); //��������� ����� �� �����

            //�������� ������������ ������, � ������� �������� �������� � ���������� �� ��� ������ ������
            Task generationProcess = new Task(() => {
                playlistGenerator = GeneratePlaylistTree;
                playlistManager.playlists.Add(playlistManager.GeneratePlaylist(ref fileText, fileName)); //������� ������ ��������� �� ���������� ������
                playlistManager.CurrentPlaylistIndex = playlistManager.playlists.Count - 1; // ��������� �������� ���������� ������� ����������
                playlistManager.GeneratePlaylistTree();   
                
            });

           generationProcess.Start();            

        }

        public void GeneratePlaylistTree()
        {
            int currentPlaylist = playlistManager.CurrentPlaylistIndex;
            //������� �������� ���� ���������
            AddPlaylist(playlistManager.playlists[currentPlaylist].Name);
            //��������� � ������� ����� ��� ��������� � ������, ��������� �� ���� � ������
            for (int i = 0; i < playlistManager.playlists[currentPlaylist].groupsList.Count; i++)
            {
                AddGroup(playlistManager.playlists[currentPlaylist].groupsList[i].Name);
                for (int k = 0; k < playlistManager.playlists[currentPlaylist].groupsList[i].channelsList.Count; k++)
                {
                    AddChannel(playlistManager.playlists[currentPlaylist].groupsList[i].channelsList[k].Name, i);
                }
            }

        }

        //������� ������ ������� ��������� � ������� ���� ���������
        public void AddPlaylist(string name)
        {
            treeView1.Nodes.Add(name);
        }

        //��������� ���� ������������ ������
        public void AddGroup(string name)
        {
            // playlistManager.currentPlaylist?.FindGroup("", true);
            treeView1.Nodes[playlistManager.CurrentPlaylistIndex].Nodes.Add(name);
        }

        //��������� ���� ������������� ������ ��� ��������� ������
        public void AddChannel(string name, int groupIndex)
        {
            treeView1.Nodes[playlistManager.CurrentPlaylistIndex].Nodes[groupIndex].Nodes.Add(name);
        }

        // ������������ ���� ������
        async public void VisualizeChannel(int groupIndex, int channelIndex)
        {

            //������ ���������� ������ ���������� ���������
            saveChannel.Enabled = false;
            //���������� ������ ������������ ���������, �������� ���������� � ������
            channelGroup.Visible = true;

            playlistManager.CurrentPlaylist.CurrentChannelIndex = channelIndex;
            playlistManager.CurrentPlaylist.CurrentGroupIndex = groupIndex;
            Channel currentChannel = playlistManager.CurrentPlaylist.CurrentChannel;

            //��������� ��������� ������ ������� ������
            channelNameText.Text = currentChannel.Name;
            logoLinkText.Text = currentChannel.LogoPath;
            channelLinkText.Text = currentChannel.ChannelPath;
            groupText.Text = currentChannel.GroupName;
            durationText.Text = currentChannel.ChannelDuration;
            //�������� ��������� ����������� �� ������
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

                
        //�������� ����������� �� ������
        async Task<Image> LoadLogo(string logoLink)
        {
            try
            {
                // ������������ ������ � �������� �����
                HttpResponseMessage response = await WebManager.client.GetAsync(logoLink);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                return Image.FromStream(stream);              

            }
            catch (Exception)
            {
                // � ������ ������� ������� �������
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

        // ���������� ���������� �� ������ ��� ������� �� ��� ���� � ������
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (dragTime)
                return;

            //� ����������� �� ���������� ���� ��������� ����������� ��������� ����
            switch (e.Node?.Level)
            {
                case 0: playlistManager.CurrentPlaylistIndex = e.Node.Index; OnPlaylistClick?.Invoke(); break;
                case 1: playlistManager.CurrentPlaylistIndex = e.Node.Parent.Index; OnGroupClick?.Invoke(e.Node.Index); break;
                case 2: playlistManager.CurrentPlaylistIndex = e.Node.Parent.Parent.Index; OnChannelClick?.Invoke(e.Node.Parent.Index, e.Node.Index); break;
                default: break;
            }
                       
        }

        //�������������� ����� ����� ������
        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    switch (e.Node?.Level)
                    {
                        //���� ������������� �������� ���������, �� ������ ������ ��� � ��������������� �������
                        case 0: playlistManager.CurrentPlaylist.Name = e.Label; break;
                        //���� ������������� ������, �� ������ ��������� ��� �������� � ������� ����� ������ �� �������� �� ���� �������
                        case 1: playlistManager.CurrentPlaylist.groupsList[e.Node.Parent.Index].Name = e.Label; GroupRename(e.Node.Index, e.Label); break;
                        //���� ������������� �����, ������ ��� �������� � �������
                        case 2: playlistManager.CurrentPlaylist.groupsList[e.Node.Parent.Index].channelsList[e.Node.Index].Name = e.Label; channelNameText.Text = e.Label; break;
                        default: break;
                    }

                    e.Node?.EndEdit(false);
                }
                else
                {
                    // �������� �������������� � ������, ���� �������� �������� ������;
                    // ������ ��������� �� ������ � ������ ��������� ��������������
                    e.CancelEdit = true;
                    ShowErrorMessage("�������� �� ����� ���� ������");
                    e.Node.BeginEdit();
                }
            }
        }

        //��������� ����� ������ ��� ���� ������� � ����������������� ������
        private void GroupRename(int groupIndex, string newGroupName)
        {
            for (int i = 0; i < playlistManager.CurrentPlaylist.groupsList[groupIndex].channelsList.Count; i++)
                playlistManager.CurrentPlaylist.groupsList[groupIndex].channelsList[i].GroupName = newGroupName;
            
        }

        // ��������� � ������ ���������� �� ���� ��������� �����
        async private void SaveChannelData()
        {
            // ������� ����� ������ � �������
            Channel currentChannel = new Channel(channelLinkText.Text, channelNameText.Text, logoLinkText.Text, groupText.Text, durationText.Text);
            int newGroupIndex = -1;
            //��������� ����������� �������� � �������� ������ ������, ���� ����� ������� ����� ����
            try
            {
               newGroupIndex =  playlistManager.CurrentPlaylist.SaveChannel(currentChannel);
            }
            catch (Exception e)
            {
                ShowErrorWindow(e.Message);
                return;
            }

            //� ������ ������ �������� �������� ������: ������� ��� ����
            treeView1.SelectedNode.Text = channelNameText.Text;
            //������� ����� ������, ���� ����������
            if(newGroupIndex != -1)
            {
                // ���������, ����� �� ��������� ����� ���� ������, ��� ����� ������������
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
                // ������� ������ ���� � ���������� ���
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
                // � �������� ����� ���
                treeView1.SelectedNode = oldNode;
                
            }
            //���� �������� � ������ ������ ���, ������������ �������� � ��������� ������� ����������
            else
            {
                // ��������� ����� �������
                try
                {
                    logoPicture.Image = await Task.Run(() => LoadLogo(currentChannel.LogoPath));
                }
                catch (Exception)
                {
                    logoPicture.Image = null;
                    ShowErrorMessage("Cannot load logo");
                }
                // �������� � ������ ������������ ������� ����� �� ����� � ���������� �������
                durationText.Text = currentChannel.ChannelDuration;
            }

            //�������
            OnChannelSaveData?.Invoke();
        }

        private void saveChannel_Click(object sender, EventArgs e)
        {
            SaveChannelData();
            
        }

        //�������� �������� �� ����� ������
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
            //���������� ��������� ����
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
            // �������� ���������� ��������� � ������
            Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));

            // �������� ���� � ����� �����
            TreeNode? targetNode = treeView1.GetNodeAt(targetPoint);

            // �������� ��������������� ����
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            // �������� ���� �� ����������� ��������� ���������
            if (draggedNode.Level >= targetNode?.Level)
                treeView1.SelectedNode = targetNode;
        }

        /* �������������� �����
        ���������� ���� ����� ��� ������ ������ ������, ��� ������ ������ �� ������� ����
        ��� ����������� ������ ������ ������ ���������� ������� �����
        ��� ����������� �� ������� ���� ���������� ����������� ���� � ������ ��������� 
        */
        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
             // �������� ���������� ����, ��� ������� ��������� ����
            Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));

            // �������� ���� � ����� �����
            TreeNode targetNode = treeView1.GetNodeAt(targetPoint);

            // �������� ��������������� ����
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            if (targetNode == null)
                return;

            // ����������, ��� ��������������� � ��������� ���� - �� ���� � �� ��
            if (!draggedNode.Equals(targetNode) && e.Effect == DragDropEffects.Move)
            {
                // � ����������� �� ������ ������� ������������ ���� 
                switch (draggedNode.Level)
                {
                    // �������� - ������
                    case 0: return;
                    // ������
                    case 1:
                        {
                            // � ����������� �� ������� �������� ����
                            switch (targetNode.Level)
                            {
                                // ���� ��� �� ������ ��������, ��������� � ����� ����� ��������� ������� ������
                                case 0:
                                    {
                                        if (targetNode.Index != draggedNode.Parent.Index)
                                        {
                                            // ����������� �������� � ���������
                                            playlistManager.MoveElement(playlistManager.playlists[draggedNode.Parent.Index].groupsList[draggedNode.Index], draggedNode.Parent.Index, targetNode.Index);
                                            MoveNode(ref draggedNode, ref targetNode);
                                        }
                                        else return;
                                        break;
                                    }
                                // ���� ��� ������ ������, �� ������ ����������� ������ �� ����� ������� ������
                                case 1:
                                    {
                                        // ����������� �������� � ���������
                                        
                                        SwapNodes(ref draggedNode, ref targetNode, 1);
                                        break;
                                    }
                                default: return;
                            }
                            break;
                        }
                    // �����
                    case 2:
                        {
                            // � ����������� �� ������� �������� ����
                            switch (targetNode.Level)
                            {
                                // ���� ��� �� ������ ���������, ��������� � ����� ���� ��������� ������� �����
                                case 1:
                                    {
                                        if (targetNode.Index != draggedNode.Parent.Index)
                                        {
                                            // ����������� �������� � ���������
                                            playlistManager.MoveElement(draggedNode.Index, (draggedNode.Parent.Parent.Index, draggedNode.Parent.Index), (targetNode.Parent.Index, targetNode.Index));
                                            MoveNode(ref draggedNode, ref targetNode);
                                        }
                                        else return;
                                        break;
                                    }
                                // ���� ��� ������ �����, �� ������ ����������� ������ �� ����� ������� ������
                                case 2:
                                    {
                                        // ����������� �������� � ���������
                                        SwapNodes(ref draggedNode, ref targetNode, 2);
                                        break;
                                    }
                                default: return;
                            }
                            break;
                        }

                }

                dragTime = false;
            }
        }

        void SwapNodes(ref TreeNode draggedNode, ref TreeNode targetNode, int level)
        {
            // ����������� �����
            TreeNode clondeNode = (TreeNode)draggedNode.Clone();
            int[] oldNode = GetCoordinates(draggedNode);
            // ���� ���� ��������������� �����, �� ���������� �� ����� ����������� ����, � ���������� ��������� ����
            if (draggedNode.Index > targetNode.Index || draggedNode.Parent.Index > targetNode.Parent.Index)
                targetNode.Parent.Nodes.Insert(targetNode.Index, clondeNode);
            // ���� ������, �� ���������������� ���� ���������� ����� �����������
            else targetNode.Parent.Nodes.Insert(targetNode.Index + 1, clondeNode);
            //������� ��������������� ���� � �������� ����������� �����
            draggedNode.Remove();

            treeView1.SelectedNode = clondeNode;
            int[] newNode = GetCoordinates(treeView1.SelectedNode);

            // ShowErrorMessage($"{oldNode[0]} {oldNode[1]} {oldNode[2]} : {newNode[0]} {newNode[1]} {newNode[2]}");

            if (level == 2)
            playlistManager.MoveElement(oldNode, newNode);
            else playlistManager.MoveElement(true, oldNode, newNode);
        }

        void MoveNode(ref TreeNode draggedNode, ref TreeNode targetNode) 
        {
            // ����������� �����
            draggedNode.Remove();
            targetNode.Nodes.Add(draggedNode);
        }

        // ������� ��������� ���� � ���������� � ���
        public void DeleteNode()
        {
            if (treeView1.SelectedNode == null)
                throw new Exception("No node selected");
            // ������� ������ � �������, ������ ������� ���������� ����
            TreeNode curNode = treeView1.SelectedNode;
            int[] coordinates = GetCoordinates(curNode);
            // ������� ������ �� ���������
            playlistManager.DeleteElement(coordinates);
            // ������� ����
            treeView1.SelectedNode.Remove();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            DeleteNode();
        }

        // ��������� ������� ������ � ������
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
                // ���� ���� �����-�� ���� �������, ��������� ������� � ����������� �� �������
                if (selected != null) 
                {
                    switch (selected.Level)
                    {
                        case 0: newGroupName = playlistManager.AddGroup(selected.Index); break;
                        case 1: newGroupName = playlistManager.AddChannel((selected.Parent.Index, selected.Index)); break;
                        default: return;

                    }
                 
                }
                // ������� ���� � ���������� ������
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
                // ���������� ������ ���������� ��������� ��� �������� ������ �� ���������
                int playlistIndex = 0;
                if (treeView1.SelectedNode != null)
                    playlistIndex = GetCoordinates(treeView1.SelectedNode)[0];

                saveFileDialog1.FileName = playlistManager.playlists[playlistIndex].Name;

                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                // �������� ���� � ������������ ����� � �������� ������ ��� ������ � ���� � ��������� ������
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

        // ��������� ������� �� �������� ���� � ���� ��� ������������ �����
        int[] GetCoordinates(TreeNode targetNode)
        {
            // ������� ������ � �������, ������ ������� ���������� ����
            int[] coordinates = new int[targetNode.Level + 1];
            // ���������� ��������������� ������� ���� � ��� ������������ �����
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

        // ��������� ��������� ����� �� �����������
        // ������ �������� ������ �� ��������� �����, �, ���� �� �������, ������� ����� ���������
        private void checkButton_Click(object sender, EventArgs e)
        {
            Channel currentChannel = playlistManager.CurrentPlaylist.CurrentChannel;
            CheckChannel(currentChannel.ChannelPath, true);
        }

        async Task<bool> CheckChannel(string checkPath, bool showMessage)
        {
            try
            {
                // ������������ ������ � �������� �����
                using var cts = new CancellationTokenSource();
                int timeout = 5;
                int.TryParse(timeoutText.Text, out timeout);

                if (timeout <= 1)
                {
                    timeout = 1;
                    timeoutText.Text = "1";
                }

                cts.CancelAfter(TimeSpan.FromSeconds(timeout));

                HttpResponseMessage response = await WebManager.client.GetAsync(checkPath, cts.Token);
                response.EnsureSuccessStatusCode();
                // string data = await response.Content.ReadAsStringAsync();
                // ���������� ���������
                if (showMessage)
                    ShowErrorWindow("����� ��������");
            }
            catch (Exception)
            {
                // ���������� ���������
                if (showMessage)
                    ShowErrorWindow("����� ����������");
                // ���� ������ ��������, �������, ��� ������ ����������
                return false;
            }

            return true;
        }

        private void checkAllButton_Click(object sender, EventArgs e)
        {
            CheckAllChannels();
        }

        async void CheckAllChannels()
        {
            // ���������� ��������� ��������
            int playlistIndex = 0;
            if (treeView1.SelectedNode != null)
                playlistIndex = GetCoordinates(treeView1.SelectedNode)[0];
            // ���������� ��� ������ � ��������� �����������
            try
            {
                for (int i = 0; i < playlistManager.playlists[playlistIndex].groupsList.Count; i++)
                {
                    ShowErrorMessage($"{playlistManager.playlists[playlistIndex].groupsList.Count}");
                    for (int k = 0; k < playlistManager.playlists[playlistIndex].groupsList[i].channelsList.Count; k++)
                    {
                        
                        // ������� ����� ��� �������� �������� ���������� �����
                        treeView1.Nodes[playlistIndex].Nodes[i].Nodes[k].BackColor = Color.Gray;

                        bool status;
                        var processor = CheckChannel(playlistManager.playlists[playlistIndex].groupsList[i].channelsList[k].ChannelPath, false);
                        status = await processor;
                        // ������������� � ������ ���� � ����������� �� ����������
                        if (status)
                            treeView1.Nodes[playlistIndex].Nodes[i].Nodes[k].BackColor = Color.Green;
                        else treeView1.Nodes[playlistIndex].Nodes[i].Nodes[k].BackColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }

            
        }
    }

}
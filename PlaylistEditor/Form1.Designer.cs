namespace PlaylistEditor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.OpenListButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.logoPicture = new System.Windows.Forms.PictureBox();
            this.channelNameText = new System.Windows.Forms.TextBox();
            this.logoLinkText = new System.Windows.Forms.TextBox();
            this.channelLinkText = new System.Windows.Forms.TextBox();
            this.logoHeader = new System.Windows.Forms.Label();
            this.channelNameHeader = new System.Windows.Forms.Label();
            this.logoLinkHeader = new System.Windows.Forms.Label();
            this.channeLinkHeader = new System.Windows.Forms.Label();
            this.groupHeader = new System.Windows.Forms.Label();
            this.groupText = new System.Windows.Forms.TextBox();
            this.durationHeader = new System.Windows.Forms.Label();
            this.durationText = new System.Windows.Forms.TextBox();
            this.channelGroup = new System.Windows.Forms.GroupBox();
            this.currentItemLabel = new System.Windows.Forms.Label();
            this.scheduleButton = new System.Windows.Forms.Button();
            this.playChannel = new System.Windows.Forms.Button();
            this.checkButton = new System.Windows.Forms.Button();
            this.reloadLogo = new System.Windows.Forms.Button();
            this.saveChannel = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.newGroupButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.checkAllButton = new System.Windows.Forms.Button();
            this.stopCheckingButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.pasteButton = new System.Windows.Forms.Button();
            this.copyButton = new System.Windows.Forms.Button();
            this.channelsCountHeader = new System.Windows.Forms.Label();
            this.channelsCount = new System.Windows.Forms.Label();
            this.searchHeader = new System.Windows.Forms.Label();
            this.searchText = new System.Windows.Forms.TextBox();
            this.searchCount = new System.Windows.Forms.Label();
            this.nextSearchResult = new System.Windows.Forms.Button();
            this.previousSearchResult = new System.Windows.Forms.Button();
            this.loadingProgressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.logoPicture)).BeginInit();
            this.channelGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenListButton
            // 
            this.OpenListButton.Image = global::PlaylistEditor.Properties.Resources.OpenFolder;
            this.OpenListButton.Location = new System.Drawing.Point(24, 28);
            this.OpenListButton.Name = "OpenListButton";
            this.OpenListButton.Size = new System.Drawing.Size(24, 24);
            this.OpenListButton.TabIndex = 0;
            this.OpenListButton.UseVisualStyleBackColor = true;
            this.OpenListButton.Click += new System.EventHandler(this.OpenListButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Txt files|*.txt|m3u files|*.m3u";
            this.openFileDialog1.FilterIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(24, 578);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(819, 44);
            this.textBox1.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.HideSelection = false;
            this.treeView1.LabelEdit = true;
            this.treeView1.Location = new System.Drawing.Point(24, 58);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(207, 464);
            this.treeView1.TabIndex = 2;
            this.treeView1.Tag = "";
            this.treeView1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_AfterLabelEdit);
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.treeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView1_DragOver);
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
            // 
            // logoPicture
            // 
            this.logoPicture.Location = new System.Drawing.Point(28, 43);
            this.logoPicture.Name = "logoPicture";
            this.logoPicture.Size = new System.Drawing.Size(128, 128);
            this.logoPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoPicture.TabIndex = 3;
            this.logoPicture.TabStop = false;
            // 
            // channelNameText
            // 
            this.channelNameText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.channelNameText.Location = new System.Drawing.Point(162, 43);
            this.channelNameText.Name = "channelNameText";
            this.channelNameText.Size = new System.Drawing.Size(435, 23);
            this.channelNameText.TabIndex = 4;
            // 
            // logoLinkText
            // 
            this.logoLinkText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logoLinkText.Location = new System.Drawing.Point(162, 93);
            this.logoLinkText.Name = "logoLinkText";
            this.logoLinkText.Size = new System.Drawing.Size(405, 23);
            this.logoLinkText.TabIndex = 5;
            // 
            // channelLinkText
            // 
            this.channelLinkText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.channelLinkText.Location = new System.Drawing.Point(162, 200);
            this.channelLinkText.Name = "channelLinkText";
            this.channelLinkText.Size = new System.Drawing.Size(405, 23);
            this.channelLinkText.TabIndex = 6;
            // 
            // logoHeader
            // 
            this.logoHeader.Location = new System.Drawing.Point(28, 21);
            this.logoHeader.Name = "logoHeader";
            this.logoHeader.Size = new System.Drawing.Size(128, 15);
            this.logoHeader.TabIndex = 7;
            this.logoHeader.Text = "Логотип";
            this.logoHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // channelNameHeader
            // 
            this.channelNameHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.channelNameHeader.Location = new System.Drawing.Point(162, 17);
            this.channelNameHeader.Name = "channelNameHeader";
            this.channelNameHeader.Size = new System.Drawing.Size(435, 23);
            this.channelNameHeader.TabIndex = 8;
            this.channelNameHeader.Text = "Название канала";
            this.channelNameHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // logoLinkHeader
            // 
            this.logoLinkHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logoLinkHeader.Location = new System.Drawing.Point(162, 67);
            this.logoLinkHeader.Name = "logoLinkHeader";
            this.logoLinkHeader.Size = new System.Drawing.Size(435, 23);
            this.logoLinkHeader.TabIndex = 9;
            this.logoLinkHeader.Text = "Ссылка на логотип";
            this.logoLinkHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // channeLinkHeader
            // 
            this.channeLinkHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.channeLinkHeader.Location = new System.Drawing.Point(162, 174);
            this.channeLinkHeader.Name = "channeLinkHeader";
            this.channeLinkHeader.Size = new System.Drawing.Size(435, 23);
            this.channeLinkHeader.TabIndex = 10;
            this.channeLinkHeader.Text = "Ссылка на канал/трек";
            this.channeLinkHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupHeader
            // 
            this.groupHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupHeader.Location = new System.Drawing.Point(165, 117);
            this.groupHeader.Name = "groupHeader";
            this.groupHeader.Size = new System.Drawing.Size(432, 23);
            this.groupHeader.TabIndex = 12;
            this.groupHeader.Text = "Категория";
            this.groupHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupText
            // 
            this.groupText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupText.Location = new System.Drawing.Point(162, 143);
            this.groupText.Name = "groupText";
            this.groupText.Size = new System.Drawing.Size(435, 23);
            this.groupText.TabIndex = 11;
            // 
            // durationHeader
            // 
            this.durationHeader.Location = new System.Drawing.Point(31, 174);
            this.durationHeader.Name = "durationHeader";
            this.durationHeader.Size = new System.Drawing.Size(125, 23);
            this.durationHeader.TabIndex = 14;
            this.durationHeader.Text = "Длительность";
            this.durationHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // durationText
            // 
            this.durationText.Location = new System.Drawing.Point(28, 200);
            this.durationText.MaxLength = 6;
            this.durationText.Name = "durationText";
            this.durationText.Size = new System.Drawing.Size(128, 23);
            this.durationText.TabIndex = 13;
            // 
            // channelGroup
            // 
            this.channelGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.channelGroup.AutoSize = true;
            this.channelGroup.Controls.Add(this.currentItemLabel);
            this.channelGroup.Controls.Add(this.scheduleButton);
            this.channelGroup.Controls.Add(this.playChannel);
            this.channelGroup.Controls.Add(this.checkButton);
            this.channelGroup.Controls.Add(this.reloadLogo);
            this.channelGroup.Controls.Add(this.saveChannel);
            this.channelGroup.Controls.Add(this.channelNameHeader);
            this.channelGroup.Controls.Add(this.durationHeader);
            this.channelGroup.Controls.Add(this.logoPicture);
            this.channelGroup.Controls.Add(this.durationText);
            this.channelGroup.Controls.Add(this.channelNameText);
            this.channelGroup.Controls.Add(this.groupHeader);
            this.channelGroup.Controls.Add(this.logoLinkText);
            this.channelGroup.Controls.Add(this.groupText);
            this.channelGroup.Controls.Add(this.channelLinkText);
            this.channelGroup.Controls.Add(this.channeLinkHeader);
            this.channelGroup.Controls.Add(this.logoHeader);
            this.channelGroup.Controls.Add(this.logoLinkHeader);
            this.channelGroup.Location = new System.Drawing.Point(237, 58);
            this.channelGroup.Name = "channelGroup";
            this.channelGroup.Size = new System.Drawing.Size(606, 464);
            this.channelGroup.TabIndex = 15;
            this.channelGroup.TabStop = false;
            this.channelGroup.Text = "Информация о канале";
            this.channelGroup.Visible = false;
            // 
            // currentItemLabel
            // 
            this.currentItemLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentItemLabel.Location = new System.Drawing.Point(0, 417);
            this.currentItemLabel.Name = "currentItemLabel";
            this.currentItemLabel.Size = new System.Drawing.Size(607, 23);
            this.currentItemLabel.TabIndex = 31;
            this.currentItemLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scheduleButton
            // 
            this.scheduleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scheduleButton.Image = global::PlaylistEditor.Properties.Resources.ListTimePicker;
            this.scheduleButton.Location = new System.Drawing.Point(573, 12);
            this.scheduleButton.Name = "scheduleButton";
            this.scheduleButton.Size = new System.Drawing.Size(24, 24);
            this.scheduleButton.TabIndex = 30;
            this.scheduleButton.UseVisualStyleBackColor = true;
            this.scheduleButton.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // playChannel
            // 
            this.playChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.playChannel.Image = global::PlaylistEditor.Properties.Resources.Play;
            this.playChannel.Location = new System.Drawing.Point(573, 199);
            this.playChannel.Name = "playChannel";
            this.playChannel.Size = new System.Drawing.Size(24, 23);
            this.playChannel.TabIndex = 18;
            this.playChannel.UseVisualStyleBackColor = true;
            this.playChannel.Click += new System.EventHandler(this.playChannel_Click);
            // 
            // checkButton
            // 
            this.checkButton.Location = new System.Drawing.Point(28, 240);
            this.checkButton.Name = "checkButton";
            this.checkButton.Size = new System.Drawing.Size(174, 23);
            this.checkButton.TabIndex = 17;
            this.checkButton.Text = "Проверить доступность";
            this.checkButton.UseVisualStyleBackColor = true;
            this.checkButton.Click += new System.EventHandler(this.checkButton_Click);
            // 
            // reloadLogo
            // 
            this.reloadLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.reloadLogo.Image = ((System.Drawing.Image)(resources.GetObject("reloadLogo.Image")));
            this.reloadLogo.Location = new System.Drawing.Point(573, 92);
            this.reloadLogo.Name = "reloadLogo";
            this.reloadLogo.Size = new System.Drawing.Size(24, 23);
            this.reloadLogo.TabIndex = 16;
            this.reloadLogo.UseVisualStyleBackColor = true;
            this.reloadLogo.Click += new System.EventHandler(this.reloadLogo_Click);
            // 
            // saveChannel
            // 
            this.saveChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveChannel.Enabled = false;
            this.saveChannel.Location = new System.Drawing.Point(423, 240);
            this.saveChannel.Name = "saveChannel";
            this.saveChannel.Size = new System.Drawing.Size(174, 23);
            this.saveChannel.TabIndex = 15;
            this.saveChannel.Text = "Сохранить изменения";
            this.saveChannel.UseVisualStyleBackColor = true;
            this.saveChannel.Click += new System.EventHandler(this.saveChannel_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Image = global::PlaylistEditor.Properties.Resources.Delete;
            this.deleteButton.Location = new System.Drawing.Point(207, 28);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(24, 24);
            this.deleteButton.TabIndex = 16;
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // newGroupButton
            // 
            this.newGroupButton.Image = global::PlaylistEditor.Properties.Resources.NewDocument;
            this.newGroupButton.Location = new System.Drawing.Point(177, 28);
            this.newGroupButton.Name = "newGroupButton";
            this.newGroupButton.Size = new System.Drawing.Size(24, 24);
            this.newGroupButton.TabIndex = 17;
            this.newGroupButton.UseVisualStyleBackColor = true;
            this.newGroupButton.Click += new System.EventHandler(this.newGroupButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Image = global::PlaylistEditor.Properties.Resources.Save;
            this.saveButton.Location = new System.Drawing.Point(54, 28);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(24, 24);
            this.saveButton.TabIndex = 18;
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Txt files|*.txt";
            // 
            // checkAllButton
            // 
            this.checkAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkAllButton.Image = global::PlaylistEditor.Properties.Resources.DownloadDocument;
            this.checkAllButton.Location = new System.Drawing.Point(790, 28);
            this.checkAllButton.Name = "checkAllButton";
            this.checkAllButton.Size = new System.Drawing.Size(24, 24);
            this.checkAllButton.TabIndex = 19;
            this.checkAllButton.UseVisualStyleBackColor = true;
            this.checkAllButton.Click += new System.EventHandler(this.checkAllButton_Click);
            // 
            // stopCheckingButton
            // 
            this.stopCheckingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopCheckingButton.Image = global::PlaylistEditor.Properties.Resources.StopOutline;
            this.stopCheckingButton.Location = new System.Drawing.Point(760, 28);
            this.stopCheckingButton.Name = "stopCheckingButton";
            this.stopCheckingButton.Size = new System.Drawing.Size(24, 24);
            this.stopCheckingButton.TabIndex = 21;
            this.stopCheckingButton.UseVisualStyleBackColor = true;
            this.stopCheckingButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsButton.Image = global::PlaylistEditor.Properties.Resources.Settings;
            this.settingsButton.Location = new System.Drawing.Point(820, 28);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(24, 24);
            this.settingsButton.TabIndex = 22;
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // pasteButton
            // 
            this.pasteButton.Enabled = false;
            this.pasteButton.Image = global::PlaylistEditor.Properties.Resources.Paste;
            this.pasteButton.Location = new System.Drawing.Point(147, 28);
            this.pasteButton.Name = "pasteButton";
            this.pasteButton.Size = new System.Drawing.Size(24, 24);
            this.pasteButton.TabIndex = 23;
            this.pasteButton.UseVisualStyleBackColor = true;
            this.pasteButton.Click += new System.EventHandler(this.pasteButton_Click);
            // 
            // copyButton
            // 
            this.copyButton.Image = global::PlaylistEditor.Properties.Resources.Copy;
            this.copyButton.Location = new System.Drawing.Point(117, 28);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(24, 24);
            this.copyButton.TabIndex = 24;
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // channelsCountHeader
            // 
            this.channelsCountHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.channelsCountHeader.Location = new System.Drawing.Point(24, 528);
            this.channelsCountHeader.Name = "channelsCountHeader";
            this.channelsCountHeader.Size = new System.Drawing.Size(195, 15);
            this.channelsCountHeader.TabIndex = 18;
            this.channelsCountHeader.Text = "Каналов в выбранном элементе: ";
            this.channelsCountHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.channelsCountHeader.Click += new System.EventHandler(this.label1_Click);
            // 
            // channelsCount
            // 
            this.channelsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.channelsCount.Location = new System.Drawing.Point(216, 525);
            this.channelsCount.Name = "channelsCount";
            this.channelsCount.Size = new System.Drawing.Size(65, 15);
            this.channelsCount.TabIndex = 25;
            this.channelsCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // searchHeader
            // 
            this.searchHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.searchHeader.Location = new System.Drawing.Point(584, 528);
            this.searchHeader.Name = "searchHeader";
            this.searchHeader.Size = new System.Drawing.Size(45, 15);
            this.searchHeader.TabIndex = 26;
            this.searchHeader.Text = "Поиск:";
            this.searchHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // searchText
            // 
            this.searchText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.searchText.Location = new System.Drawing.Point(635, 525);
            this.searchText.MaxLength = 20;
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(147, 23);
            this.searchText.TabIndex = 18;
            this.searchText.TextChanged += new System.EventHandler(this.searchText_TextChanged);
            // 
            // searchCount
            // 
            this.searchCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.searchCount.Location = new System.Drawing.Point(655, 551);
            this.searchCount.Name = "searchCount";
            this.searchCount.Size = new System.Drawing.Size(188, 15);
            this.searchCount.TabIndex = 27;
            this.searchCount.Text = "Найдено:";
            this.searchCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nextSearchResult
            // 
            this.nextSearchResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nextSearchResult.Image = global::PlaylistEditor.Properties.Resources.NextFrame;
            this.nextSearchResult.Location = new System.Drawing.Point(819, 525);
            this.nextSearchResult.Name = "nextSearchResult";
            this.nextSearchResult.Size = new System.Drawing.Size(24, 24);
            this.nextSearchResult.TabIndex = 28;
            this.nextSearchResult.UseVisualStyleBackColor = true;
            this.nextSearchResult.Click += new System.EventHandler(this.nextSearchResult_Click);
            // 
            // previousSearchResult
            // 
            this.previousSearchResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.previousSearchResult.Image = global::PlaylistEditor.Properties.Resources.PreviousFrame;
            this.previousSearchResult.Location = new System.Drawing.Point(790, 525);
            this.previousSearchResult.Name = "previousSearchResult";
            this.previousSearchResult.Size = new System.Drawing.Size(24, 24);
            this.previousSearchResult.TabIndex = 29;
            this.previousSearchResult.UseVisualStyleBackColor = true;
            this.previousSearchResult.Click += new System.EventHandler(this.previousSearchResult_Click);
            // 
            // loadingProgressBar
            // 
            this.loadingProgressBar.Enabled = false;
            this.loadingProgressBar.Location = new System.Drawing.Point(304, 525);
            this.loadingProgressBar.Name = "loadingProgressBar";
            this.loadingProgressBar.Size = new System.Drawing.Size(226, 23);
            this.loadingProgressBar.Step = 1;
            this.loadingProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.loadingProgressBar.TabIndex = 30;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 634);
            this.Controls.Add(this.loadingProgressBar);
            this.Controls.Add(this.previousSearchResult);
            this.Controls.Add(this.nextSearchResult);
            this.Controls.Add(this.searchCount);
            this.Controls.Add(this.searchText);
            this.Controls.Add(this.searchHeader);
            this.Controls.Add(this.channelsCount);
            this.Controls.Add(this.channelsCountHeader);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.pasteButton);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.stopCheckingButton);
            this.Controls.Add(this.checkAllButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.newGroupButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.channelGroup);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.OpenListButton);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PlaylistEditor";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.logoPicture)).EndInit();
            this.channelGroup.ResumeLayout(false);
            this.channelGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button OpenListButton;
        private OpenFileDialog openFileDialog1;
        private TextBox textBox1;
        private TreeView treeView1;
        private PictureBox logoPicture;
        private TextBox channelNameText;
        private TextBox logoLinkText;
        private TextBox channelLinkText;
        private Label logoHeader;
        private Label channelNameHeader;
        private Label logoLinkHeader;
        private Label channeLinkHeader;
        private Label groupHeader;
        private TextBox groupText;
        private Label durationHeader;
        private TextBox durationText;
        private GroupBox channelGroup;
        private Button saveChannel;
        private Button reloadLogo;
        private Button deleteButton;
        private ToolTip toolTip1;
        private Button newGroupButton;
        private Button saveButton;
        private SaveFileDialog saveFileDialog1;
        private Button checkButton;
        private Button checkAllButton;
        private Button stopCheckingButton;
        private Button settingsButton;
        private Button pasteButton;
        private Button copyButton;
        private Label channelsCountHeader;
        private Label channelsCount;
        private Label searchHeader;
        private TextBox searchText;
        private Label searchCount;
        private Button nextSearchResult;
        private Button previousSearchResult;
        private Button playChannel;
        private Button scheduleButton;
        private Label currentItemLabel;
        private ProgressBar loadingProgressBar;
    }
}
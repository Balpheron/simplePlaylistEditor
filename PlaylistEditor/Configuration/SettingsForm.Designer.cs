namespace PlaylistEditor
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.timeoutTextBox = new System.Windows.Forms.TextBox();
            this.timeoutLabel = new System.Windows.Forms.Label();
            this.customParamsHeader = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.headerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vlcPath = new System.Windows.Forms.TextBox();
            this.vlcPathHeader = new System.Windows.Forms.Label();
            this.settingsButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.scheduleHeader = new System.Windows.Forms.Label();
            this.scheduleText = new System.Windows.Forms.TextBox();
            this.scheduleCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // timeoutTextBox
            // 
            this.timeoutTextBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.timeoutTextBox.Location = new System.Drawing.Point(200, 20);
            this.timeoutTextBox.MaxLength = 2;
            this.timeoutTextBox.Name = "timeoutTextBox";
            this.timeoutTextBox.Size = new System.Drawing.Size(37, 23);
            this.timeoutTextBox.TabIndex = 0;
            this.timeoutTextBox.Text = "5";
            // 
            // timeoutLabel
            // 
            this.timeoutLabel.AutoSize = true;
            this.timeoutLabel.Location = new System.Drawing.Point(10, 23);
            this.timeoutLabel.Name = "timeoutLabel";
            this.timeoutLabel.Size = new System.Drawing.Size(184, 15);
            this.timeoutLabel.TabIndex = 1;
            this.timeoutLabel.Text = "Максимальное время проверки";
            // 
            // customParamsHeader
            // 
            this.customParamsHeader.AutoSize = true;
            this.customParamsHeader.Location = new System.Drawing.Point(10, 240);
            this.customParamsHeader.Name = "customParamsHeader";
            this.customParamsHeader.Size = new System.Drawing.Size(163, 15);
            this.customParamsHeader.TabIndex = 3;
            this.customParamsHeader.Text = "Дополнительные парамеры";
            this.customParamsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.headerColumn,
            this.dataColumn});
            this.dataGridView1.Location = new System.Drawing.Point(14, 267);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(457, 289);
            this.dataGridView1.TabIndex = 5;
            // 
            // headerColumn
            // 
            this.headerColumn.HeaderText = "Название";
            this.headerColumn.MaxInputLength = 50;
            this.headerColumn.Name = "headerColumn";
            this.headerColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.headerColumn.Width = 200;
            // 
            // dataColumn
            // 
            this.dataColumn.HeaderText = "Ключевое слово";
            this.dataColumn.MaxInputLength = 50;
            this.dataColumn.Name = "dataColumn";
            this.dataColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataColumn.Width = 210;
            // 
            // vlcPath
            // 
            this.vlcPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.vlcPath.Location = new System.Drawing.Point(14, 81);
            this.vlcPath.MaxLength = 1000;
            this.vlcPath.Name = "vlcPath";
            this.vlcPath.Size = new System.Drawing.Size(427, 23);
            this.vlcPath.TabIndex = 6;
            // 
            // vlcPathHeader
            // 
            this.vlcPathHeader.AutoSize = true;
            this.vlcPathHeader.Location = new System.Drawing.Point(14, 63);
            this.vlcPathHeader.Name = "vlcPathHeader";
            this.vlcPathHeader.Size = new System.Drawing.Size(111, 15);
            this.vlcPathHeader.TabIndex = 7;
            this.vlcPathHeader.Text = "Путь до VLC player:";
            // 
            // settingsButton
            // 
            this.settingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsButton.Image = global::PlaylistEditor.Properties.Resources.OpenFolder;
            this.settingsButton.Location = new System.Drawing.Point(447, 81);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(24, 24);
            this.settingsButton.TabIndex = 23;
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "exe";
            this.openFileDialog1.FileName = "vlc.exe";
            this.openFileDialog1.Filter = "VLC exe|*.exe";
            this.openFileDialog1.Title = "VLC Player Path";
            // 
            // scheduleHeader
            // 
            this.scheduleHeader.AutoSize = true;
            this.scheduleHeader.Location = new System.Drawing.Point(10, 176);
            this.scheduleHeader.Name = "scheduleHeader";
            this.scheduleHeader.Size = new System.Drawing.Size(181, 15);
            this.scheduleHeader.TabIndex = 25;
            this.scheduleHeader.Text = "Источник программы передач:";
            // 
            // scheduleText
            // 
            this.scheduleText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.scheduleText.Location = new System.Drawing.Point(10, 194);
            this.scheduleText.MaxLength = 1000;
            this.scheduleText.Name = "scheduleText";
            this.scheduleText.Size = new System.Drawing.Size(427, 23);
            this.scheduleText.TabIndex = 24;
            // 
            // scheduleCheckBox
            // 
            this.scheduleCheckBox.AutoSize = true;
            this.scheduleCheckBox.Location = new System.Drawing.Point(10, 131);
            this.scheduleCheckBox.Name = "scheduleCheckBox";
            this.scheduleCheckBox.Size = new System.Drawing.Size(197, 19);
            this.scheduleCheckBox.TabIndex = 26;
            this.scheduleCheckBox.Text = "Загружать программу передач";
            this.scheduleCheckBox.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 568);
            this.Controls.Add(this.scheduleCheckBox);
            this.Controls.Add(this.scheduleHeader);
            this.Controls.Add(this.scheduleText);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.vlcPathHeader);
            this.Controls.Add(this.vlcPath);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.customParamsHeader);
            this.Controls.Add(this.timeoutLabel);
            this.Controls.Add(this.timeoutTextBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.Text = "Настройки";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox timeoutTextBox;
        private Label timeoutLabel;
        private Label customParamsHeader;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn headerColumn;
        private DataGridViewTextBoxColumn dataColumn;
        private TextBox vlcPath;
        private Label vlcPathHeader;
        private Button settingsButton;
        private OpenFileDialog openFileDialog1;
        private Label scheduleHeader;
        private TextBox scheduleText;
        private CheckBox scheduleCheckBox;
    }
}
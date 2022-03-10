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
            this.customParamsHeader.Location = new System.Drawing.Point(10, 62);
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
            this.dataGridView1.Location = new System.Drawing.Point(22, 83);
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
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 384);
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
    }
}
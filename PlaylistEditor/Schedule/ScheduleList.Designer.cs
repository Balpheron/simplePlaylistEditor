namespace PlaylistEditor
{
    partial class ScheduleList
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
            this.schudelListBox = new System.Windows.Forms.ListBox();
            this.infoBox = new System.Windows.Forms.TextBox();
            this.infoHeader = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // schudelListBox
            // 
            this.schudelListBox.FormattingEnabled = true;
            this.schudelListBox.ItemHeight = 15;
            this.schudelListBox.Location = new System.Drawing.Point(12, 12);
            this.schudelListBox.Name = "schudelListBox";
            this.schudelListBox.Size = new System.Drawing.Size(358, 424);
            this.schudelListBox.TabIndex = 0;
            this.schudelListBox.SelectedIndexChanged += new System.EventHandler(this.schudelListBox_SelectedIndexChanged);
            // 
            // infoBox
            // 
            this.infoBox.Location = new System.Drawing.Point(388, 30);
            this.infoBox.Multiline = true;
            this.infoBox.Name = "infoBox";
            this.infoBox.ReadOnly = true;
            this.infoBox.Size = new System.Drawing.Size(281, 406);
            this.infoBox.TabIndex = 1;
            // 
            // infoHeader
            // 
            this.infoHeader.Location = new System.Drawing.Point(388, 12);
            this.infoHeader.Name = "infoHeader";
            this.infoHeader.Size = new System.Drawing.Size(281, 15);
            this.infoHeader.TabIndex = 8;
            this.infoHeader.Text = "Информация";
            this.infoHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ScheduleList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 450);
            this.Controls.Add(this.infoHeader);
            this.Controls.Add(this.infoBox);
            this.Controls.Add(this.schudelListBox);
            this.Name = "ScheduleList";
            this.Text = "Программа передач";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScheduleList_FormClosing);
            this.Load += new System.EventHandler(this.ScheduleList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox schudelListBox;
        private TextBox infoBox;
        private Label infoHeader;
    }
}
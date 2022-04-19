using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaylistEditor
{
    public partial class SettingsForm : Form
    {
        private Form1 mainForm;
        

        public SettingsForm(Form1 mainForm)
        {
            InitializeComponent();
            // делаем неактивной основную форму
            this.mainForm = mainForm;
            mainForm.Enabled = false;
            
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // считываем конфигурацию и визуализируем ее в интрефейсных элементах
            Configurator.ReadConfig();
            Config? config = Configurator.currentConfig;

            if (config == null)
                return;

            timeoutTextBox.Text = config.CheckTimeoutTime.ToString();
            vlcPath.Text = config.VLCPath;
            scheduleText.Text = config.schedulePath;
            scheduleCheckBox.Checked = config.loadSchedule;
            // добавляем строки в таблицу для каждого параметра в словаре
            foreach (var item in config.customValues)
            {
                string[] rowData = { item.Key, item.Value };
                dataGridView1.Rows.Add(rowData);
            }
        }


        private async void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // сохраняем конфигурацию
            Configurator.currentConfig = ComposeConfig();
            Configurator.WriteConfig();
            // активируем основную форму и создаем новые строки, если необходмо
            mainForm.Enabled = true;
            mainForm.CustomValuesUI();
            // проверяем архив с программой передач
            await mainForm.LoadScheduleArchive();
        }

        // собираем конфигурацию из значений в строках интерфейса
        private Config ComposeConfig()
        {
            // создаем объект конфигурации на основе данных из интерфейса
            Config config = new Config();

            int timeout;
            int.TryParse(timeoutTextBox.Text, out timeout);
            config.CheckTimeoutTime = timeout;
            config.VLCPath = vlcPath.Text;
            config.schedulePath = scheduleText.Text;
            config.customValues = new Dictionary<string, string>();
            config.loadSchedule = scheduleCheckBox.Checked;

            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.IsNewRow)
                    continue;

                if (item.Cells[1].Value == null || item.Cells[1].Value == null)
                    continue;

                try
                {
                    config.customValues.Add(item.Cells[0].Value.ToString(), item.Cells[1].Value.ToString());
                }
                catch (Exception)
                {

                }

            }
            return config;
        }

        // выбираем путь для VLC player
        private void settingsButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string fileName = openFileDialog1.FileName;
            vlcPath.Text = fileName;
        }
    }
}

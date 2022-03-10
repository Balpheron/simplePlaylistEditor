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

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            Configurator.currentConfig = ComposeConfig();

            Configurator.WriteConfig();
            mainForm.Enabled = true;
            mainForm.CustomValuesUI();
        }

        private Config ComposeConfig()
        {
            // создаем объект конфигурации на основе данных из интерфейса
            Config config = new Config();

            int timeout;
            int.TryParse(timeoutTextBox.Text, out timeout);
            config.CheckTimeoutTime = timeout;
            config.customValues = new Dictionary<string, string>();

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

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // считываем конфигурацию и визуализируем ее в интрефейсных элементах
            Configurator.ReadConfig();
            timeoutTextBox.Text = Configurator.currentConfig.CheckTimeoutTime.ToString();
            foreach (var item in Configurator.currentConfig.customValues)
            {
                string[] rowData = {item.Key, item.Value};
                dataGridView1.Rows.Add(rowData);
            }
        }
    }
}

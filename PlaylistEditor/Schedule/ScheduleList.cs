using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PlaylistEditor.Schedule;

namespace PlaylistEditor
{
    public partial class ScheduleList : Form
    {
        private int startIndex = 0;

        public struct ScheduleViewItem
        {
            public int id { get; set; }
            public string label{ get; set; }
            
            public ScheduleViewItem(int id, string label)
            {
                this.id = id;
                this.label = label;
            }
        }

        protected Form1 mainForm;
        protected ScheduleManager manager;
        protected List<ScheduleViewItem> listItems; //список из элементов с двумя параметрами - id и строка, состоящая
                                                          // из время_начала-время_конца-название  
                                                          
        
        public ScheduleList(Form1 mainForm, ScheduleManager scheduleManager)
        {
            InitializeComponent();
            // делаем неактивной основную форму
            this.mainForm = mainForm;
            mainForm.Enabled = false;
            manager = scheduleManager;
            listItems = new List<ScheduleViewItem>();
        }

        protected void ScheduleList_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.Enabled = true;
        }

        protected virtual void ComposeSchedule()
        {
            int index = 0;
            int prevHours = -1;
            string label;
            startIndex = 0;
            try
            {
                foreach (var item in manager)
                {

                    // если это первый предмет или величина часов даты начала предыдущего объекта больше текущего,
                    // то добавляем отметку о дате с отрицательным id
                    DateTime startTime = DateTime.Now; DateTime.TryParseExact(item.StartTime, Syntax.dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out startTime);
                    DateTime endTime = DateTime.Now; DateTime.TryParseExact(item.EndTime, Syntax.dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out endTime);

                    // и получаем индекс идущего в эфире объекта
                    if (startIndex == 0 && DateTime.Now < endTime && DateTime.Now > startTime)
                        startIndex = listItems.Count;

                    if (index == 0 || startTime.Hour < prevHours)
                    {
                        label = $"\t\t{startTime.ToString("M")}";
                        listItems.Add(new ScheduleViewItem((index - 1) * -1, label));
                    }
                    // компонуем объект: время_начала-время_конца название 
                    label = $"{startTime.ToString("t")} - {endTime.ToString("t")} {item.Name}";
                    listItems.Add(new ScheduleViewItem(index, label));
                    // увеличиваем индекс и запоминаем время
                    index++;
                    prevHours = startTime.Hour;
                }
            }
            catch
            {

            }
        }

        protected virtual void FillList()
        {
            if (listItems.Count < 1)
                return;

            schudelListBox.DataSource = listItems;
            schudelListBox.DisplayMember = "label";
            // переключаемся на эфир
            schudelListBox.SelectedIndex = startIndex;
        }

        private void ScheduleList_Load(object sender, EventArgs e)
        {
            ComposeSchedule();
            FillList();
        }

        private void schudelListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string content = ShowItemInfo(schudelListBox.SelectedIndex);
            infoBox.Text = content;
        }

        // компонуем описании элемента программы передач из данных
        protected virtual string ShowItemInfo(int index)
        {
            // отправляем запрос не по фактическому индексу из listbox, а по id из нашего списка
           ScheduleManager.ScheduleItem? item = manager.GetScheduleItemData(listItems[index].id);

            if (item == null)
                return "Нет информации";

            string content = "";
            content += $"Название: {item?.Name}\r\n";
            content += $"Жанр: {item?.Category}\r\n";
            content += $"Описание: {item?.Description}\r\n";
            return content;
        }
    }
}

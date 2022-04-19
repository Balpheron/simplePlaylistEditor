using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistEditor
{
    public class Config
    {
        private int checkTimeoutTime = 5;
        public string? VLCPath;
        public string? schedulePath;
        public DateTime lastCheckDate;
        public bool loadSchedule;
        public int CheckTimeoutTime { get { return checkTimeoutTime; } set { if (value <= 1) checkTimeoutTime = 1; else checkTimeoutTime = value; } }  
        public Dictionary<string, string> customValues;
        
        // конфигурация по умолчанию
        public Config()
        {
            CheckTimeoutTime = 5;
            VLCPath = null;
            schedulePath = "https://iptvx.site/epg.xml.gz";
            lastCheckDate = DateTime.Now;
            loadSchedule = false;
            customValues = new Dictionary<string, string>() { { "ID для EPG", "tvg-id"},
            { "Длительность архива", "tvg-rec"},
            };
        }
    }
}

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
        internal int CheckTimeoutTime { get { return checkTimeoutTime; } set { if (value <= 1) checkTimeoutTime = 1; else checkTimeoutTime = value; } }  
        internal Dictionary<string, string> customValues; 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistEditor
{
    internal static class Icons
    {
        /* картинки для узлов: 0 - база для каналов; 1 - идет процесс проверки; 2 - доступен; 3 - условно доступен; 4 - недоступен
            5 - плейлист, 6 - группа    
        */
        internal static ImageList GetIconsList()
        {
            ImageList imageList = new ImageList();
            Image img;
            img = Properties.Resources.DesktopGray; // 0
            imageList.Images.Add(img);
            img = Properties.Resources.QuestionMark; // 1
            imageList.Images.Add(img);
            img = Properties.Resources.DesktopGreen; // 2
            imageList.Images.Add(img);
            img = Properties.Resources.DesktopBlue; // 3
            imageList.Images.Add(img);
            img = Properties.Resources.DesktopRed; // 4
            imageList.Images.Add(img);
            img = Properties.Resources.ListView; // 5
            imageList.Images.Add(img);
            img = Properties.Resources.FolderOpened; // 6
            imageList.Images.Add(img);
            return imageList;
        }

    }
}

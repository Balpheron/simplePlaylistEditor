using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistEditor
{
    public class Channel : IEquatable<Channel>
    {
        
        public static int channelsCount = 0;
        string name = "DefaultChannel";
        public string Name { get { return name; } set { name = value; } }
        string logoPath = "";
        public string LogoPath { get { return logoPath; } } //путь к логотипу; в случае, если он пустой, используется какие-то стандартное изображение
        string channelPath = "";
        public string ChannelPath { get { return channelPath; } } //путь к контенту канала - в общем случае, ссылка на поток
        string groupName = Syntax.defaultGroupName;
        public string GroupName { get { return groupName; } set { if (value == "") groupName = Syntax.defaultGroupName; else groupName = value; } }  //имя группы каналов или папки в плейлисте 
        int channelDuration = 0; // длительность трека; если стоит значение -1 или 0, трек является прямой трансляцией
        public string ChannelDuration { get { if (channelDuration == -1 || channelDuration == 0) return "Трансляция"; return channelDuration.ToString(); } set { if(!int.TryParse(value, out channelDuration)) channelDuration = -1; } }
        
        public string[] customData = new string[1];
        public string additionalData = "";

        public delegate string ChannelInfoComposer(); // делегат для компоновки возвращаемой строки из данных о канале

        int availChecked; // проверен ли канал на доступность; 0 - не проверен, 1 - доступен, -1 - не доступен
        public int Checked { get { return availChecked; } set { if (value == 0) availChecked = -1; else availChecked = 1; } }

        public Channel(string channelPath, string name, string logoPath, string groupName, string trackDuration)
        {
            this.channelPath = channelPath;
            this.logoPath = logoPath;
            this.name = name;
            ChannelDuration = trackDuration;
            GroupName = groupName;
            channelsCount++;
            availChecked = 0;
        }

        public Channel()
        {
            //создаем стандартное имя с порядковым номером
            this.name = $"Channel{channelsCount}";
            channelsCount++;
            availChecked = 0;
        }

        public bool Equals(Channel? other)
        {
            if (other == null) return false;
            return (other.Name == this.Name);
        }

        public string GetChannelStringInfo(ChannelInfoComposer? Composer)
        {
            if (Composer == null)
                Composer += DefaultInfoComposer;
            return Composer?.Invoke();
        }

        string DefaultInfoComposer()
        {
            // заголовок и длительность трека/трансляция
            string channelInfo = $@"{Syntax.channelHeader}{channelDuration} ";
            // логотип, если он есть
            if (logoPath != "")
            channelInfo += $@"{Syntax.logoTag}""{LogoPath}""";
            // категория
            channelInfo += $@"{Syntax.groupTag}""{GroupName}""";
            // дополнительные параметры, идущие в одной строке до названия канала, если они не пустые
            int ndx = 0;
            foreach (var item in Configurator.currentConfig.customValues)
            {
                if (customData[ndx].Length != null && customData[ndx] != "")
                channelInfo += $@"{item.Value}=""{customData[ndx]}""";
                ndx++;
            }
            // название канала
            channelInfo += $",{Name}\n";
            // дополнительные строки
            if (additionalData != "")
                channelInfo += $"{additionalData}\n";
            // на следующей строке располагается ссылка на контент
            channelInfo += $"{ChannelPath}\n";
            return channelInfo;
        }

        public Channel Clone()
        {
            Channel channel = new Channel();
            channel.Name = Name;
            channel.channelDuration = channelDuration;
            channel.channelPath = channelPath;
            channel.logoPath = logoPath;
            channel.channelDuration = channelDuration;
            channel.groupName = groupName;
            channel.additionalData = additionalData;
            channel.customData = customData;
            return channel;
        }
    }
}

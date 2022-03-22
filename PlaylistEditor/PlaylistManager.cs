using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PlaylistEditor
{
    public class PlaylistManager : INodeDataCollection
    {
        public List<Playlist> playlists = new List<Playlist>();
        int currentPlaylistIndex = 0;
        public int CurrentPlaylistIndex { get { return currentPlaylistIndex; } set { currentPlaylistIndex = value; } }
        public Playlist CurrentPlaylist { get { return playlists[currentPlaylistIndex]; } } 

        public delegate void ErrorMessage(string message);
        internal event ErrorMessage? OnError;
        private static PlaylistManager instance;

        public static PlaylistManager GetInstance()
        {
            if (instance == null)
                instance = new PlaylistManager();
            return instance;
        }

        Form1 mainForm;

        public void AddListener(ErrorMessage messager)
        {
            OnError += messager;
        }

        public PlaylistManager()
        {
            mainForm = new Form1();
        }

        public PlaylistManager(Form1 mainForm)
        {
            this.mainForm = mainForm;
            instance = this;
        }

        public Playlist GeneratePlaylist(ref string rawContent, string filePath = "")
        {
            string playlistName = "Default Playlist";
            if (filePath != "")
            playlistName = System.IO.Path.GetFileNameWithoutExtension(filePath);

            Playlist playlist = new Playlist(playlistName); //создаем пустой плейлист с именем, совпадающим с именем файла
            Channel.channelsCount = 0; //обнуляем счетчик каналов

            //ищем заголовок в строке и вырезаем его в отдельную переменную
            try
            {
                string regexPattern = Syntax.listHeader + @".*";
                Regex regex = new Regex(regexPattern);
                rawContent = regex.Replace(rawContent, m => { playlist.Header = m.Value; return ""; }, 1 );
            }
            catch (Exception)
            {
                OnError?.Invoke("No m3u header exists");
                playlist.Header = Syntax.listHeader;
            }
            // разбиваем файл на строки, соответствующие одному каналу
            
            string[] channelsInfoString = rawContent.Split(Syntax.channelHeader, StringSplitOptions.RemoveEmptyEntries);

            // создаем массив нестандартных параметров
            string[] customTags = new string[Configurator.currentConfig.customValues.Count];
            int ndx = 0;
            foreach (var item in Configurator.currentConfig.customValues)
            {
                customTags[ndx++] = item.Value;
            }

            // перебираем полученный массив
            Channel tempChannel;
            for (int i = 0; i < channelsInfoString.Length; i++)
            {
                if (channelsInfoString[i].Length < 4)
                    continue;

                tempChannel = GenerateChannel(channelsInfoString[i], customTags);
                int groupIndex = playlist.FindGroup(tempChannel.GroupName, true); //ищем нужную категорию
                playlist.AddChannel(tempChannel, groupIndex); //добавляем канал в эту категорию

            }

            return playlist;
        }

        public void GeneratePlaylistTree()
        {
            mainForm.Invoke(mainForm.playlistGenerator);
        }

        public Channel GenerateChannel(string rawData, params string[] customTags)
        {
            //удаляем пустые строки
            rawData = Regex.Replace(rawData, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
            // находим параметры, для которых выделяются отдельные строки
            // в первую очередь, пренадлежность канала к какой-либо группе
            string groupName = "";
            string currentPattern = Syntax.altGroupTag + @".*";
            Regex regex = new Regex(currentPattern);
            try
            {
                rawData = regex.Replace(rawData, m => { groupName = m.Value; return ""; }, 1);
                groupName = groupName.Replace(Syntax.altGroupTag, "");
            }
            catch (Exception e)
            {
                groupName = "";
                OnError?.Invoke(e.Message);
            }
            // оставшиеся параметры

            //разбиваем полученную строку на подстроки; при этом последняя подстрока здесь явлется путем к треку или ссылкой на поток
            string[] subStrings = rawData.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            //разбиваем заглавную строку на подстроки по символу ",", чтобы вычленить названия канала или трека
            //при этом заголовком всегда будет последняя подстрока
            string[] headerSubStrings = subStrings[0].Split(',');
            string channelName = headerSubStrings[^1];

            // если для одного канал строк больше, чем 2 (не считая отдельную строку для группы), записываем эти строки
            string additionalString = "";
            if (subStrings.Length > 2)
            {
                for (int i = 1; i < subStrings.Length - 1; i++)
                {
                    additionalString += subStrings[i];
                }
            }
            //получаем информацию из оставшейся подстроки заглавной строки о:
            //длительность трека - первые несколько цифр с возможным знаком '-' перед значением
            currentPattern = @".\d*";
            regex = new Regex(currentPattern);
            string trackDuration = "-1";
            try
            {
                trackDuration = regex.Match(headerSubStrings[0]).Value;
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
            }
            //логотип трека или канала; находится между кавычками после определенного тега
            string logoPath = "";
            try
            {
                regex = new Regex(Syntax.logoTag + "\"[^\"]+\"");
                logoPath = Syntax.GetStringBetween(regex.Match(headerSubStrings[0]).Value, Syntax.logoTag);
            }
            catch (Exception e)
            {
                //OnError?.Invoke(e.Message);
            }
            //категорию канала, если она не была определена до этого момента; аналогично логотипу
            if (groupName == "")
            {
                try
                {
                    regex = new Regex(Syntax.groupTag + "\"[^\"]+\"");
                    groupName = Syntax.GetStringBetween(regex.Match(headerSubStrings[0]).Value, Syntax.groupTag);
                }
                catch (Exception e)
                {
                    //OnError?.Invoke(e.Message);
                }
            }
            //создаем канал с полученными данными
            Channel result = new Channel(subStrings[^1], channelName, logoPath, groupName, trackDuration);

            //схожим образом считываем дополнительные данные, если необходимо
            if (customTags.Length > 0)
            {
                result.customData = new string[customTags.Length];

                for (int i = 0; i < customTags.Length; i++)
                {
                    regex = new Regex(customTags[i] + "=" + "\"[^\"]+\"");
                    logoPath = Syntax.GetStringBetween(regex.Match(headerSubStrings[0]).Value, customTags[i] + "=");
                    result.customData[i] = logoPath;
                }
            }

            
            result.additionalData = additionalString;
            
            return result;
        }

        public string SavePlaylistAsFile(int playlistIndex)
        {
            string result = playlists[playlistIndex].PlaylistToFile();
            return result;
        }

        // получаем объект по заданным координатам в древе
        public INodeData? GetObject(int[] coordinates)
        {
            switch (coordinates.Length)
            {
                case 1: return playlists[coordinates[0]];
                case 2: return playlists[coordinates[0]].groupsList[coordinates[1]];
                case 3: return playlists[coordinates[0]].groupsList[coordinates[1]].channelsList[coordinates[2]];
                default: return null;
            }            
        }

        // получаем родительский объект по заданным координатам в древе
        public INodeDataCollection? GetParent(int[] coordinates)
        {
            try
            {
                switch (coordinates.Length)
                {
                    case 2: return playlists[coordinates[0]];
                    case 3: return playlists[coordinates[0]].groupsList[coordinates[1]];
                    default: return this;
                }
            }
            catch (Exception)
            {
                return this;
            }
            

        }

        public void RemoveChild(int index)
        {
            playlists.RemoveAt(index);
        }

        public void AddChild(INodeData element, int index = -1)
        {
            if (index == -1)
                playlists.Add((Playlist)element);
            else playlists.Insert(index, (Playlist)element);
        }

        public string AddChild()
        {
            Playlist newPlaylist = new Playlist("");
            playlists.Add(newPlaylist);
            return newPlaylist.Name;
        }

        // удаляем канал по определенным координатам

    }

    
}

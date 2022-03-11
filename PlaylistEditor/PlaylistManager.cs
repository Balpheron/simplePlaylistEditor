using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PlaylistEditor
{
    public class PlaylistManager
    {
        public List<Playlist> playlists = new List<Playlist>();
        int currentPlaylistIndex = 0;
        public int CurrentPlaylistIndex { get { return currentPlaylistIndex; } set { currentPlaylistIndex = value; } }
        public Playlist CurrentPlaylist { get { return playlists[currentPlaylistIndex]; } } 

        public delegate void ErrorMessage(string message);
        event ErrorMessage? OnError;
        
        Form1 mainForm;

        public void AddListener(ErrorMessage messager)
        {
            OnError += messager;
        }

        public PlaylistManager(Form1 mainForm)
        {
            this.mainForm = mainForm;
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
                playlist.groupsList[groupIndex].channelsList.Add(tempChannel); //добавляем канал в эту категорию

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

        // переносим группу из одного плейлиста в другой
        // (группа для переноса, индекс плейлиста-источника переноса, индекс плейлиста-цели переноса)
        public void MoveElement(int groupIndex, int oldIndex, int newIndex, bool removeOld)
        {
            Group tempGroup = playlists[oldIndex].groupsList[groupIndex].Clone();
            if (removeOld)
                playlists[oldIndex].groupsList.RemoveAt(groupIndex);
            playlists[newIndex].groupsList.Add(tempGroup);
        }

        // аналогично для канала, но нужно больше входных данных - об индексе плейлиста
        public void MoveElement(int channelIndex, (int, int) oldIndex, (int, int) newIndex, bool removeOld)
        {
            Channel tempChannel = playlists[oldIndex.Item1].groupsList[oldIndex.Item2].channelsList[channelIndex];
            if (removeOld)
                playlists[oldIndex.Item1].groupsList[oldIndex.Item2].channelsList.RemoveAt(channelIndex);
            playlists[newIndex.Item1].groupsList[newIndex.Item2].channelsList.Add(tempChannel);
        }

        // переносим группу из одного места внутри плейлиста в другое
        // (группа для переноса, индекс плейлиста-источника переноса, (индекс плейлиста-цели переноса, индекс группы-цели переноса))
        public void MoveElement(bool group, int[] oldCoords, int[] newCoords, bool removeOld)
        {
            Group tempGroup = playlists[oldCoords[0]].groupsList[oldCoords[1]].Clone();
            if (removeOld)
                playlists[oldCoords[0]].groupsList.RemoveAt(oldCoords[1]);
            playlists[newCoords[0]].groupsList.Insert(newCoords[1], tempGroup);
        }

        // аналогично для канала, но нужно больше входных данных - об индексе плейлиста
        public void MoveElement(int[] oldCoords, int[] newCoords, bool removeOld)
        {
            Channel tempChannel = playlists[oldCoords[0]].groupsList[oldCoords[1]].channelsList[oldCoords[2]];
            if (removeOld)
                playlists[oldCoords[0]].groupsList[oldCoords[1]].channelsList.RemoveAt(oldCoords[2]);
            playlists[newCoords[0]].groupsList[newCoords[1]].channelsList.Insert(newCoords[2], tempChannel);
        }

        // удаляем канал по определенным координатам
        public void DeleteElement(params int[] coordinates)
        {
            switch (coordinates.Length)
            {
                case 1: playlists.RemoveAt(coordinates[0]); break;
                case 2: playlists[coordinates[0]].groupsList.RemoveAt(coordinates[1]); break;
                case 3: playlists[coordinates[0]].groupsList[coordinates[1]].channelsList.RemoveAt(coordinates[2]); break;
                default: return;
                    
            }
        }

        public string AddGroup(int playlistIndex)
        {
            try
            {
                playlists[playlistIndex].groupsList.Add(new Group());
            }
            catch (Exception)
            {
                throw new Exception("Cannot add new group");
            }

            return playlists[playlistIndex].groupsList[^1].Name;
            
        }

        public string AddChannel((int, int) coordinates)
        {
            try
            {
                playlists[coordinates.Item1].groupsList[coordinates.Item2].channelsList.Add(new Channel());
            }
            catch (Exception)
            {   
                throw new Exception("Cannot add new group");
            }

            return playlists[coordinates.Item1].groupsList[coordinates.Item2].channelsList[^1].Name;
        }

        public string SavePlaylistAsFile(int playlistIndex, string filePath)
        {
            string result = "";
            // добавляем в самое начало файла заголовок плейлиста
            result += $"{playlists[playlistIndex].Header}\n";
            // перебираем по порядку канала в каждой группе указанного плейлиста
            for (int i = 0; i < playlists[playlistIndex].groupsList.Count; i++)
            {
                for (int k = 0; k < playlists[playlistIndex].groupsList[i].channelsList.Count; k++)
                {
                    try
                    {
                        result += playlists[playlistIndex].groupsList[i].channelsList[k].GetChannelStringInfo(null);
                    }
                    catch (Exception e)
                    {
                        mainForm.ShowErrorWindow(e.Message);
                    }
                    
                }
            }

            return result;
        }

        // получаем объект по заданным координатам в древе
        public object? GetObject(int[] coordinates)
        {
            switch (coordinates.Length)
            {
                case 0: return playlists[coordinates[0]];
                case 1: return playlists[coordinates[0]].groupsList[coordinates[1]];
                case 2: return playlists[coordinates[0]].groupsList[coordinates[1]].channelsList[coordinates[2]];
                default: return null;
            }
        }
    }

    
}

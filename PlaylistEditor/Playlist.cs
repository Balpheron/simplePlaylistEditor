using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistEditor
{
    public class Playlist
    {
        string name = "DefaultPlaylist";
        public string Name { get { return name; } set { if (value == "") name = "DefaultPlaylist"; else name = value; } }
        
        public List<Group> groupsList;

        string header = "";
        public string Header { get { return header; } set { header = value; } }

        int currentGroupIndex;
        public int CurrentGroupIndex { set { currentGroupIndex = value; } }

        int currentChannelIndex;
        public int CurrentChannelIndex { set { currentChannelIndex = value; } }   
        public Channel CurrentChannel { get {  return groupsList[currentGroupIndex].channelsList[currentChannelIndex]; } } // возвращает выбранный канал

        public Playlist(string name)
        {
            Name = name;
            groupsList = new List<Group>();
        }

        //возвращает индекс группы с искомым именем
        //если createIfMissing == true, добавляет группу с необходимым именем
        //иначе возвращает индекс группы Syntax.defaultGroupName
        public int FindGroup(string groupName, bool createIfMissing)
        {
            for (int i = 0; i < groupsList.Count; i++)
            {
                if (groupsList[i].Name.Equals(groupName, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            //если нужно, то добавляем группу и возвращаем ее индекс
            if (createIfMissing)
            {
                groupsList.Add(new Group(groupName));
                return groupsList.Count - 1;
            }
            //или рекурсивно ищем и создаем при необходимости стандартную группу
            else return FindGroup(Syntax.defaultGroupName, true);
        }

        //проверяем правильность данных, полученных из строк для редактирования каналов
        //меняем локацию в группе, если нужно, и возвращаем номер новой группы
        //если перемещение между группами не нужно, возвращаем -1
        public int SaveChannel(Channel newChannel)
        {
            int result = -1;
            // проверяем имя на число символов
            if (newChannel.Name.Length < 1)
                throw new Exception("Имя канала должно содержать хотя бы один символ");
            // проверка длины строки с содержимым
            if (newChannel.ChannelPath.Length < 1)
                throw new Exception("Ссылка на содержимое должна содержать хотя бы один символ");
            // определеямся, что делать с измененной категорией перед созданием объекта канала, чтобы возможно было отследить изменение категории:
            if (CurrentChannel.GroupName != newChannel.GroupName)
            {
                // находим индекс нужной группы или создаем новую
                result = FindGroup(newChannel.GroupName, true);
            }
            //переписываем объект канала в коллекции
            EditChannel(newChannel);
            // перемещяем объект канала в другой лист, если нужно
            if (result != -1)
                MoveChannel((result, -1));
            return result;

        }

        //заменяет текущий канал на заданный
        public void EditChannel(Channel newChannel)
        {
            groupsList[currentGroupIndex].channelsList[currentChannelIndex] = newChannel;
        }

        public void MoveChannel((int group, int channel) startingPosition, (int group, int channel) endingPosition)
        {
            Channel tempChannel = groupsList[startingPosition.group].channelsList[startingPosition.channel];
            groupsList[startingPosition.group].channelsList.RemoveAt(startingPosition.channel);
            groupsList[endingPosition.group].channelsList.Add(tempChannel);
        }

        public void MoveChannel((int group, int channel) endingPosition)
        {
            Channel tempChannel = CurrentChannel;
            groupsList[currentGroupIndex].channelsList.RemoveAt(currentChannelIndex);
            groupsList[endingPosition.group].channelsList.Add(tempChannel);
        }

    }
}

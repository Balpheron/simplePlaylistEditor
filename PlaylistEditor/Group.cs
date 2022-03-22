using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistEditor
{
    public class Group : IEquatable<Group>, INodeData, INodeDataCollection, IEnumerable
    {
        
        string name = Syntax.defaultGroupName;
        public static int groupsCounter;
        public string Name { get { return name; } set { if (value == "") name = Syntax.defaultGroupName; else name = value; } }
        int currentChannelIndex = 0;
        public int CurrentChannelIndex { set { currentChannelIndex = value; } }
        public Channel CurrentChannel { get { return channelsList[currentChannelIndex]; } }

        public object Current => throw new NotImplementedException();

        internal List<Channel> channelsList;
        public Group(string name = "")
        {
            Name = name;
            channelsList = new List<Channel>(); //создаем пустой список каналов для последующего заполнения
            groupsCounter++;
        }
        // создание группы с заданным именем и клонированием списка каналов
        public Group(string name, List<Channel> channelsList)
        {
            Name = name;
            this.channelsList = new List<Channel>();
            for (int i = 0; i < channelsList.Count; i++)
            {
                if (channelsList[i].Clone() is Channel chnl)
                this.channelsList.Add(chnl);
            }
        }

        public bool Equals(Group? other)
        {
            if (other == null) return false;
            return (other.Name == this.Name);
        }

        // клонирование группы
        public INodeData Clone()
        {
            INodeData group = new Group(Name, channelsList);
            return group;
            
        }

        public void RemoveChild(int index)
        {
            channelsList.RemoveAt(index);
        }

        public void AddChild(INodeData element, int index = -1)
        {
            Channel channel = (Channel)element;
            channel.GroupName = Name;
            if (index == -1)
                channelsList.Add(channel);
            else channelsList.Insert(index, channel);
        }

        public string AddChild()
        {
            Channel channel = new Channel();
            channelsList.Add(channel);
            return channel.Name;
        }

        public void Rename(string newName)
        {
            Name = newName;
            foreach (var item in channelsList)
            {
                item.GroupName = newName;
            }
                         
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return new GroupEnum(channelsList);
        }
               
    }

    public class GroupEnum : IEnumerator
    {
        private List<Channel> list;
        int position = -1;

        public GroupEnum(List<Channel> list)
        {
            this.list = list;
        }

        public object Current { get { return new int[] {position}; } }

        public bool MoveNext()
        {
            position++;
            return (position < list.Count);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
    }

}

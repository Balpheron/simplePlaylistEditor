using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistEditor
{
    public class Group : IEquatable<Group>
    {
        
        string name = Syntax.defaultGroupName;
        public static int groupsCounter;
        public string Name { get { return name; } set { if (value == "") name = Syntax.defaultGroupName + groupsCounter.ToString(); else name = value; } }
        int currentChannelIndex = 0;
        public int CurrentChannelIndex { set { currentChannelIndex = value; } }
        public Channel CurrentChannel { get { return channelsList[currentChannelIndex]; } }

        public List<Channel> channelsList;
        public Group(string name = "")
        {
            Name = name;
            channelsList = new List<Channel>(); //создаем пустой список каналов для последующего заполнения
            groupsCounter++;
        }

        public bool Equals(Group? other)
        {
            if (other == null) return false;
            return (other.Name == this.Name);
        }

        public Group Clone()
        {
            Group group = new Group();
            group.Name = Name;
            group.channelsList = new List<Channel>();
            for (int i = 0; i < channelsList.Count; i++)
            {
                group.channelsList.Add(channelsList[i].Clone());
            }
            return group;
        }
    }
}

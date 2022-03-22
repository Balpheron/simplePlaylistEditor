using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistEditor
{
    public interface INodeData
    {

        public INodeData Clone();
        public void Rename(string newName);
    }
}

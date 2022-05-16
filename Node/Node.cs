using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectWPF.Node
{
    class Node
    {
        public enum NodeType { Click };

        public float x, y;
        public NodeType nodeType;
    }
}

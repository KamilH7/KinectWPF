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

        public Node(float x, float y, NodeType nodeType)
        {
            this.x = x;
            this.y = y;

            this.nodeType = nodeType;
        }

        public Node()
        {

        }
    }
}

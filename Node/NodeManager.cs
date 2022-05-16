using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectWPF.Node
{
    class NodeManager
    {
        private float width, height;
        private List<Node> nodes;

        public void AddNode(Node newNode)
        {
            if (!nodes.Exists((node) => { return node == newNode; }))
            {
                nodes.Add(newNode);
            }
        }

        public void RemoveNode(Node removedNode)
        {
            nodes.Remove(removedNode);
        }

        public void GenerateNodes(int nodeCount, Node.NodeType nodeType)
        {
            Random rnd = new Random();

            for (int i = 0; i < nodeCount)
            {
                Node newNode = new Node((float)rnd.NextDouble() * width, (float)rnd.NextDouble() * height, nodeType);
                AddNode(newNode);
            }
        }
    }
}

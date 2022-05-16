using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace KinectWPF.Node
{
    class NodeManager
    {
        private float width, height;
        private List<Node> nodes;

        public IReadOnlyList<Node> Nodes { get => nodes; }

        public NodeManager(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

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

        public void GenerateNodes(int nodeCount, float radius, Node.NodeType nodeType)
        {
            Random rnd = new Random();

            for (int i = 0; i < nodeCount; ++i)
            {
                Node newNode = new Node((float)rnd.NextDouble() * width, (float)rnd.NextDouble() * height, radius, nodeType);
                AddNode(newNode);
            }
        }

        public Node CircleIntersection(float x, float y, float radius)
        {
            Node intersectingNode = nodes.Find((node) => { return Math.Pow(x - node.x, 2) + Math.Pow(y - node.y, 2) < Math.Pow(radius + node.radius, 2); });

            return intersectingNode;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using static KinectWPF.Node.Node;

namespace KinectWPF.Node
{
    class NodeManager
    {
        private MainWindow mainWindow;
        private const float spawnTimer = 2f;

        private Dictionary<NodeType, string> nodeImageNameByType = new Dictionary<NodeType, string>()
        {
            {NodeType.Click,"Banana"},
        };

        private float currentSpawnTimer;
        private float width, height;

        private List<Node> nodes = new List<Node>();

        public IReadOnlyList<Node> Nodes { get => nodes; }

        public NodeManager(float width, float height, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            this.width = width;
            this.height = height;

            mainWindow.OnUpdate += Update;
        }

        private void Update()
        {
            currentSpawnTimer += MainWindow.DeltaTime;

            if(currentSpawnTimer >= spawnTimer)
            {
                GenerateNodes(1,Node.NodeType.Click);

                currentSpawnTimer = 0;

                Trace.WriteLine(MainWindow.DeltaTime);
            }
        }

        private void AddNode(Node newNode)
        {
            if (!nodes.Exists((node) => { return node == newNode; }))
            {
                nodes.Add(newNode);
            }
        }

        private void RemoveNode(Node removedNode)
        {
            nodes.Remove(removedNode);
        }

        private void GenerateNodes(int nodeCount, Node.NodeType nodeType)
        {
            Random rnd = new Random();

            for (int i = 0; i < nodeCount; ++i)
            {
                //Node newNode = new Node(nodeImageNameByType[NodeType.Click], (float) rnd.NextDouble() * width, (float) rnd.NextDouble() * height, nodeType, mainWindow);
                AddNode(newNode);
            }
        }

        private Node CircleIntersection(float x, float y, float radius)
        {
            Node intersectingNode = nodes.Find((node) => { return Math.Pow(x - node.x, 2) + Math.Pow(y - node.y, 2) < Math.Pow(radius + node.radius, 2); });

            return intersectingNode;
        }
    }
}

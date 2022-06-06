using KinectWPF.Controllers.KinectController;
using KinectWPF.Enums;
using KinectWPF.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KinectWPF.Node
{
    class NodeSpawner
    {
        Dictionary<int, Uri> FruitImages = new Dictionary<int, Uri>()
            {
                {  (int)FruitTypeEnum.STRAWBERRY, new Uri(string.Concat(@"pack://application:,,,/",Assembly.GetExecutingAssembly().GetName().Name,@";component/",@"Resources/FruitImages/strawberry.png"), UriKind.Absolute) },
                {  (int)FruitTypeEnum.BANANA, new Uri(string.Concat(@"pack://application:,,,/",Assembly.GetExecutingAssembly().GetName().Name,@";component/",@"Resources/FruitImages/banana.png"), UriKind.Absolute) },
                {  (int)FruitTypeEnum.APPLE, new Uri(string.Concat(@"pack://application:,,,/",Assembly.GetExecutingAssembly().GetName().Name,@";component/",@"Resources/FruitImages/apple.png"), UriKind.Absolute) }
            };

        private IInputController inputController;
        private MainWindow mainWindow;
        private const float spawnTimer = 1f;
        private float currentSpawnTimer;
        private List<Node> nodes = new List<Node>();

        public NodeSpawner(MainWindow mainWindow, IInputController inputController)
        {
            this.mainWindow = mainWindow;
            this.inputController = inputController;

            mainWindow.OnGameStart += GameStart;
            mainWindow.OnGameOver += ClearNodes;
        }

        private void GameStart()
        {
            mainWindow.OnUpdate += Update;
        }

        private void Update()
        {
            HandleNodeSpawning();
            HandleNodeCollision();
        }

        private void HandleNodeCollision()
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                if (inputController.IsHoveringOver(nodes[i].GetPosition(),nodes[i].GetRadius()))
                {
                    nodes[i].Destroy();
                    RemoveNode(nodes[i]);
                }
            }
        }

        private void HandleNodeSpawning()
        {
            currentSpawnTimer += MainWindow.DeltaTime;

            if (currentSpawnTimer >= spawnTimer)
            {
                GenerateNodes(1);

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

        private void ClearNodes()
        {
            foreach(Node node in nodes)
            {
                node.Destroy();
            }

            nodes.Clear();
        }

        private void GenerateNodes(int nodeCount)
        {
            Random rnd = new Random();

            for (int i = 0; i < nodeCount; ++i)
            {
                Node newNode = new Node(mainWindow, GetRandomImage(), GetRandomPosition());
                AddNode(newNode);
            }
        }

        private Point GetRandomPosition()
        {
            Random rnd = new Random();

            Point point = new Point(rnd.Next(0, (int)mainWindow.Width), rnd.Next(0, (int)mainWindow.Height));

            return point;
        }

        private Rectangle GetRandomImage()
        {
            ImageBrush fruitBrush = new ImageBrush();
            int randomNumber = RandomHelper.GenerateRandomFruitType();

            fruitBrush.ImageSource = new BitmapImage(FruitImages.FirstOrDefault(f => f.Key == randomNumber).Value);

            var fruit = new Rectangle()
            {
                Tag = "Fruit",
                Height = 110,
                Width = 110,
                Fill = fruitBrush
            };

            return fruit;
        }
    }
}

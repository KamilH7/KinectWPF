using KinectWPF.Controllers.KinectController;
using KinectWPF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KinectWPF.Node
{
    class NodeController
    {
        Dictionary<int, Uri> FruitImages = new Dictionary<int, Uri>()
            {
                {  (int)FruitTypeEnum.STRAWBERRY, new Uri(string.Concat(@"pack://application:,,,/",Assembly.GetExecutingAssembly().GetName().Name,@";component/",@"Resources/FruitImages/strawberry.png"), UriKind.Absolute) },
                {  (int)FruitTypeEnum.BANANA, new Uri(string.Concat(@"pack://application:,,,/",Assembly.GetExecutingAssembly().GetName().Name,@";component/",@"Resources/FruitImages/banana.png"), UriKind.Absolute) },
                {  (int)FruitTypeEnum.APPLE, new Uri(string.Concat(@"pack://application:,,,/",Assembly.GetExecutingAssembly().GetName().Name,@";component/",@"Resources/FruitImages/apple.png"), UriKind.Absolute) }
            };

        private float nodeRadius = 50;
        private IInputController inputController;
        private MainWindow mainWindow;
        private const float spawnTimer = 2f;
        private float currentSpawnTimer;
        private List<Node> nodes = new List<Node>();
        private Random random;

        public NodeController(MainWindow mainWindow, IInputController inputController)
        {
            this.mainWindow = mainWindow;
            this.inputController = inputController;

            mainWindow.OnGameStart += GameStart;
            mainWindow.OnGameOver += GameEnd;

            random = new Random();
        }

        private void GameStart()
        {
            mainWindow.OnUpdate += Update;
        }

        private void GameEnd()
        {
            mainWindow.OnUpdate -= Update;
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
                if (inputController.IsHoveringOver(nodes[i].GetPosition(),nodeRadius))
                {
                    nodes[i].Destroy();
                    mainWindow.IncreaseScore();
                }
            }
        }

        private void HandleNodeSpawning()
        {
            currentSpawnTimer += MainWindow.DeltaTime;

            if (currentSpawnTimer >= spawnTimer)
            {
                GenerateNodes(2);

                currentSpawnTimer = 0;
            }
        }

        private void AddNode(Node newNode)
        {
            if (!nodes.Exists((node) => { return node == newNode; }))
            {
                nodes.Add(newNode);
                newNode.OnDestroy += RemoveNode;
            }
        }

        private void RemoveNode(Node removedNode)
        {
            removedNode.OnDestroy -= RemoveNode;
            nodes.Remove(removedNode);
        }

        private void GenerateNodes(int nodeCount)
        {
            for (int i = 0; i < nodeCount; ++i)
            {
                Point randomPosition;

                do
                {
                    randomPosition = GetRandomPosition();
                } while (inputController.IsHoveringOver(randomPosition,nodeRadius));

                Node newNode = new Node(mainWindow, GetRandomImage(), GetRandomPosition());
                AddNode(newNode);
            }
        }

        private Point GetRandomPosition()
        {
            double r = mainWindow.Width/2.5f * Math.Sqrt(random.NextDouble());
            double theta = random.NextDouble() * 2 * Math.PI;
            double x = mainWindow.Width / 2 + r * Math.Cos(theta);
            double y = mainWindow.Height / 2 + r * Math.Sin(theta);

            return new Point(x,y);
        }

        private Rectangle GetRandomImage()
        {
            ImageBrush fruitBrush = new ImageBrush();
            int randomNumber = random.Next(0, FruitImages.Count);

            fruitBrush.ImageSource = new BitmapImage(FruitImages.FirstOrDefault(f => f.Key == randomNumber).Value);

            var fruit = new Rectangle()
            {
                Tag = "Fruit",
                Height = nodeRadius * 2,
                Width = nodeRadius * 2,
                Fill = fruitBrush
            };

            return fruit;
        }
    }
}

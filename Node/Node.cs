using KinectWPF.Enums;
using KinectWPF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KinectWPF.Node
{
    class Node
    {
        public enum NodeType { Click };

        public float x, y;
        public float radius;
        public NodeType nodeType;
        public Rectangle sprite;

        public Node(float x, float y, float radius, NodeType nodeType)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;

            sprite = GetImage();

            this.nodeType = nodeType;
        }

        public Node()
        {

        }

        internal Rectangle GetImage()
        {
            ImageBrush fruitBrush = new ImageBrush();
            int randomNumber = RandomHelper.GenerateRandomFruitType();

            fruitBrush.ImageSource = new BitmapImage(GetFruitsInformations().FirstOrDefault(f => f.Key == randomNumber).Value);

            var fruit = new Rectangle()
            {
                Tag = "Fruit",
                Height = 110,
                Width = 110,
                Fill = fruitBrush
            };

            return fruit;
        }

        internal Dictionary<int, Uri> GetFruitsInformations()
        {
            return new Dictionary<int, Uri>()
            {
                {  (int)FruitTypeEnum.PAPAYA, new Uri(string.Concat(@"pack://application:,,,/",Assembly.GetExecutingAssembly().GetName().Name,@";component/",@"Images/papaya.png"), UriKind.Absolute) },
                {  (int)FruitTypeEnum.BANANA, new Uri(string.Concat(@"pack://application:,,,/",Assembly.GetExecutingAssembly().GetName().Name,@";component/",@"Images/banana.png"), UriKind.Absolute) },
                {  (int)FruitTypeEnum.PEACH, new Uri(string.Concat(@"pack://application:,,,/",Assembly.GetExecutingAssembly().GetName().Name,@";component/",@"Images/peach.png"), UriKind.Absolute) }
            };
        }
    }
}

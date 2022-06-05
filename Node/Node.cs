using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;


namespace KinectWPF.Node
{
    class Node
    {
        private MainWindow mainWindow;
        private Point position;
        private Rectangle sprite;

        public Node(MainWindow mainWindow, Rectangle sprite, Point position)
        {
            this.mainWindow = mainWindow;
            this.sprite = sprite;

            mainWindow.MainCanvas.Children.Add(sprite);

            this.position = position;

            Canvas.SetLeft(sprite, position.X);
            Canvas.SetTop(sprite, position.Y);
        }
        public void Destroy()
        {
            mainWindow.MainCanvas.Children.Remove(sprite);
        }

        public Point GetPosition()
        {
            return position;
        }
    }
}

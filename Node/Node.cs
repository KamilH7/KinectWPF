using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace KinectWPF.Node
{
    internal class Node
    {
        #region Events

        public event Action<Node> OnDestroy;

        #endregion

        #region Private Fields

        private MainWindow mainWindow;
        private Point position;
        private Rectangle sprite;

        private float destroyTime = 2.0f;
        private float currentDestroyTimer;

        #endregion

        #region Constructors

        public Node(MainWindow mainWindow, Rectangle sprite, Point position)
        {
            this.mainWindow = mainWindow;
            this.sprite = sprite;

            mainWindow.MainCanvas.Children.Add(sprite);

            this.position = position;

            Canvas.SetLeft(sprite, position.X - sprite.Height / 2);
            Canvas.SetTop(sprite, position.Y - sprite.Width / 2);

            mainWindow.OnUpdate += TimedDestroy;
            mainWindow.OnGameOver += Destroy;
        }

        #endregion

        #region Public Methods

        public void TimedDestroy()
        {
            currentDestroyTimer += MainWindow.DeltaTime;

            if (currentDestroyTimer >= destroyTime)
            {
                Destroy();
            }
        }

        public void Destroy()
        {
            OnDestroy?.Invoke(this);
            mainWindow.OnUpdate -= TimedDestroy;
            mainWindow.MainCanvas.Children.Remove(sprite);
        }

        public Point GetPosition()
        {
            return position;
        }

        #endregion
    }
}
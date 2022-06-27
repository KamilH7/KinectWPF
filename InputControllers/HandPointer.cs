using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KinectWPF.InputControllers
{
    internal enum HandSide
    {
        Left,
        Right
    }

    internal class HandPointer
    {
        #region Private Fields

        private MainWindow mainWindow;
        private Uri leftHandImage = new Uri(string.Concat(@"pack://application:,,,/", Assembly.GetExecutingAssembly().GetName().Name, @";component/", @"Resources/hand_icon_left.png"),
                                            UriKind.Absolute);
        private Uri rightHandImage = new Uri(string.Concat(@"pack://application:,,,/", Assembly.GetExecutingAssembly().GetName().Name, @";component/", @"Resources/hand_icon_right.png"),
                                             UriKind.Absolute);
        private Rectangle hand;

        #endregion

        #region Public Properties

        public Point position { get; private set; }

        #endregion

        #region Constructors

        public HandPointer(MainWindow mainWindow, HandSide handSide)
        {
            this.mainWindow = mainWindow;
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(handSide == HandSide.Left ? leftHandImage : rightHandImage);

            hand = new Rectangle { Tag = "Hand", Height = 100, Width = 100, Fill = imageBrush };

            mainWindow.MainCanvas.Children.Add(hand);
        }

        #endregion

        #region Public Methods

        public void SetPosition(Point position)
        {
            this.position = position;

            Canvas.SetLeft(hand, position.X - hand.Width / 2);
            Canvas.SetTop(hand, position.Y - hand.Height / 2);
        }

        public bool IsHoveringOver(Point point, float radius)
        {
            double sqrDistance = Math.Pow(point.X - position.X, 2) + Math.Pow(point.Y - position.Y, 2);
            double distance = Math.Sqrt(sqrDistance);

            return distance <= radius + hand.Width / 4 ? true : false;
        }

        public void Destroy()
        {
            mainWindow.MainCanvas.Children.Remove(hand);
        }

        #endregion
    }
}
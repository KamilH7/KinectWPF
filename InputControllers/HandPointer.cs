using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KinectWPF.InputControllers
{
    enum HandSide
    {
        Left,
        Right
    }

    class HandPointer
    {
        private MainWindow mainWindow;
        private Uri leftHandImage = new Uri(string.Concat(@"pack://application:,,,/", Assembly.GetExecutingAssembly().GetName().Name, @";component/", @"Resources/hand_icon_left.png"), UriKind.Absolute);
        private Uri rightHandImage = new Uri(string.Concat(@"pack://application:,,,/", Assembly.GetExecutingAssembly().GetName().Name, @";component/", @"Resources/hand_icon_right.png"), UriKind.Absolute);
        private Rectangle hand;

        public HandPointer(MainWindow mainWindow, HandSide handSide)
        {
            this.mainWindow = mainWindow;
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(handSide == HandSide.Left ? leftHandImage : rightHandImage);

            hand = new Rectangle()
            {
                Tag = "Hand",
                Height = 100,
                Width = 100,
                Fill = imageBrush
            };

            mainWindow.MainCanvas.Children.Add(hand);
        }

        public void SetPosition(Point position)
        {
            Canvas.SetLeft(hand, position.X - hand.Width / 2);
            Canvas.SetTop(hand, position.Y - hand.Height / 2);
        }

        public void Destroy()
        {
            mainWindow.MainCanvas.Children.Remove(hand);
        }
    }
}

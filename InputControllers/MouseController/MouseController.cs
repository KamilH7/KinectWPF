using KinectWPF.Controllers.KinectController;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace KinectWPF.Controllers.MouseController
{
    class MouseController : IInputController
    {
        private Rectangle handImage;
        private MainWindow window;

        public MouseController(Rectangle handImage, MainWindow mainWindow)
        {
            this.handImage = handImage;
            handImage.Visibility = Visibility.Visible;
            mainWindow.OnUpdate += DrawPointer;
        }

        public void Initialize(MainWindow window)
        {
            this.window = window;
            window.OnUpdate += DrawPointer;
        }

        public bool IsHoveringOver(Point point, float radius)
        {
            Point mousePosition = GetMousePositionOnScreen();

            double sqrDistance = Math.Pow(point.X - mousePosition.X, 2) + Math.Pow(point.Y - mousePosition.Y, 2);
            double distance = Math.Sqrt(sqrDistance);

            return distance <= radius ? true : false;
        }

        public bool IsInStartPosition()
        {
            Point mousePosition = GetMousePositionOnScreen();
            Point middle = new Point(window.Width / 2, window.Height / 2);

            bool isXOkay = mousePosition.X < middle.X + 50 && 
                           mousePosition.X > middle.X - 50;

            bool isYOkay = mousePosition.Y < middle.Y + 50 && 
                           mousePosition.Y > middle.Y - 50;

            return isXOkay && isYOkay;
        }

        private void DrawPointer()
        {
            Point mousePosition = GetMousePositionOnScreen();
            Canvas.SetTop(handImage, mousePosition.Y - handImage.Height / 2);
            Canvas.SetLeft(handImage, mousePosition.X - handImage.Width / 2);
        }

        private Point GetMousePositionOnScreen()
        {
            if (window.IsVisible)
            {
                return Mouse.GetPosition(window);
            }
            else
            {
                return new Point(0, 0);
            }
        }
    }
}

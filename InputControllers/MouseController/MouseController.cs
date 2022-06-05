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
        private float acceptanceRadius = 5f;
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

        public bool IsHoveringOver(Point point)
        {
            Point mousePosition = GetMousePositionOnScreen();

            double sqrDistance = Math.Pow(point.X - mousePosition.X, 2) - Math.Pow(point.Y - mousePosition.Y, 2);
            double distance = Math.Sqrt(sqrDistance);

            return distance <= acceptanceRadius ? true : false;
        }

        private void DrawPointer()
        {
            Point mousePosition = GetMousePositionOnScreen();
            Canvas.SetTop(handImage, mousePosition.Y);
            Canvas.SetLeft(handImage, mousePosition.X);
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

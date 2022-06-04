using KinectWPF.Controllers.KinectController;
using System;
using System.Windows;
using System.Windows.Input;

namespace KinectWPF.Controllers.MouseController
{
    class MouseController : IInputController
    {
        private float acceptanceRadius = 5f;
        private MainWindow window;

        public void Initialize(MainWindow window)
        {
            this.window = window;
        }

        public bool IsHoveringOver(Point point)
        {
            Point mousePosition = GetMousePositionOnScreen();

            double sqrDistance = Math.Pow(point.X - mousePosition.X, 2) - Math.Pow(point.Y - mousePosition.Y, 2);
            double distance = Math.Sqrt(sqrDistance);

            return distance <= acceptanceRadius ? true : false;
        }

        private Point GetMousePositionOnScreen()
        {
            return window.PointToScreen(Mouse.GetPosition(window));
        }
    }
}

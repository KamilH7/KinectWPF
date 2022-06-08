using KinectWPF.Controllers.KinectController;
using KinectWPF.InputControllers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KinectWPF.Controllers.MouseController
{
    class MouseController : IInputController
    {
        private HandPointer handPointer;
        private MainWindow window;

        public MouseController(MainWindow mainWindow)
        {
            handPointer = new HandPointer(mainWindow, HandSide.Left);
            mainWindow.OnUpdate += DrawPointer;
        }

        public void Initialize(MainWindow window)
        {
            this.window = window;
            window.OnUpdate += DrawPointer;
        }

        public bool IsHoveringOver(Point point, float radius)
        {
            return handPointer.IsHoveringOver(point, radius);
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
            handPointer.SetPosition(GetMousePositionOnScreen());
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

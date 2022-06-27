using System.Windows;
using System.Windows.Input;
using KinectWPF.Calibration;
using KinectWPF.Controllers.KinectController;
using KinectWPF.InputControllers;

namespace KinectWPF.Controllers.MouseController
{
    internal class MouseController : IInputController
    {
        #region Private Fields

        private HandPointer handPointer;
        private MainWindow window;
        private PointTransformer pointTransformer;

        #endregion

        #region Constructors

        public MouseController(MainWindow mainWindow)
        {
            handPointer = new HandPointer(mainWindow, HandSide.Left);
            mainWindow.OnUpdate += DrawPointer;
        }

        #endregion

        #region Public Methods

        public Point GetCalibrationPosition(CalibrationStage calibrationStage)
        {
            return handPointer.position;
        }

        public void Initialize(MainWindow window, PointTransformer pointTransformer)
        {
            this.window = window;
            this.pointTransformer = pointTransformer;
            window.OnUpdate += DrawPointer;
        }

        public bool IsHoveringOver(Point point, float radius)
        {
            return handPointer.IsHoveringOver(point, radius);
        }

        public bool IsInSamplePosition()
        {
            return Mouse.LeftButton == MouseButtonState.Pressed;
        }

        public bool IsInStartPosition()
        {
            Point mousePosition = GetMousePositionOnScreen();
            Point middle = new Point(window.Width / 2, window.Height / 2);

            bool isXOkay = mousePosition.X < middle.X + 50 && mousePosition.X > middle.X - 50;

            bool isYOkay = mousePosition.Y < middle.Y + 50 && mousePosition.Y > middle.Y - 50;

            return isXOkay && isYOkay;
        }

        #endregion

        #region Private Methods

        private void DrawPointer()
        {
            handPointer.SetPosition(pointTransformer.TransformPoint(GetMousePositionOnScreen()));
        }

        private Point GetMousePositionOnScreen()
        {
            if (window.IsVisible)
            {
                return Mouse.GetPosition(window);
            }

            return new Point(0, 0);
        }

        #endregion
    }
}
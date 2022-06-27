using System.Windows;
using KinectWPF.Controllers.KinectController;

namespace KinectWPF.Calibration
{
    internal class PointTransformer
    {
        #region Private Fields

        private Point bottomRightPosition;
        private Point topLeftPosition;

        private CalibrationStage calibrationStage;
        private IInputController inputController;
        private MainWindow mainWindow;

        private string tempText;
        private Point temp;

        #endregion

        #region Public Properties

        public bool IsCallibrating { get; private set; }

        #endregion

        #region Constructors

        public PointTransformer(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            bottomRightPosition = new Point(mainWindow.Width, mainWindow.Height);
            topLeftPosition = new Point(0, 0);
        }

        #endregion

        #region Public Methods

        public void StartCalibration(IInputController inputController)
        {
            this.inputController = inputController;

            calibrationStage = CalibrationStage.BottomRightData;
            mainWindow.OnUpdate += LookForSamplePosition;
            IsCallibrating = true;
            tempText = (string) mainWindow.StartText.Content;
            mainWindow.StartText.Content = "Prawy dolny róg";
        }

        public Point TransformPoint(Point point)
        {
            double transformedX = TransformCoordinate(point.X, topLeftPosition.X, bottomRightPosition.X, 0f, mainWindow.Width);
            double transformedY = TransformCoordinate(point.Y, topLeftPosition.Y, bottomRightPosition.Y, 0f, mainWindow.Height);

            return new Point(transformedX, transformedY);
        }

        #endregion

        #region Private Methods

        private void LookForSamplePosition()
        {
            if (inputController.IsInSamplePosition())
            {
                switch (calibrationStage)
                {
                    case CalibrationStage.BottomRightData :
                        SampleBottomRightData();

                        break;
                    case CalibrationStage.TopLeftData :
                        SampleTopLeftData();

                        break;
                }
            }
        }

        private void Await()
        {
            if (!inputController.IsInSamplePosition())
            {
                mainWindow.OnUpdate += LookForSamplePosition;
                mainWindow.OnUpdate -= Await;
            }
        }

        private void SampleBottomRightData()
        {
            calibrationStage = CalibrationStage.TopLeftData;

            mainWindow.OnUpdate -= LookForSamplePosition;
            mainWindow.OnUpdate += Await;

            temp = inputController.GetCalibrationPosition(CalibrationStage.BottomRightData);

            mainWindow.StartText.Content = "Lewy górny róg";
        }

        private void SampleTopLeftData()
        {
            mainWindow.OnUpdate -= LookForSamplePosition;

            topLeftPosition = inputController.GetCalibrationPosition(CalibrationStage.TopLeftData);
            bottomRightPosition = temp;

            IsCallibrating = false;

            mainWindow.StartText.Content = tempText;
        }

        private double TransformCoordinate(double INPUT, double INPUT_MIN, double INPUT_MAX, double GOAL_MIN, double GOAL_MAX)
        {
            return (INPUT - INPUT_MIN) / (INPUT_MAX - INPUT_MIN) * (GOAL_MAX - GOAL_MIN) + GOAL_MIN;
        }

        #endregion
    }
}
using KinectWPF.Controllers.KinectController;
using System.Windows;

namespace KinectWPF.Calibration
{
    class PointTransformer
    {
        public bool IsCallibrating { get; private set; }

        Point bottomRightPosition;
        Point topLeftPosition;

        CalibrationStage calibrationStage;
        IInputController inputController;
        MainWindow mainWindow;

        string tempText;
        Point temp;

        public PointTransformer(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            bottomRightPosition = new Point(mainWindow.Width,mainWindow.Height);
            topLeftPosition = new Point(0, 0);
        }

        public void StartCalibration(IInputController inputController)
        {
            this.inputController = inputController;

            calibrationStage = CalibrationStage.BottomRightData;
            mainWindow.OnUpdate += LookForSamplePosition;
            IsCallibrating = true;
            tempText = (string) mainWindow.StartText.Content;
            mainWindow.StartText.Content = "Prawy dolny róg";
        }

        private void LookForSamplePosition()
        {
            if (inputController.IsInSamplePosition())
            {
                switch (calibrationStage)
                {
                    case CalibrationStage.BottomRightData:
                        SampleBottomRightData();
                        break;
                    case CalibrationStage.TopLeftData:
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

        public Point TransformPoint(Point point)
        {
            double transformedX = TransformCoordinate(point.X, topLeftPosition.X, bottomRightPosition.X, 0f, mainWindow.Width);
            double transformedY = TransformCoordinate(point.Y, topLeftPosition.Y, bottomRightPosition.Y, 0f, mainWindow.Height);

            return new Point(transformedX,transformedY);
        }

        private double TransformCoordinate(double INPUT, double INPUT_MIN, double INPUT_MAX, double GOAL_MIN, double GOAL_MAX)
        {
            return (INPUT - INPUT_MIN) / (INPUT_MAX - INPUT_MIN) * (GOAL_MAX - GOAL_MIN) + GOAL_MIN;
        }
    }
}

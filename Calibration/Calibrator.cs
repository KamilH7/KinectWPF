using KinectWPF.Controllers.KinectController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KinectWPF.Calibration
{
    class Calibrator
    {
        public bool IsCallibrating { get; private set; }

        Point BottomRightPosition;
        Point TopLeftPosition;

        CalibrationStage calibrationStage;
        IInputController inputController;
        MainWindow mainWindow;

        public Calibrator(MainWindow mainWindow, IInputController inputController)
        {
            this.inputController = inputController;
            this.mainWindow = mainWindow;
        }

        public void StartCalibration()
        {
            calibrationStage = CalibrationStage.TopLeftData;
            mainWindow.OnUpdate += LookForSamplePosition;
            IsCallibrating = true;
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

        private void FinishCalibration()
        {
            inputController.SetOffset(new Point());

            IsCallibrating = false;
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

            BottomRightPosition = inputController.GetPosition();
        }

        private void SampleTopLeftData()
        {
            mainWindow.OnUpdate -= LookForSamplePosition;

            TopLeftPosition = inputController.GetPosition();

            FinishCalibration();
        }
    }
}

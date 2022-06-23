using KinectWPF.Calibration;
using System.Windows;

namespace KinectWPF.Controllers.KinectController
{
    interface IInputController
    {
        void Initialize(MainWindow window, PointTransformer pointTransformer);
        bool IsHoveringOver(Point point, float radius);
        bool IsInStartPosition();
        bool IsInSamplePosition();
        Point GetCalibrationPosition(CalibrationStage calibrationStage);
    }
}

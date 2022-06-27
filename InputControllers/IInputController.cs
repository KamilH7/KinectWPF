using System.Windows;
using KinectWPF.Calibration;

namespace KinectWPF.Controllers.KinectController
{
    internal interface IInputController
    {
        #region Public Methods

        void Initialize(MainWindow window, PointTransformer pointTransformer);
        bool IsHoveringOver(Point point, float radius);
        bool IsInStartPosition();
        bool IsInSamplePosition();
        Point GetCalibrationPosition(CalibrationStage calibrationStage);

        #endregion
    }
}
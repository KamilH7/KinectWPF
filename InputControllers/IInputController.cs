using System.Windows;

namespace KinectWPF.Controllers.KinectController
{
    interface IInputController
    {
        void Initialize(MainWindow window);
        bool IsHoveringOver(Point point, float radius);
        bool IsInStartPosition();
        bool IsInSamplePosition();
        void SetOffset(Point offset);
        Point GetPosition();
    }
}

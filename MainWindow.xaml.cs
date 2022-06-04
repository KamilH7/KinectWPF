using KinectWPF.Controllers.KinectController;
using KinectWPF.Controllers.MouseController;
using System.Windows;
using System.Windows.Controls;

namespace KinectWPF
{
    public partial class MainWindow : Window
    {
        public Image MainImg { get; private set; }

        private IInputController inputController;
        private bool useMouse = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeReferences();
            InitializeController();
        }

        public void InitializeReferences()
        {
            this.MainImg = kinectImage;
        }

        public void InitializeController()
        {
            if (useMouse)
            {
                inputController = new MouseController();
            }
            else
            {
                inputController = new KinectController();
            }

            inputController.Initialize(this);
        }

    }
}

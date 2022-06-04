using KinectWPF.Controllers.KinectController;
using KinectWPF.Controllers.MouseController;
using System;
using System.Windows;
using System.Windows.Controls;

namespace KinectWPF
{
    public partial class MainWindow : Window
    {
        public Image MainImg { get; private set; }
        public event Action OnUpdate;
       
        private IInputController inputController;
        private bool useMouse = true;
        private bool running = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeReferences();
            InitializeController();

            Loop();
        }

        public void Loop()
        {
            while (running)
            {
                OnUpdate?.Invoke();
            }
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

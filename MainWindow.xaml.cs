using KinectWPF.Controllers.KinectController;
using KinectWPF.Controllers.MouseController;
using KinectWPF.Node;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace KinectWPF
{
    public partial class MainWindow : Window
    {
        public Image MainImg { get; private set; }
        public static float DeltaTime = 0;
        public event Action OnUpdate;
       
        private IInputController inputController;
        private bool useMouse = true;
        private bool running = true;

        public MainWindow()
        {
            InitializeComponent();
            InitializeReferences();
            InitializeController();

            NodeManager nodeController = new NodeManager((float) MainImg.Width, (float) MainImg.Height, this);
            Loop();
        }

        public void Loop()
        {
            while (running)
            {
                DateTime startTime = DateTime.Now;

                OnUpdate?.Invoke();

                Thread.Sleep(10);

                TimeSpan span = DateTime.Now.Subtract(startTime);

                DeltaTime = (float) span.TotalSeconds;
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

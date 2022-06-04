using KinectWPF.Controllers.KinectController;
using KinectWPF.Controllers.MouseController;
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
        public float DeltaTime = 0;
        public event Action OnUpdate;
       
        private IInputController inputController;
        private bool useMouse = true;
        private bool running = true;

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
                DateTime startTime = DateTime.Now;

                OnUpdate?.Invoke();

                Thread.Sleep(10);

                TimeSpan span = DateTime.Now.Subtract(startTime);

                DeltaTime = (float) span.TotalMilliseconds;

                Trace.WriteLine(DeltaTime);
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

using KinectWPF.Controllers.KinectController;
using KinectWPF.Controllers.MouseController;
using KinectWPF.Node;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace KinectWPF
{
    public partial class MainWindow : Window
    {
        public Canvas MainCanvas { get; private set; }
        public Image KinectImage { get; private set; }
        public static float DeltaTime = 0;
        public event Action OnUpdate;

        private DispatcherTimer looper = new DispatcherTimer();
        private IInputController inputController;
        private const bool useMouse = true;

        public MainWindow()
        {
            InitializeComponent();
            InitializeReferences();

            MainCanvas.Focus();

            InitializeController();
            InitializeGame();
        }

        private void InitializeGame()
        {
            NodeSpawner nodeSpawner = new NodeSpawner(this, inputController);
            StartMainThread();
        }

        private void StartMainThread()
        {
            CompositionTarget.Rendering += Loop;
        }

        private void Loop(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;

            OnUpdate?.Invoke();

            Thread.Sleep(1);

            TimeSpan span = DateTime.Now.Subtract(start);

            DeltaTime = (float) span.TotalSeconds;

            Trace.WriteLine(DeltaTime);
        }

        private void InitializeReferences()
        {
            MainCanvas = canvas;
            KinectImage = kinectImage;
        }

        private void InitializeController()
        {
            if (useMouse)
            {
                inputController = new MouseController(leftHand,this);
            }
            else
            {
                inputController = new KinectController(leftHand,rightHand);
            }

            inputController.Initialize(this);
        }

    }
}

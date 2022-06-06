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
        public static float DeltaTime = 0;

        public event Action OnUpdate;
        public event Action OnGameStart;
        public event Action OnGameOver;

        public Canvas MainCanvas { get; private set; }
        public Image KinectImage { get; private set; }


        private DispatcherTimer looper = new DispatcherTimer();
        private IInputController inputController;
        private const bool useMouse = true;

        private const float gameTime = 30;
        private float gameTimer;

        private int score = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeReferences();
            InitializeController();

            NodeSpawner nodeSpawner = new NodeSpawner(this, inputController);
            CompositionTarget.Rendering += MainLoop;

            InitializeGame();
        }


        private void InitializeGame()
        {
            EnableStartUI();

            CompositionTarget.Rendering -= EndLoop;
            CompositionTarget.Rendering += StartLoop;
        }

        private void StartGame()
        {
            OnGameStart.Invoke();

            SetTime(gameTime);
            SetScore(0);

            EnableGameUI();

            CompositionTarget.Rendering -= StartLoop;
            CompositionTarget.Rendering += GameLoop;
        }

        private void EndGame()
        {
            OnGameOver.Invoke();

            UpdateEndScoreText();

            EnableEndUI();

            CompositionTarget.Rendering -= GameLoop;
            CompositionTarget.Rendering += EndLoop;
        }


        private void MainLoop(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;

            OnUpdate?.Invoke();

            Thread.Sleep(1);

            TimeSpan span = DateTime.Now.Subtract(start);

            DeltaTime = (float)span.TotalSeconds;
        }

        private void StartLoop(object sender, EventArgs e)
        {
            if (inputController.IsInStartPosition())
            {
                StartGame();
            }
        }


        private void GameLoop(object sender, EventArgs e)
        {
            gameTimer -= DeltaTime;

            UpdateGameTimerText();

            if (ShouldGameEnd())
            {
                EndGame();
            }
        }

        private void EndLoop(object sender, EventArgs e)
        {
            if (inputController.IsInStartPosition())
            {
                InitializeGame();
            }
        }

        public void IncreaseScore()
        {
            score += 1;
            UpdateScoreText();
        }

        private void SetScore(int score)
        {
            this.score = score;
            UpdateScoreText();
        }

        private void SetTime(float time)
        {
            this.gameTimer = time;
            UpdateScoreText();
        }

        private void UpdateGameTimerText()
        {
            TimeNumber.Content = ((int) gameTimer).ToString();
        }

        private void UpdateScoreText()
        {
            ScoreNumber.Content = score.ToString();
        }

        private void UpdateEndScoreText()
        {
            EndNumber.Content = score.ToString();
        }

        private void EnableStartUI()
        {
            Hide(ScoreText);
            Hide(ScoreNumber);
            Hide(TimeText);
            Hide(TimeNumber);
            Hide(EndNumber);
            Hide(EndText);

            Show(StartText);
        }

        private void EnableGameUI()
        {
            Hide(StartText);
            Hide(EndText);
            Hide(EndNumber);

            Show(ScoreText);
            Show(ScoreNumber);
            Show(TimeText);
            Show(TimeNumber);
        }

        private void EnableEndUI()
        {
            Hide(ScoreText);
            Hide(ScoreNumber);
            Hide(TimeText);
            Hide(TimeNumber);
            Hide(StartText);

            Show(EndNumber);
            Show(EndText);
        }


        private void Hide(UIElement element)
        {
            element.Visibility = Visibility.Hidden;
        }

        private void Show(UIElement element)
        {
            element.Visibility = Visibility.Visible;
        }

        private bool ShouldGameEnd() => gameTimer <= 0;

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

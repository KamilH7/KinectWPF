using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using KinectWPF.Calibration;
using KinectWPF.Controllers.KinectController;
using KinectWPF.Controllers.MouseController;
using KinectWPF.Node;

namespace KinectWPF
{
    public partial class MainWindow : Window
    {
        #region Events

        public event Action OnUpdate;
        public event Action OnGameStart;
        public event Action OnGameOver;

        #endregion

        #region Private Fields

        private DateTime previousFrameTime;

        private IInputController inputController;
        private PointTransformer pointTransformer;
        private float gameTimer;

        private int score;

        private float startScreenMinTime = 2f;
        private float starScreenTimer;

        #endregion

        #region Constants

        private const bool useMouse = true;
        private const float gameTime = 30;
        public static float DeltaTime;

        #endregion

        #region Public Properties

        public Label MainText { get; private set; }
        public Canvas MainCanvas { get; private set; }
        public Image KinectImage { get; private set; }

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            InitializeReferences();

            pointTransformer = new PointTransformer(this);

            InitializeController();

            pointTransformer.StartCalibration(inputController);

            NodeController nodeSpawner = new NodeController(this, inputController);

            CompositionTarget.Rendering += MainLoop;

            InitializeGame();
        }

        #endregion

        #region Public Methods

        public void IncreaseScore()
        {
            score += 1;
            UpdateScoreText();
        }

        #endregion

        #region Private Methods

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
            if (previousFrameTime == null)
            {
                previousFrameTime = DateTime.Now;
                DeltaTime = 0;
            }
            else
            {
                TimeSpan span = DateTime.Now.Subtract(previousFrameTime);
                DeltaTime = (float) span.TotalSeconds;
                previousFrameTime = DateTime.Now;
            }

            OnUpdate?.Invoke();
        }

        private void StartLoop(object sender, EventArgs e)
        {
            if (starScreenTimer < startScreenMinTime)
            {
                starScreenTimer += DeltaTime;
            }
            else if (inputController.IsInStartPosition() && !pointTransformer.IsCallibrating)
            {
                starScreenTimer = 0;
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

        private void SetScore(int score)
        {
            this.score = score;
            UpdateScoreText();
        }

        private void SetTime(float time)
        {
            gameTimer = time;
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
            MainText = StartText;
            MainCanvas = canvas;
            KinectImage = kinectImage;
        }

        private void InitializeController()
        {
            if (useMouse)
            {
                inputController = new MouseController(this);
            }
            else
            {
                inputController = new KinectController(this);
            }

            inputController.Initialize(this, pointTransformer);
        }

        #endregion
    }
}
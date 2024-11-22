using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double GameBoardWidth = 350;
        private GameBoard gameBoard;
        private const int CellSize = 22;
        private const int GameBoardNumOfColumns = 10;
        private const int GameBoardNumOfRows = 18;
        private Block currentBlock;
        private Block nextBlock;
        //private List<Block> activeBlocks = new List<Block>();

        private double normalFallSpeed;
        private double fastFallSpeed;
        private bool isFastFalling;

        private bool isSpacePressed = false;

        private DispatcherTimer gameTimer;
        private DispatcherTimer movementTimer;
        private string movementDirection;

        private bool isMovingLeft = false;
        private bool isMovingRight = false;

        public int score { get; set; } = 0;
        private int gameLvl { get; set; } = 1;

        private MediaPlayer mediaPlayer;

        public MainWindow()
        {
            InitializeComponent();
            PlaySoundtrack();

            // Set DataContext for binding
            DataContext = this;

            gameBoard = new GameBoard(this, GameBoardNumOfRows, GameBoardNumOfColumns);
            GameBoardWidth = gameBoard.Board.GetLength(1) * CellSize;

            gameBoard.InitializeUIGameBoard(GameCanvas, CellSize);

            // Generate the first block
            currentBlock = new Block();
            currentBlock.GenerateBlock(gameBoard);
            currentBlock.PlaceBlockOnBoard(gameBoard);

            // Generate the second block
            nextBlock = new Block();
            nextBlock.GenerateBlock(gameBoard);
            RenderNextBlock(nextBlock);

            gameBoard.UpdateUIGameBoard(currentBlock);


            normalFallSpeed = currentBlock.FallSpeedMs;
            fastFallSpeed = normalFallSpeed / 10;
            isFastFalling = false;

            // Set up the game timer
            gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(normalFallSpeed)
            };
            gameTimer.Tick += GameTick;
            gameTimer.Start();

            // Set up movement timer for left/right movement
            movementTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            movementTimer.Tick += MovementTick;

            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
        }

        private void GameTick(object sender, EventArgs e)
        {
            if (!currentBlock.CanBlockFall(gameBoard))
            {
                HandleBlockLanding();
            }
            else
            {
                currentBlock.FallDown(gameBoard);
                gameBoard.UpdateUIGameBoard(currentBlock);
            }
        }

        private void HandleBlockLanding()
        {
            // activate movement again after clicking space
            isSpacePressed = false;

            //activeBlocks.Add(currentBlock);
            gameBoard.ClearFullRows();

            // Create a new block
            currentBlock.InheritBlock(gameBoard, nextBlock);
            currentBlock.PlaceBlockOnBoard(gameBoard);

            nextBlock = new Block();
            nextBlock.GenerateBlock(gameBoard);
            RenderNextBlock(nextBlock);

            gameBoard.UpdateUIGameBoard(currentBlock);

            // Check if game over
            if (!currentBlock.CanBlockFall(gameBoard))
            {
                GameOver();
            }
            else
            {
                // 4 because 1 square == 1 point (Each block has 4 squares)
                AddScore(4);

                UpdateGameLevel();
            }
            gameTimer.Interval = TimeSpan.FromMilliseconds(normalFallSpeed);
        }

        private void MovementTick(object sender, EventArgs e)
        {
            currentBlock.ClearBlockFromBoard(gameBoard);

            if (movementDirection == "Left")
            {
                currentBlock.MoveLeft(gameBoard);
            }
            if (movementDirection == "Right")
            {
                currentBlock.MoveRight(gameBoard);
            }

            currentBlock.PlaceBlockOnBoard(gameBoard);
            gameBoard.UpdateUIGameBoard(currentBlock);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Clear the current block position before moving
            currentBlock.ClearBlockFromBoard(gameBoard);

            if (e.Key == Key.Space)
            {
                gameTimer.Interval = TimeSpan.FromMilliseconds(1);
                gameTimer.Stop();
                isSpacePressed = true;
                currentBlock.FallToTheVeryBottom(gameBoard);
                currentBlock.PlaceBlockOnBoard(gameBoard);
                gameBoard.UpdateUIGameBoard(currentBlock);
                gameTimer.Start();
                return;
            }
            if (isSpacePressed) return;

            if (e.Key == Key.Left && !isMovingLeft)
            {
                isMovingLeft = true;
                currentBlock.MoveLeft(gameBoard);
                StartMovement("Left");
            }
            if (e.Key == Key.Right && !isMovingRight)
            {
                isMovingRight = true;
                currentBlock.MoveRight(gameBoard);
                StartMovement("Right");
            }
            if (e.Key == Key.Down && !isFastFalling)
            {
                isFastFalling = true;
                gameTimer.Interval = TimeSpan.FromMilliseconds(fastFallSpeed);
            }
            if (e.Key == Key.Up)
            {
                currentBlock.Rotate(gameBoard);
            }

            currentBlock.PlaceBlockOnBoard(gameBoard);
            gameBoard.UpdateUIGameBoard(currentBlock);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                isMovingLeft = false;
                StopMovement();
            }
            if (e.Key == Key.Right)
            {
                isMovingRight = false;
                StopMovement();
            }
            if (e.Key == Key.Down && isFastFalling)
            {
                isFastFalling = false;
                gameTimer.Interval = TimeSpan.FromMilliseconds(normalFallSpeed);
            }
        }

        private void StartMovement(string direction)
        {
            if (movementDirection != direction)
            {
                movementDirection = direction;
                movementTimer.Start();
            }
        }

        private void StopMovement()
        {
            if (!isMovingLeft && !isMovingRight)
            {
                movementTimer.Stop();
                movementDirection = null;
            }
        }

        public void AddScore(int points)
        {
            score += points;
            scoreLabel.Text = "Score: " + score.ToString();
        }

        private void UpdateGameLevel()
        {
            if (score >= 100) UpdateGameInfo(2, 800);
            if (score >= 250) UpdateGameInfo(3, 700);
            if (score >= 500) UpdateGameInfo(4, 600);
            if (score >= 1000) UpdateGameInfo(5, 500);
            if (score >= 2000) UpdateGameInfo(6, 400);
            if (score >= 5000) UpdateGameInfo(7, 300);
        }

        private void UpdateGameInfo(int level, int newFallSpeedMs)
        {
            gameLvl = level;
            gameLvlLabel.Text = "Level " + gameLvl.ToString();
            currentBlock.FallSpeedMs = newFallSpeedMs;
            normalFallSpeed = currentBlock.FallSpeedMs;
            fastFallSpeed = normalFallSpeed / 10;
            gameTimer.Interval = TimeSpan.FromMilliseconds(normalFallSpeed);
        }

        private void RenderNextBlock(Block block)
        {
            NextBlockCanvas.Children.Clear();

            int[,] shape = block.currentShape;
            Brush color = block.BlockColor;

            // Calculate the size of the shape
            int rows = shape.GetLength(0);
            int cols = shape.GetLength(1);

            // Calculate scaling for rectangles
            double cellSize = CellSize * 1.5;
            double rectangleSize = cellSize + 1;

            // Calculate the center offset
            double canvasWidth = NextBlockCanvas.ActualWidth;
            double canvasHeight = NextBlockCanvas.ActualHeight;
            double shapeWidth = cols * cellSize;
            double shapeHeight = rows * cellSize;
            double offsetX = (canvasWidth - shapeWidth) / 2;
            double offsetY = (canvasHeight - shapeHeight) / 2;

            // If its not a square go a little bit down
            if (block.currentShape != block.TypeOfBlock[3])
            {
                offsetX += 16;
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (shape[i, j] != 0)
                    {
                        var rectangle = new Rectangle
                        {
                            Width = rectangleSize,
                            Height = rectangleSize,
                            Fill = color,
                            Stroke = Brushes.Black,
                            StrokeThickness = 2
                        };

                        Canvas.SetTop(rectangle, offsetX + i * cellSize + 1);
                        Canvas.SetLeft(rectangle, offsetY + j * cellSize + 1);
                        NextBlockCanvas.Children.Add(rectangle);
                    }
                }
            }
        }

        private void GameOver()
        {
            gameTimer.Stop();
            mediaPlayer.Stop();
            MessageBox.Show("Game over. Try again!");
            ResetGame();
        }

        private void ResetGame()
        {
            // Start the music
            mediaPlayer.Play();

            gameBoard = new GameBoard(this, GameBoardNumOfRows, GameBoardNumOfColumns);
            gameBoard.InitializeUIGameBoard(GameCanvas, CellSize);

            // Generate the first block
            currentBlock = new Block();
            currentBlock.GenerateBlock(gameBoard);
            gameBoard.UpdateUIGameBoard(currentBlock);

            // Generate the second block
            nextBlock = new Block();
            nextBlock.GenerateBlock(gameBoard);
            RenderNextBlock(nextBlock);

            // Restart the timer
            gameTimer.Interval = TimeSpan.FromMilliseconds(normalFallSpeed);
            gameTimer.Start();

            // Reset the score
            score = 0;
            scoreLabel.Text = "Score: 0";

            // Reset the level
            gameLvlLabel.Text = "Level 0";
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Settings works");
        }

        private void PlaySoundtrack()
        {
            try
            {
                mediaPlayer = new MediaPlayer();
                //mediaPlayer.Open(new Uri("pack://application:,,,/Resources/TetrisTheme.mp3")); // Upewnij się, że ścieżka jest poprawna
                mediaPlayer.Open(new Uri("tetrisTheme.mp3", UriKind.Relative));
                mediaPlayer.MediaOpened += (sender, e) =>
                {
                    //MessageBox.Show("Plik audio załadowany pomyślnie.");
                };
                mediaPlayer.MediaFailed += (sender, e) =>
                {
                    MessageBox.Show("Błąd odtwarzania pliku audio: " + e.ErrorException.Message);
                };

                mediaPlayer.MediaEnded += (sender, e) =>
                {
                    mediaPlayer.Position = TimeSpan.Zero;
                    mediaPlayer.Play();
                };

                mediaPlayer.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex.Message);
            }
        }
    }
}
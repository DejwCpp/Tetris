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

        public MainWindow()
        {
            InitializeComponent();

            // Set DataContext for binding
            DataContext = this;

            gameBoard = new GameBoard(this, GameBoardNumOfRows, GameBoardNumOfColumns);
            GameBoardWidth = gameBoard.Board.GetLength(1) * CellSize;

            gameBoard.InitializeUIGameBoard(GameCanvas, CellSize);

            // Generate the first block
            currentBlock = new Block();
            currentBlock.GenerateBlock(gameBoard);
            currentBlock.PlaceBlockOnBoard(gameBoard);

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

                // Check if game over
                if (!currentBlock.CanBlockFall(gameBoard))
                {
                    GameOver();
                    return;
                }
                // 4 because 1 square == 1 point (Each block has 4 squares)
                AddScore(4);

                UpdateGameLevel();
            }
            else
            {
                currentBlock.FallDown(gameBoard);
            }
            gameBoard.UpdateUIGameBoard(currentBlock);
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
                isSpacePressed = true;
                currentBlock.FallToTheVeryBottom(gameBoard);
                currentBlock.PlaceBlockOnBoard(gameBoard);
                gameBoard.UpdateUIGameBoard(currentBlock);
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
            scoreLabel.Text = "Wynik: " + score.ToString();
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
            gameLvlLabel.Text = "Poziom: " + gameLvl.ToString();
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

            for (int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0;  j < shape.GetLength(1); j++)
                {
                    if (shape[i, j] != 0)
                    {
                        var rectangle = new Rectangle
                        {
                            Width = CellSize,
                            Height = CellSize,
                            Fill = color,
                            Stroke = Brushes.White,
                            StrokeThickness = 0.2
                        };

                        Canvas.SetTop(rectangle, i * CellSize);
                        Canvas.SetLeft(rectangle, j * CellSize);
                        NextBlockCanvas.Children.Add(rectangle);
                    }
                }
            }
        }

        private void GameOver()
        {
            gameTimer.Stop();
            MessageBox.Show("Game over. Try again!");
            ResetGame();
        }

        private void ResetGame()
        {
            //activeBlocks.Clear();
            gameBoard = new GameBoard(this, GameBoardNumOfRows, GameBoardNumOfColumns);
            gameBoard.InitializeUIGameBoard(GameCanvas, CellSize);

            // Generate the first block
            currentBlock = new Block();
            currentBlock.GenerateBlock(gameBoard);
            gameBoard.UpdateUIGameBoard(currentBlock);

            // Restart the timer
            gameTimer.Interval = TimeSpan.FromMilliseconds(normalFallSpeed);
            gameTimer.Start();

            // Reset the score
            score = 0;
            scoreLabel.Text = "Wynik: 0";

            // Reset the level
            gameLvlLabel.Text = "Poziom: 0";
        }
    }
}
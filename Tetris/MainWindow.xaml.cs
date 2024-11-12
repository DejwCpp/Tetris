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
        private DispatcherTimer gameTimer;
        private Block currentBlock;

        private double normalFallSpeed;
        private double fastFallSpeed;
        private bool isFastFalling;

        private DispatcherTimer movementTimer;
        private string movementDirection;

        private bool isMovingLeft = false;
        private bool isMovingRight = false;

        public MainWindow()
        {
            InitializeComponent();

            // Set DataContext for binding
            DataContext = this;

            gameBoard = new GameBoard(GameBoardNumOfRows, GameBoardNumOfColumns);
            GameBoardWidth = gameBoard.Board.GetLength(1) * CellSize;

            gameBoard.InitializeUIGameBoard(GameCanvas, CellSize);

            // Generate the first block
            currentBlock = new Block();
            currentBlock.GenerateBlock(gameBoard);

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
            currentBlock.FallDown(gameBoard);
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
                isMovingRight= false;
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
    }
}
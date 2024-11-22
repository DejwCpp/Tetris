using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{
    /**********************************************
     * CLASS NAME: GameBoard
     * 
     * Varriables:
     * - _mainWindow MainWindow
     * - board int[,]
     * - Board int[,]
     * - cellRectangles Rectangle[,]
     * - random Random
     * - cellColors Brush[,]
     * Methods:
     * - GameBoard(MainWindow mainWindow, int rows, int columns)
     * - SetCell(int row, int column, int value, Brush color = null)
     * - GetCell(int row, int column)
     * - IsWithinBounds(int row, int column)
     * - InitializeUIGameBoard(Canvas GameCanvas, int CellSize)
     * - UpdateUIGameBoard(Block currentBlock)
     * - GetCellColor(int cellValue, int row, int col)
     * - ClearFullRows()
     * - ClearRow(int row)
     * - ShiftRowsDown(int fromRow)
     * - IsRowFull(int row)
     **********************************************/
    public class GameBoard
    {
        private MainWindow _mainWindow;

        private int[,] board;
        public int[,] Board => board;
        private Rectangle[,] cellRectangles;
        private static readonly Random random = new Random();
        private Brush[,] cellColors;

        public GameBoard(MainWindow mainWindow, int rows, int columns)
        {
            _mainWindow = mainWindow;
            // creating one extra wall on left, right and bottom that wont display in UI
            board = new int[rows + 1, columns + 2];
            cellColors = new Brush[rows + 1, columns + 2];
        }


        public void SetCell(int row, int column, int value, Brush color = null)
        {
            if (IsWithinBounds(row, column))
            {
                board[row, column] = value;

                if (value != 0 && color != null)
                {
                    cellColors[row, column] = color;
                }
                else if (value == 0)
                {
                    cellColors[row, column] = Brushes.Black;
                }
            }
        }

        public int GetCell(int row, int column)
        {
            return IsWithinBounds(row, column) ? board[row, column] : -1;
        }

        public bool IsWithinBounds(int row, int column)
        {
            return row >= 0 && row < board.GetLength(0) - 1 &&
                   column >= 1 && column < board.GetLength(1) - 1;
        }

        public void InitializeUIGameBoard(Canvas GameCanvas, int CellSize)
        {
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);

            double strokeThickness = 2; // Set a consistent and visible stroke thickness

            // Create a single border rectangle for the entire game board
            var borderRectangle = new Rectangle
            {
                Width = (cols - 2) * CellSize + strokeThickness, // Add strokeThickness to ensure the right wall is visible
                Height = (rows - 1) * CellSize + strokeThickness, // Add strokeThickness to ensure the bottom wall is visible
                Stroke = Brushes.White,
                StrokeThickness = strokeThickness,
                Fill = Brushes.Transparent // Make the interior transparent
            };

            // Position the border rectangle (account for stroke thickness offset)
            Canvas.SetTop(borderRectangle, 0 - strokeThickness / 2);
            Canvas.SetLeft(borderRectangle, CellSize - strokeThickness / 2);

            GameCanvas.Children.Add(borderRectangle);

            // Initialize cellRectangles without strokes
            cellRectangles = new Rectangle[rows, cols];
            for (int row = 0; row < rows - 1; row++)
            {
                for (int col = 1; col < cols - 1; col++)
                {
                    var rectangle = new Rectangle
                    {
                        Width = CellSize,
                        Height = CellSize,
                        Fill = Brushes.Black
                    };

                    Canvas.SetTop(rectangle, row * CellSize);
                    Canvas.SetLeft(rectangle, col * CellSize);

                    GameCanvas.Children.Add(rectangle);
                    cellRectangles[row, col] = rectangle;
                }
            }
        }




        //public void InitializeUIGameBoard(Canvas GameCanvas, int CellSize)
        //{
        //    cellRectangles = new Rectangle[board.GetLength(0), board.GetLength(1)];

        //    for (int row = 0; row < board.GetLength(0) - 1; row++)
        //    {
        //        for (int col = 1; col < board.GetLength(1) - 1; col++)
        //        {
        //            var rectangle = new Rectangle
        //            {
        //                Width = CellSize,
        //                Height = CellSize,
        //                Fill = Brushes.Black,
        //                Stroke = Brushes.White,
        //                StrokeThickness = 0.2
        //            };

        //            Canvas.SetTop(rectangle, row * CellSize);
        //            Canvas.SetLeft(rectangle, col * CellSize);

        //            GameCanvas.Children.Add(rectangle);
        //            cellRectangles[row, col] = rectangle;
        //        }
        //    }
        //}

        public void UpdateUIGameBoard(Block currentBlock)
        {
            for (int row = 0; row < board.GetLength(0) - 1; row++)
            {
                for (int col = 1; col < board.GetLength(1) - 1; col++)
                {
                    int cellValue = GetCell(row, col);
                    cellRectangles[row, col].Fill = GetCellColor(cellValue, row, col);
                    //cellRectangles[row, col].Stroke = Brushes.White;
                    cellRectangles[row, col].StrokeThickness = 0.2;

                    if (cellValue > 0)
                    {
                        cellRectangles[row, col].Stroke = Brushes.Black;
                        cellRectangles[row, col].StrokeThickness = 1.5;
                    }
                }
            }
        }

        private Brush GetCellColor(int cellValue, int row, int col)
        {
            return cellValue == 0 ? Brushes.Black : cellColors[row, col];
        }

        public void ClearFullRows()
        {
            int countFullRows = 0;
            int scoreBonus = 0;

            for (int row = 0; row < board.GetLength(0) - 1; row++)
            {
                if (IsRowFull(row))
                {
                    ClearRow(row);
                    ShiftRowsDown(row);

                    // Recheck the same row after shifting
                    row--;

                    countFullRows++;
                }
            }
            // Add points for full row
            for (int i = 0; i < countFullRows; i++)
            {
                // 1st row => 10 points
                // 2nd row => 20 points
                // 3rd row => 30 points
                // 4rd row => 40 points
                _mainWindow.AddScore(10 + scoreBonus);
                scoreBonus += 10;
            }
        }

        private void ClearRow(int row)
        {
            for (int col = 1; col < board.GetLength(1) - 1; col++)
            {
                board[row, col] = 0;
                cellColors[row, col] = Brushes.Black;
            }
        }

        private void ShiftRowsDown(int fromRow)
        {
            for (int row = fromRow; row > 0; row--)
            {
                for (int col = 1; col < board.GetLength(1) - 1; col++)
                {
                    board[row, col] = board[row - 1, col];

                    cellColors[row, col] = cellColors[row - 1, col];
                }
            }

            // Clear the topmost row
            for (int col = 1; col < board.GetLength(1) - 1; col++)
            {
                board[0, col] = 0;
                cellColors[0, col] = Brushes.Black;
            }
        }

        public bool IsRowFull(int row)
        {
            for (int col = 1; col < board.GetLength(1) - 1; col++)
            {
                if (board[row, col] == 0) return false;
            }
            return true;
        }
    }
}

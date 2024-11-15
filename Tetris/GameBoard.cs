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
     * - board int[,]
     * Methods:
     * - GameBoard(int rows, int columns)
     * - SetCell(int row, int column, int value)
     * - GetCell(int row, int column)
     * - IsWithinBounds(int row, int column)
     * - DisplayGameBoard()
     * - GenerateNewBlock()
     * - IsRowFull()
     * - IsGameOver()
     **********************************************/
    public class GameBoard
    {
        private int[,] board;
        private Rectangle[,] cellRectangles;
        private static readonly Random random = new Random();
        private Brush[,] cellColors;

        public GameBoard(int rows, int columns)
        {
            // creating one extra wall on left, right and bottom that wont display in UI
            board = new int[rows + 1, columns + 2];
            cellColors = new Brush[rows + 1, columns + 2];
        }

        public int[,] Board => board;

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
            cellRectangles = new Rectangle[board.GetLength(0), board.GetLength(1)];

            for (int row = 0; row < board.GetLength(0) - 1; row++)
            {
                for (int col = 1; col < board.GetLength(1) - 1; col++)
                {
                    var rectangle = new Rectangle
                    {
                        Width = CellSize,
                        Height = CellSize,
                        Fill = Brushes.Black,
                        Stroke = Brushes.White,
                        StrokeThickness = 0.2
                    };

                    Canvas.SetTop(rectangle, row * CellSize);
                    Canvas.SetLeft(rectangle, col * CellSize);

                    GameCanvas.Children.Add(rectangle);
                    cellRectangles[row, col] = rectangle;
                }
            }
        }

        public void UpdateUIGameBoard(Block currentBlock)
        {
            for (int row = 0; row < board.GetLength(0) - 1; row++)
            {
                for (int col = 1; col < board.GetLength(1) - 1; col++)
                {
                    int cellValue = GetCell(row, col);
                    cellRectangles[row, col].Fill = GetCellColor(cellValue, row, col);
                }
            }
        }

        private Brush GetCellColor(int cellValue, int row, int col)
        {
            return cellValue == 0 ? Brushes.Black : cellColors[row, col];
        }

        public void ClearFullRows()
        {
            for (int row = 0; row < board.GetLength(0) - 1; row++)
            {
                if (IsRowFull(row))
                {
                    ClearRow(row);
                    ShiftRowsDown(row);

                    // Recheck the same row after shifting
                    row--;
                }
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

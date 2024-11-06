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

        public GameBoard(int rows, int columns)
        {
            // creating one extra wall on left, right and bottom that wont display in UI
            board = new int[rows + 1, columns + 2];
        }

        public int[,] Board => board;

        public void SetCell(int row, int column, int value)
        {
            if (IsWithinBounds(row, column))
            {
                board[row, column] = value;
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
                        StrokeThickness = 0.5
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
                    cellRectangles[row, col].Fill = GetCellColor(cellValue, currentBlock);
                }
            }
        }

        private Brush GetCellColor(int cellValue, Block currentBlock)
        {
            return cellValue == 0 ? Brushes.Black : currentBlock.BlockColor;
        }

        public void GenerateNewBlock()
        {

        }

        public bool IsRowFull(int row)
        {
            return false;
        }

        public bool IsGameOver()
        {
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class GameBoard
    {
        private int[,] board;

        public GameBoard(int rows, int columns)
        {
            board = new int[rows, columns];
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

        private bool IsWithinBounds(int row, int column)
        {
            return row >= 0 && row < board.GetLength(0) &&
                   column >= 0 && column < board.GetLength(1);
        }

        public void DisplayGameBoard()
        {

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

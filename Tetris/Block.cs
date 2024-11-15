using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tetris
{
    /*****************************
     * CLASS NAME: Block
     * 
     * Varriables:
     * - TypeOfBlock int[,]
     * - BlockColor string
     * - BlockFallSpeed int
     * Methods:
     * - Block()
     * - GenerateBlock()
     * - FallDown()
     * - Movement()
     * - MoveLeft()
     * - MoveRight()
     * - Rotate()
     * - FallFaster()
     * - SetTypeOfBlock()
     * - SetBlockColor()
     * - SetBlockFallSpeed()
     *****************************/
    public class Block
    {
        private List<int[,]> TypeOfBlock;
        public Brush BlockColor { get; private set; }
        public int FallSpeedMs { get; private set; }
        private int currentRow;
        private int currentColumn;
        private int[,] currentShape;
        private static readonly Random random = new Random();

        public Block()
        {
            TypeOfBlock = new List<int[,]>();
            FallSpeedMs = 1000;
            InitializeTypeOfBlock();
        }

        public void GenerateBlock(GameBoard board)
        {
            int startPoint = (board.Board.GetLength(1) / 2) - 2;
            currentShape = TypeOfBlock[random.Next(0, TypeOfBlock.Count)];

            string[] hexColors = { "#fcdb1c", "#ff8400", "#3dc930", "#eb0046", "#b330f0", "#0f6df2" };
            Brush[] colors = hexColors.Select(hex => (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex))).ToArray();
            BlockColor = colors[random.Next(colors.Length)];

            currentRow = 0;
            currentColumn = startPoint;

            PlaceBlockOnBoard(board);
        }

        public void FallDown(GameBoard board)
        {
            ClearBlockFromBoard(board);

            if (CanChangePosition(board, currentShape, 0, 1))
            {
                currentRow++;
            }

            PlaceBlockOnBoard(board);
        }

        public void PlaceBlockOnBoard(GameBoard board)
        {
            for (int i = 0; i < currentShape.GetLength(0); i++)
            {
                for (int j = 0; j < currentShape.GetLength(1); j++)
                {
                    if (currentShape[i, j] == 1)
                    {
                        board.SetCell(currentRow + i, currentColumn + j, 1);
                    }
                }
            }
        }

        public void ClearBlockFromBoard(GameBoard board)
        {
            for (int i = 0; i < currentShape.GetLength(0); i++)
            {
                for (int j = 0; j < currentShape.GetLength(1); j++)
                {
                    if (currentShape[i, j] == 1)
                    {
                        board.SetCell(currentRow + i, currentColumn + j, 0);
                    }
                }
            }
        }

        public void MoveLeft(GameBoard board)
        {
            if (CanChangePosition(board, currentShape, -1, 0))
            {
                currentColumn--;
            }
        }

        public void MoveRight(GameBoard board)
        {
            if (CanChangePosition(board, currentShape, 1, 0))
            {
                currentColumn++;
            }
        }

        public void Rotate(GameBoard board)
        {
            int N = currentShape.GetLength(0);

            int[,] rotatedBlock = new int[N, N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    rotatedBlock[j, N - 1 - i] = currentShape[i, j];
                }
            }

            if(CanChangePosition(board, rotatedBlock, 0, 0))
            {
                currentShape = rotatedBlock;
            }
        }

        public void FallToTheVeryBottom(GameBoard board)
        {
            while (CanChangePosition(board, currentShape, 0, 1))
            {
                currentRow++;
            }
        }

        private bool CanChangePosition(GameBoard board, int[,] block, int horizontal, int vertical)
        {
            for (int i = 0; i < currentShape.GetLength(0); i++)
            {
                for (int j = 0; j < currentShape.GetLength(1); j++)
                {
                    if (block[i, j] == 1)
                    {
                        int newRow = currentRow + i + vertical;
                        int newCol = currentColumn + j + horizontal;

                        if (!board.IsWithinBounds(newRow, newCol) || board.GetCell(newRow, newCol) != 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool CanBlockFall(GameBoard gameBoard)
        {
            ClearBlockFromBoard(gameBoard);
            bool canFall = CanChangePosition(gameBoard, currentShape, 0, 1);
            PlaceBlockOnBoard(gameBoard);
            return canFall;
        }


        /* private methods */

        private void InitializeTypeOfBlock()
        {
            /*
             *  . . . .
             *  # # # #
             *  . . . .
             *  . . . .
             */
            int[,] block1 = {
                { 0, 0, 0, 0 },
                { 1, 1, 1, 1 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 } };

            /*  
             *  # . .
             *  # # #
             *  . . .
             */
            int[,] block2 = {
                { 1, 0, 0 },
                { 1, 1, 1 },
                { 0, 0, 0 } };

            /*  
             *  . . #
             *  # # #
             *  . . .
             */
            int[,] block3 = {
                { 0, 0, 1 },
                { 1, 1, 1 },
                { 0, 0, 0 } };

            /*  
             *  . # # .
             *  . # # .
             *  . . . .
             */
            int[,] block4 = {
                { 1, 1 },
                { 1, 1 } };

            /*  
             *  . # #
             *  # # .
             *  . . .
             */
            int[,] block5 = {
                { 0, 1, 1 },
                { 1, 1, 0 },
                { 0, 0, 0 } };

            /*  
             *  . # .
             *  # # #
             *  . . .
             */
            int[,] block6 = {
                { 0, 1, 0 },
                { 1, 1, 1 },
                { 0, 0, 0 } };

            /*  
             *  # # .
             *  . # #
             *  . . .
             */
            int[,] block7 = {
                { 1, 1, 0 },
                { 0, 1, 1 },
                { 0, 0, 0 } };

            TypeOfBlock.Add(block1);
            TypeOfBlock.Add(block2);
            TypeOfBlock.Add(block3);
            TypeOfBlock.Add(block4);
            TypeOfBlock.Add(block5);
            TypeOfBlock.Add(block6);
            TypeOfBlock.Add(block7);
        }
    }
}

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
        private int BlockFallSpeed;
        private static readonly Random random = new Random();

        public Block()
        {
            TypeOfBlock = new List<int[,]>();
            InitializeTypeOfBlock();
        }

        public void GenerateBlock(GameBoard board)
        {
            int startPoint = (board.Board.GetLength(1) / 2) - 2;
            int[,] spawnBlock = TypeOfBlock[random.Next(0, TypeOfBlock.Count)];

            Brush[] colors = { Brushes.Pink, Brushes.Yellow, Brushes.Green, Brushes.Blue, Brushes.Orange };
            BlockColor = colors[random.Next(colors.Length)];

            for (int i = 0; i < spawnBlock.GetLength(0); i++)
            {
                for (int j = 0; j < spawnBlock.GetLength(1); j++)
                {
                    if (spawnBlock[i, j] == 0) continue;
                    
                    board.SetCell(i, j + startPoint, 1);                    
                }
            }
        }

        public void FallDown()
        {

        }

        public void Movement()
        {

        }

        public void MoveLeft()
        {

        }

        public void MoveRight()
        {

        }

        public void Rotate()
        {

        }

        public void FallFaster()
        {

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

        private void SetBlockColor()
        {

        }

        private void SetBlockFallSpeed()
        {

        }
    }
}

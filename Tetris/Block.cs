using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private string BlockColor;
        private int BlockFallSpeed;

        public Block()
        {
            InitializeTypeOfBlock();
        }

        public void GenerateBlock(int[,] board)
        {
            int startPoint = (board.GetLength(0) / 2) - 1;

            // here start to write code when youre back
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
             *  . . . .
             *  . # # .
             *  . # # .
             *  . . . .
             */
            int[,] block4 = {
                { 0, 0, 0, 0 },
                { 0, 1, 1, 0 },
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 } };

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

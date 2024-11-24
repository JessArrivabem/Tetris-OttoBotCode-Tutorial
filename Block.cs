using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffSet { get; } // where the blocks spawn on the grid
        public abstract int Id { get; }

        private int rotationState;
        private Position offset;

        public Block()
        {
            offset = new Position(StartOffSet.Row, StartOffSet.Column);
        }

        public IEnumerable<Position> TilePositions() // method which returns grey positions occupied by the block on current location and offset
        {
            foreach(Position p in Tiles[rotationState])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        public void RotateCW() // method that rotates the block 90 degrees by increment the  current rotate state until 0 on final state
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        public void RotateCCW()
        {
            if (rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }

        public void Move(int rows, int columns) // movement method which moves the block by a given number by rows and columns
        { 
            offset.Row += rows;
            offset.Column += columns;
        }

        public void Reset ()  // reset method which resets the rotation and position  
        {
            rotationState = 0;
            offset.Row = StartOffSet.Row;
            offset.Column = StartOffSet.Column;
        }

    }
}

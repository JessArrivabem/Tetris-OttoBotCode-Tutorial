using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class GameGrid
    {
        private readonly int[,] grid; // two dimensational rectangular array - first is the row and second is the column
        public int Rows { get; }
        public int Columns { get; }

        public int this[int r, int c] // index to access the array
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }

        public GameGrid(int rows, int columns) // constructor
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns]; // initialize the array
        }

        public bool IsInside(int r, int c) // method that checks if a given row and column is inside the grid or not
        {
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
        }

        public bool IsEmpty(int r, int c) // method that checks if a cell is empty or not
        {
            return IsInside(r, c) && grid[r, c] == 0;
        }

        public bool IsRowFull(int r) // method that checks if a row is full
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsRowEmpty(int r) // method that checks if a row is empty
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] != 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void ClearRow(int r) // method that in case the row is full it will be cleared
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r, c] = 0;
            }
        }

        private void MoveRowDown(int r, int numRows) // so after cleared, it will be moved down
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r + numRows, c] = grid[r, c];
                grid[r, c] = 0;
            }
        }

        public int ClearFullRow()
        {
            int cleared = 0;

            for (int r = Rows - 1; r >= 0; r--) // checks if the current row is full
            {
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                else if (cleared > 0) // we move it down by the number of cleared rows
                {
                    MoveRowDown(r, cleared);
                }
            }

            return cleared;
        }
    }
}

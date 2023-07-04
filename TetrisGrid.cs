using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_C_Sharp_Game
{
    public class TetrisGrid // Holds the 2d array
    {
        private readonly int[,] grid;
        public int Rows { get; }
        public int Columns { get; }

        public int this[int a, int b]
        {
            get => grid[a, b];
            set => grid[a, b] = value;
        }

        public TetrisGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }

        public bool Isinside(int a, int b)
        {
            return a >= 0 && a < Rows && b >= 0 && b < Columns;
        }

        public bool Isempty(int a, int b)
        {
            return Isinside(a, b) && grid[a, b] == 0;
        }

        public bool Isrowfull(int a)
        {
            for (int b = 0; b < Columns; b++)
            {
                if (grid[a, b] == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Isrowempty(int a)
        {
            for (int b = 0; b < Columns; b++)
            {
                if (grid[a, b] != 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void Clearrow(int a)
        {
            for (int b = 0; b < Columns; b++)
            {
                grid[a, b] = 0;
            }
        }

        private void Moverowdown(int a, int numofrows)
        {
            for (int b = 0; b < Columns; b++)
            {
                grid[a + numofrows, b] = grid[a, b];
                grid[a, b] = 0;
            }
        }

        public int Clearfullrows()
        {
            int clearedrow = 0;

            for (int r = Rows - 1; r >= 0; r--)
            {
                if (Isrowfull(r))
                {
                    Clearrow(r);
                    clearedrow++;
                }
                else if (clearedrow > 0)
                {
                    Moverowdown(r, clearedrow);
                }
            }

            return clearedrow;
        }
    }
}

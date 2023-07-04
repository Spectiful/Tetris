using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_C_Sharp_Game
{
    public abstract class Block
    {
        protected abstract Position[][] tiles1 { get; }
        protected abstract Position Startoffset { get; }
        public abstract int id { get; }

        private int rotatestate;
        private Position offset;

        public Block()
        {
            offset = new Position(Startoffset.Row, Startoffset.Column);
        }

        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in tiles1[rotatestate])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }
        //rotates block clockwise
        public void Rotateclockwise()
        {
            rotatestate = (rotatestate + 1) % tiles1.Length;
        }
        //rotates block counter clockwise
        public void Rotatecounterclockwise()
        {
            if (rotatestate == 0)
            {
                rotatestate = tiles1.Length - 1;
            }
            else
            {
                rotatestate--;
            }
        }
        //moves the block on the grid
        public void Move(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }
        //restarts the blocks position
        public void Reset()
        {
            rotatestate = 0;
            offset.Row = Startoffset.Row;
            offset.Column = Startoffset.Column;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_C_Sharp_Game
{
    public class OBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(0,0), new(0,1), new(1,0), new(1,1) }
        };

        public override int id => 4;
        protected override Position Startoffset => new Position(0, 4);
        protected override Position[][] tiles1 => tiles;
    }
}

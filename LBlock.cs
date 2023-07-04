using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_C_Sharp_Game
{
    public class LBlock : Block
    {
        protected override Position[][] tiles1 => new Position[][] {
            new Position[] {new(0,2), new(1,0), new(1,1), new(1,2)},
            new Position[] {new(0,1), new(1,1), new(2,1), new(2,2)},
            new Position[] {new(1,0), new(1,1), new(1,2), new(2,0)},
            new Position[] {new(0,0), new(0,1), new(1,1), new(2,1)}
        };

        public override int id => 3;

        protected override Position Startoffset => new(0, 3);
    }
}

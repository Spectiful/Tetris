using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_C_Sharp_Game
{
    public class BlockQue
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new JBlock(),
            new LBlock(),
            new ZBlock(),
        };

        private readonly Random random = new Random();

        //shows a preview of the next block
        public Block Nextblock {  get; private set; }

        //A random block will be the next block on the que
        public BlockQue()
        {
            Nextblock = Randomblock();
        }

        //Gives a random block to player
        private Block Randomblock()
        {
            return blocks[random.Next(blocks.Length)];
        }

        //Updates the next block with 
        public Block Getandupdate()
        {
            Block block = Nextblock;

            do
            {
                Nextblock = Randomblock();
            }
            while (block.id == Nextblock.id);
            
            return block;
        }
    }
}

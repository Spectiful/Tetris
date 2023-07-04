using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_C_Sharp_Game
{
    public class Gamestate
    {
        private Block Currentblock1;

        //puts blocks in the correct position and the next block as well
        public Block Currentblock2
        {
            get => Currentblock1;
            private set
            {
                Currentblock1 = value;
                Currentblock1.Reset();

                for (int i = 0; i < 2; i++) 
                {
                    Currentblock1.Move(1, 0);

                    if (!Blockfits())
                    {
                        Currentblock1.Move(-1, 0);
                    }
                }
            }

        }

        public TetrisGrid TetrisGrid { get; }
        public BlockQue blockQue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        public Block Heldblock { get; private set; }
        public bool Canhold { get; private set; }

        //Grid for the tetris game along with getting a random block
        public Gamestate()
        {
            TetrisGrid = new TetrisGrid(22, 10);
            blockQue = new BlockQue();
            Currentblock2 = blockQue.Getandupdate();
            Canhold = true;
        }

        private bool Blockfits()
        {
            foreach (Position p in Currentblock2.TilePositions())
            {
                if (!TetrisGrid.Isempty(p.Row, p.Column))
                {
                    return false;
                }
            }
            return true;
        }

        public void Holdblock()
        {
            if (!Canhold)
            {
                return;
            }

            if (Heldblock == null)
            {
                Heldblock = Currentblock2;
                Currentblock2 = blockQue.Getandupdate();
            }

            else
            {
                Block temp = Currentblock2;
                Currentblock2 = Heldblock;
                Heldblock = temp;
            }

            Canhold = false;
        }
        public void Rotateblockclockwise()
        {
            Currentblock2.Rotateclockwise();

            if (!Blockfits())
            {
                Currentblock2.Rotatecounterclockwise();
            }
        }

        public void Rotateblockcounterclockwise()
        {
            Currentblock2.Rotatecounterclockwise();

            if (!Blockfits())
            {
                Currentblock2.Rotateclockwise();
            }
        }

        public void Moveblockleft()
        {
            Currentblock2.Move(0, -1);
            if (!Blockfits())
            {
                Currentblock2.Move(0, 1);
            }
        }

        public void Moveblockright()
        {
            Currentblock2.Move(0, 1);
            if (!Blockfits())
            {
                Currentblock2.Move(0, -1);
            }
        }

        //Ends the tetris game
        private bool Isgameover()
        {
            return !(TetrisGrid.Isrowempty(0) && TetrisGrid.Isrowempty(1));
        }

        //Places the tetris block on the grid
        private void Placeblock()
        {
            foreach (Position p in Currentblock2.TilePositions())
            {
                TetrisGrid[p.Row, p.Column] = Currentblock2.id;
            }

            Score += TetrisGrid.Clearfullrows();

            if (Isgameover())
            {
                GameOver = true;
            }
            else
            {
                Currentblock2 = blockQue.Getandupdate(); //gets a new block if game is not over
                Canhold = true; 
            }
        }

        public void Moveblockdown()
        {
            Currentblock2.Move(1, 0);

            if (!Blockfits())
            {
                Currentblock2.Move(-1, 0);
                Placeblock();
            }
        }

        private int Tiledropdistance(Position position)
        {
            int drop = 0;

            while (TetrisGrid.Isempty(position.Row + drop + 1, position.Column))
            {
                drop++;
            }

            return drop;
        }

        public int Blockdropdistance()
        {
            int drop = TetrisGrid.Rows;

            foreach (Position p in Currentblock2.TilePositions())
            {
                drop = System.Math.Min(drop, Tiledropdistance(p));
            }

            return drop;
        }

        public void Dropblock()
        {
            Currentblock2.Move(Blockdropdistance(), 0);
            Placeblock();
        }
    }
}

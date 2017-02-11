using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeProjectGame
{
    class BlockBody
    {
        public enum EnumTetris : byte { T = 1, G, I, L, E }
        Random rand = new Random();
        public EnumTetris NewBody(ref List<BlockParts> z)
        {
            EnumTetris entetr = (EnumTetris)(rand.Next(1, 6));
            switch (entetr)
            {
                case EnumTetris.T:
                    z = new List<BlockParts>()
                    {
                        new BlockParts(38, 2),
                        new BlockParts(39, 2),
                        new BlockParts(39, 1),
                        new BlockParts(40, 2)
                    };
                    break;
                case EnumTetris.L:
                    z = new List<BlockParts>()
                    {
                        new BlockParts(38, 2),
                        new BlockParts(39, 2),
                        new BlockParts(40, 2),
                        new BlockParts(40, 1)
                    };
                    break;
                case EnumTetris.I:
                    z = new List<BlockParts>()
                    {
                        new BlockParts(38, 1),
                        new BlockParts(39, 1),
                        new BlockParts(40, 1),
                        new BlockParts(41, 1)
                    };
                    break;
                case EnumTetris.E:
                    z = new List<BlockParts>()
                    {
                        new BlockParts(39, 1),
                        new BlockParts(40, 2),
                        new BlockParts(39, 2),
                        new BlockParts(40, 1)
                    };
                    break;
                case EnumTetris.G:
                    z = new List<BlockParts>()
                    {
                        new BlockParts(38, 2),
                        new BlockParts(39, 2),
                        new BlockParts(39, 1),
                        new BlockParts(40, 1)
                    };
                    break;
            }
            return entetr;
        }

        public static bool IsRightStop(List<BlockParts> bl)
        {
            bl[bl.Count - 1].X++;
            if (bl[bl.Count - 1].isLeftRightStopBlock)
                return true;
            bl[bl.Count - 1].X--;
            foreach (BlockParts b in bl)
                b.X++;

            return false;
        }

        public static bool IsLeftStop(List<BlockParts> bl)
        {
            bl[0].X--;
            if (bl[0].isLeftRightStopBlock)
                return true;
            bl[0].X++;
            foreach (BlockParts b in bl)
            {
                //b.Y--;
                b.X--;
            }

            return false;
        }

        public static bool IsDownStop(List<BlockParts> bl)
        {
            bl[1].Y++;
            if (bl[1].isStopBlock)
                return true;
            bl[1].Y--;
            //foreach (BlockParts b in bl)
            //    b.Y++;

            return false;
        }

        public static void MoveRecoil(List<BlockParts> bl, int a)
        {
            if (a >= 0)
                foreach (BlockParts b in bl)
                {
                    b.X++;
                }
            else if (a < 0)
                foreach (BlockParts b in bl)
                {
                    b.X--;
                }
        }
    }
}

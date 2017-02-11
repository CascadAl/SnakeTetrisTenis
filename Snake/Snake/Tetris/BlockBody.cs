using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeProjectGame
{
    class BlockBody
    {
        public static byte BlockColor { get; set; }
        public enum EnumTetris : byte { T = 1, G, I, L, E }
        Random rand = new Random();
        public EnumTetris NewBody(ref List<BlockParts> blockBody)
        {
            EnumTetris entetr = (EnumTetris)(rand.Next(1, 6));
            switch (entetr)
            {
                case EnumTetris.T:
                    BlockColor = 7;
                    blockBody = new List<BlockParts>()
                    {
                        new BlockParts(38, 2),
                        new BlockParts(39, 2),
                        new BlockParts(39, 1),
                        new BlockParts(40, 2),
                    };
                    break;
                case EnumTetris.L:
                    BlockColor = 4;
                    blockBody = new List<BlockParts>()
                    {
                        new BlockParts(38, 2),
                        new BlockParts(39, 2),
                        new BlockParts(40, 2),
                        new BlockParts(40, 1)
                    };
                    break;
                case EnumTetris.I:
                    BlockColor = 5;
                    blockBody = new List<BlockParts>()
                    {
                        new BlockParts(38, 1),
                        new BlockParts(39, 1),
                        new BlockParts(40, 1),
                        new BlockParts(41, 1)
                    };
                    break;
                case EnumTetris.E:
                    BlockColor = 1;
                    blockBody = new List<BlockParts>()
                    {
                        new BlockParts(39, 1),
                        new BlockParts(40, 2),
                        new BlockParts(39, 2),
                        new BlockParts(40, 1)
                    };
                    break;
                case EnumTetris.G:
                    BlockColor = 11;
                    blockBody = new List<BlockParts>()
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

        public static bool IsRightStop(List<BlockParts> blockParts)
        {
            blockParts[blockParts.Count - 1].X++;
            if (blockParts[blockParts.Count - 1].isLeftRightStopBlock)
                return true;
            blockParts[blockParts.Count - 1].X--;
            foreach (BlockParts b in blockParts)
                b.X++;

            return false;
        }

        public static bool IsLeftStop(List<BlockParts> blockParts)
        {
            blockParts[0].X--;
            if (blockParts[0].isLeftRightStopBlock)
                return true;
            blockParts[0].X++;
            foreach (BlockParts b in blockParts)
            {
                //b.Y--;
                b.X--;
            }

            return false;
        }

        public static bool IsDownStop(List<BlockParts> blockParts)
        {
            blockParts[1].Y++;
            if (blockParts[1].isStopBlock)
                return true;
            blockParts[1].Y--;
            //foreach (BlockParts b in bl)
            //    b.Y++;

            return false;
        }

        public static void MoveRecoil(List<BlockParts> blockParts, int leftRight)
        {
            if (leftRight >= 0)
                foreach (BlockParts b in blockParts)
                {
                    b.X++;
                }
            else if (leftRight < 0)
                foreach (BlockParts b in blockParts)
                {
                    b.X--;
                }
        }
    }
}

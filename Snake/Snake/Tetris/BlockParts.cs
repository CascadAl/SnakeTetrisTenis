using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeProjectGame
{
    class BlockParts
    {
        public bool isStopBlock { get; private set; }
        public bool isLeftRightStopBlock { get; private set; }

        int x, y;
        char body;

        public char Body
        {
            get { return body; }
            set
            {
                if (value == '#' || value == ' ')
                    body = value;
            }
        }

        public int X
        {
            get { return x; }
            set
            {
                if (value > (EnvironmentBase.width - 30) / 2 && value <= (EnvironmentBase.width + 30) / 2)
                {
                    isLeftRightStopBlock = false;

                    x = value;
                }
                else
                    isLeftRightStopBlock = true;
            }
        }
        public int Y
        {
            get { return y; }
            set
            {
                if (value > 0 && value < EnvironmentBase.height)
                {
                    isStopBlock = false;

                    y = value;
                }
                else
                    isStopBlock = true;
            }
        }

        public BlockParts(int a, int b)
        {
            X = a;
            Y = b;
            body = '#';
        }

        //public void YDown(BlockParts bl)
        //{
        //    bl.Y++;
        //}
        //public void XLeft(BlockParts bl)
        //{
        //    bl.X--;
        //}
        //public void XRight(BlockParts bl)
        //{
        //    bl.X++;
        //}
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeProjectGame
{
    struct BodyParts
    {
        int x, y;
        char body;

        public char Body
        {
            get { return body; }
            set
            {
                if (value == '*' || value == ' ')
                    body = value;
            }
        }

        public int X
        {
            get { return x; }
            set
            {
                if(value > 0 && value < EnvironmentBase.width)
                    x = value;
                else
                    Snake.isAlive = false;
            }
        }
        public int Y
        {
            get { return y; }
            set
            {
                if (value > 0 && value < EnvironmentBase.height)
                    y = value;
                else
                    Snake.isAlive = false;
            }
        }

        public BodyParts(int xCoord, int yCoord) :this()
        {
            X = xCoord;
            Y = yCoord;
            body = '*';
        }
    }
}

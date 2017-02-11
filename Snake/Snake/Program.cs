using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeProjectGame
{
    class Program
    {
        static void Main(string[] args)
        {
            byte a = 1;
            EnvironmentBase env = new EnvironmentBase();
            while (true)
            {
                //a = EnvironmentBase.Begin(a);
                //if (a == 1)
                //{ Snake snake = new Snake(); snake.FirstPlace(); }
                //else if (a == 2)
                { Tetris tetris = new Tetris(); tetris.MoveMethod(); }
                //else if (a == 3)
                //{ Console.WriteLine("Tenis"); Thread.Sleep(5000); }
                //else if (a == 0)
                //{ return; }
                //a = 0;
            }
        }
    }
}

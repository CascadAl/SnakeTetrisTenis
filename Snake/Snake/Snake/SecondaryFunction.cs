using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeProjectGame
{
    static class SecondaryFunction
    {
        ///<summary>
        ///There are use classes for Pause, Quit, Cheats
        /// </summary>
        static ConsoleKeyInfo k = new ConsoleKeyInfo();
        public static bool IsAlive { get; private set; }

        static SecondaryFunction()
        {
            IsAlive = true;
        }

        public static void Begin()
        {
            IsAlive = true;
        }

        ///Quit
        public static void QKey()
        {
            IsAlive = false;
        }
        ///Pause
        public static void PKey()
        {
            EnvironmentBase.Pause(true);
            while(k.Key != ConsoleKey.P)
            {
                k = Console.ReadKey(true);
                if (k.Key == ConsoleKey.Tab)
                    TabKey();
            }

            EnvironmentBase.Pause(false);
            k = new ConsoleKeyInfo();
        }
        ///Tab
        public static void TabKey()
        {
            CheatsTetris cheatClass = new CheatsTetris();

            cheatClass.Cheat();
            
            EnvironmentTetris.Frame();
        }


    }
}

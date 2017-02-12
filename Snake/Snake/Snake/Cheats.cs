using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeProjectGame
{
    class CheatsTetris
    {
        public static bool IsCheating { get; private set; }

        public static List<string> cheatList = new List<string>();
        StringBuilder cheatsText;
        public bool chetsNewLife = false;
        ConsoleKeyInfo k = new ConsoleKeyInfo();

        //Cheat Text
        Type sn = typeof(Snake);
        Type st = typeof(string);
        Type it = typeof(int);


        public void Cheat()
        {
            while (k.Key != ConsoleKey.Tab)
            {
                if ((cheatList.Count) > 5)
                    cheatList.RemoveAt(0);

                EnvironmentTetris.Cheats(true);

                cheatsText = new StringBuilder();

                while ((k = Console.ReadKey(true)).Key != ConsoleKey.Tab && k.Key != ConsoleKey.Enter)
                {
                    if (k.Key == ConsoleKey.Backspace && cheatsText.Length > 0)
                    {
                        cheatsText.Remove(cheatsText.Length - 1, 1);
                    }
                    else if (k.Key != ConsoleKey.Backspace && cheatsText.Length < 58)
                    {
                        cheatsText.Append(Convert.ToChar(k.KeyChar));
                    }
                    EnvironmentBase.Cheats(cheatsText);
                }

                if (k.Key == ConsoleKey.Tab)
                {
                    EnvironmentTetris.Cheats(false);
                    return;
                }

                string cheat = Convert.ToString(cheatsText);

                if (cheat == "new")
                {
                    IsCheating = true;
                    cheatList.Add(sn.FullName + "%" + st.FullName + "&" + cheat.GetHashCode() + "^" + cheat);
                }
                else if (cheat == "nonew")
                {
                    IsCheating = false;
                    cheatList.Add(sn.FullName + "%" + st.FullName + "&" + cheat.GetHashCode() + "^" + cheat);
                }
                else
                    try
                    {
                        int sp = Convert.ToInt32(cheat);
                        Tetris.Speed = sp;
                        cheatList.Add(sn.FullName + "%" + it.FullName + "&" + cheat.GetHashCode() + "^" + cheat);
                    }
                    catch (Exception e)
                    {
                        cheatList.Add("Invalid input$" + e.Message + "^");
                    }
            }
        }
    }
}

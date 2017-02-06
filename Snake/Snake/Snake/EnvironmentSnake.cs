using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeProjectGame
{
    //Базовая среда
    class EnvironmentBase
    {
        static ConsoleKeyInfo key = new ConsoleKeyInfo();

        enum enumFirstBegin : byte { Go = 0, Game = 1, Mode = 2, Quit = 3 }
        enum enumGameBegin : byte { None = 0, Snake = 1, Tetris = 2, Tenis = 3 }
        public enum enumModeBegin : byte { Easy = 200, Normal = 75, Hard = 20 }
        static string[] helpBegin = new string[4] { "You can play", "Select game", "Select level", "Quit the game" };
        static string[] helpGame = new string[3] { "Select the game Snake", "Select the game Tetris", "Select the game Tenis" };
        static string[] helpMode = new string[3] { "Choose the easy level", "Choose the normal level", "Choose the hard level" };

        static enumFirstBegin enFirstBeg;
        static enumGameBegin enGameBeg;
        public static enumModeBegin enModeBeg;

        public static byte width;
        public static byte height;
        
        public bool isGo = false, isGame = false, isMode = false, isQuit = false, isSnake = false,
            isTetris = false, isTenis = false, isEasy = false, isNormal = false, isHard = false;

        static string[] fir1 = new string[4] { "G O", "G a m e", "M o d e", "Q u i t" };
        static string[] fir_1 = new string[4] { "G   O", "G  a  m  e", "M  o  d  e", "Q  u  i  t" };
        static string[] game1 = new string[3] { "S n a k e", "T e t r i s", "T e n i s"};
        static string[] game_1 = new string[3] { "S  n  a  k  e", "T  e  t  r  i  s", "T  e  n  i  s" };
        static string[] mode1 = new string[3] { "E a s y", "N o r m a l", "H a r d" };
        static string[] mode_1 = new string[3] { "E  a  s  y", "N  o  r  m  a  l", "H  a  r  d" };

        //Конструктор
        static EnvironmentBase()
        {
            Console.Title = "Snake. Game by Cascad";
            Console.SetWindowSize(80, 31);

            Console.CursorVisible = false; // гасим курсор

            width = 79;
            height = 29;
        }

        //Начальная страница
        public static byte Begin(byte a)
        {
            do
            {
                key = new ConsoleKeyInfo();
                while (key.Key != ConsoleKey.Enter)
                {
                    Console.Clear();
                    for (byte i = 0; i < fir1.Length; i++)
                    {
                        if (enGameBeg == 0 & i == 0)
                            Color(0);
                        else
                            Color(9);
                        Console.SetCursorPosition((81 - fir1[i].Length) / 2, 9 + (i * 3));
                        if (i == a)
                        {
                            Color(4);
                            Console.SetCursorPosition((81 - fir_1[i].Length) / 2, 9 + (i * 3));

                            Console.Write(fir_1[a]);
                            continue;
                        }
                        Console.Write(fir1[i]);
                    }

                    Color(2);
                    Console.SetCursorPosition(0, 30);
                    Console.Write(helpBegin[a]);

                    key = Console.ReadKey(false);

                    if (key.Key == ConsoleKey.DownArrow)
                    {
                        if (a < fir1.Length - 1)
                            checked
                            {
                                a++;
                            }
                    }
                    else if (key.Key == ConsoleKey.UpArrow)
                    {
                        if (a > 0 && (enGameBeg != 0 || (a - 1) != 0))
                            checked
                            {
                                a--;
                            }
                    }
                }
                enFirstBeg = (enumFirstBegin)a;

                if (a == 1)
                {
                    JumpBegin(a, game1, game_1, helpGame[0]);
                    enGameBeg = (enumGameBegin)BeginGameOrMode(a, game1, game_1, helpGame) + 1;
                    JumpEnd(a, game1, game_1, helpGame[(int)enGameBeg - 1], (int)enGameBeg - 1);
                }
                else if (a == 2)
                {
                    JumpBegin(a, mode1, mode_1, helpMode[0]);
                    enModeBeg = (enumModeBegin)BeginGameOrMode(a, mode1, mode_1, helpMode);
                    JumpEnd(a, mode1, mode_1, helpMode[(int)enModeBeg], (int)enModeBeg);
                }
            } while ((a != 0 || enGameBeg == 0) && a != 3);
            Console.Clear();
            if (a == 3)
                return 0;
            return (byte)enGameBeg;
        }

        static void JumpBegin(byte z, string[] arr1, string[] arr_1, string help)
        {
            for (int i = 2; i < 31; i += 2)
            {
                Console.Clear();
                for (int j = 0; j < fir1.Length; j++)
                {
                    if (j < arr1.Length)
                    {
                        if (j == 0)
                        {
                            if (i > 19)
                                Color(4);
                            else
                                Color(8);

                            Console.SetCursorPosition(81 - (i + 15), 9 + (z * 3));

                            Console.Write(arr_1[j]);
                        }
                        else
                        {
                            if (i > 19)
                                Color(9);
                            else
                                Color(0);
                            Console.SetCursorPosition(81 - (i + 15), 9 + (z * 3) + j * 2);

                            Console.Write(arr1[j]);
                        }
                    }
                    if(i < 20 && (enGameBeg != 0 || j != 0))
                        Color(9);
                    else
                        Color(0);
                    Console.SetCursorPosition(((81 - fir1[j].Length) / 2) - i, 9 + (j * 3));
                    if (z == j)
                    {
                        if (i < 20)
                            Color(4);
                        else
                            Color(8);
                        Console.SetCursorPosition(((81 - fir_1[j].Length) / 2) - (i + i) / 3, 9 + (j * 3));

                        Console.Write(fir_1[j]);
                        continue;
                    }
                    Console.Write(fir1[j]);

                }
                Color(2);
                Console.SetCursorPosition(0, 30);
                if (i < 20)
                    Console.Write(helpBegin[z]);
                else
                    Console.Write(help);

                Thread.Sleep(8);
            }
        }

        static void JumpEnd(byte z, string[] arr1, string[] arr_1, string help, int l)
        {
            for (int i = 30; i >= 0; i -= 2)
            {
                Console.Clear();
                for (int j = 0; j < fir1.Length; j++)
                {
                    if (j < arr1.Length)
                    {
                        if (j == l)
                        {
                            if (i > 19)
                                Color(4);
                            else
                                Color(8);
                            Console.SetCursorPosition(81 - (i + 17), 9 + (z * 3) + j * 2);

                            Console.Write(arr_1[j]);
                        }
                        else
                        {
                            if (i > 19)
                                Color(9);
                            else
                                Color(0);
                            Console.SetCursorPosition(81 - (i + 15), 9 + (z * 3) + j * 2);

                            Console.Write(arr1[j]);
                        }
                    }

                    if(i < 20 && (enGameBeg != 0 || j != 0))
                        Color(9);
                    else
                        Color(0);
                    Console.SetCursorPosition(((81 - fir1[j].Length) / 2) - i, 9 + (j * 3));
                    if (z == j)
                    {
                        if (i < 20)
                            Color(4);
                        else
                            Color(8);
                        Console.SetCursorPosition(((81 - fir_1[j].Length) / 2) - (i + i) / 3, 9 + (j * 3));

                        Console.Write(fir_1[j]);
                        continue;
                    }
                    Console.Write(fir1[j]);
                }
                Color(2);
                Console.SetCursorPosition(0, 30);
                if (i < 20)
                    Console.Write(helpBegin[z]);
                else
                    Console.Write(help);
                Thread.Sleep(8);
            }
        }


        static int BeginGameOrMode(byte u, string[] arr1, string[] arr_1, string[] help)
        {
            for(byte i = 0; i < arr1.Length; i++)
            {
                Console.SetCursorPosition((81 - arr_1[i].Length) / 2, 9 + (u * 3) + (i * 2));

                Console.Write("                ");
            }
            byte b = 0;
            do
            {
                //Console.Clear();
                for (byte i = 0; i < arr1.Length; i++)
                {
                    Color(9);
                    Console.SetCursorPosition((81 - arr1[i].Length) / 2, 9 + (u * 3) + (i * 2));
                    if (i == b)
                    {
                        Color(4);

                        Console.SetCursorPosition((81 - arr_1[i].Length) / 2, 9 + (u * 3) + (i * 2));

                        Console.Write(arr_1[b]);
                        continue;
                    }
                    Console.Write(arr1[i]);
                }
                Color(2);
                Console.SetCursorPosition(0, 30);
                Console.Write(help[b]);

                key = Console.ReadKey(false);

                if (key.Key == ConsoleKey.DownArrow)
                {
                    if (b < arr1.Length - 1)
                        checked
                        {
                            b++;
                        }
                    Console.SetCursorPosition((81 - arr_1[b].Length) / 2, 9 + (u * 3) + (b * 2));

                    Console.Write("                ");
                    Console.SetCursorPosition((81 - arr_1[b - 1].Length) / 2, 9 + (u * 3) + ((b - 1) * 2));

                    Console.Write("                ");
                    Console.SetCursorPosition(0, 30);
                    Console.Write("                         ");
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    if (b > 0)
                        checked
                        {
                            b--;
                        }
                    Console.SetCursorPosition((81 - arr_1[b].Length) / 2, 9 + (u * 3) + (b * 2));

                    Console.Write("                ");
                    Console.SetCursorPosition((81 - arr_1[b + 1].Length) / 2, 9 + (u * 3) + ((b + 1) * 2));

                    Console.Write("                ");
                    Console.SetCursorPosition(0, 30);
                    Console.Write("                         ");
                }
            } while (key.Key != ConsoleKey.Enter);
            return b;
        }

        //Конец
        public static void End()
        {
            Color(3);
            Console.SetCursorPosition(32, 10);
            Console.WriteLine("G a m e   O v e r");

            Console.SetCursorPosition(33, 13);
            Console.WriteLine("Y o u   L o s e");

            Console.SetCursorPosition(30, 16);
            Console.WriteLine("P r e s s   E N T E R");
            Color(0);
        }

        //Цвет
        public static void Color(byte a)
        {
            if (a == 1)
                Console.ForegroundColor = ConsoleColor.Cyan;
            else if (a == 2)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else if (a == 3)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (a == 4)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (a == 5)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (a == 6)
                Console.ForegroundColor = ConsoleColor.Black;
            else if (a == 7)
                Console.ForegroundColor = ConsoleColor.Magenta;
            else if (a == 8)
                Console.ForegroundColor = ConsoleColor.White;
            else if (a == 9)
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            else if (a == 10)
                Console.ForegroundColor = ConsoleColor.Blue;
            else
                Console.ForegroundColor = ConsoleColor.Gray;
        }

        //Пауза
        public static void Pause(bool p)
        {
            if (p)
                Color(5);
            else
                Color(6);
            Console.SetCursorPosition(33, 13);
            Console.Write("||===========||");

            Console.SetCursorPosition(33, 14);
            Console.Write("|| P A U S E ||");

            Console.SetCursorPosition(33, 15);
            Console.Write("||===========|| ");

            Color(0);
        }

        public static void Cheats(StringBuilder cheat)
        {
            Console.SetCursorPosition(10, 27);
            Console.Write("                                                          ");
            Console.SetCursorPosition(10, 27);
            Console.Write(cheat);
        }

        //Cheats
        public static void Cheats(bool f)
        {
            bool second = false;
            if (f)
            {
                Color(7);
                Console.CursorVisible = true;
            }
            else
            {
                second = true;
                Color(6);
                Console.CursorVisible = false;
            }
            if (!second || !f)
            {
                Console.SetCursorPosition(9, 20);
                Console.Write(" ___________________________________________________________");

                for (int i = 0; i < 5; i++)
                {
                    Console.SetCursorPosition(9, 21 + i);
                    Console.Write("|                                                           |");
                }
                Console.SetCursorPosition(9, 26);
                Console.Write("|-----------------------------------------------------------|");

                Console.SetCursorPosition(9, 27);
                Console.Write("|                                                           |");

                Console.SetCursorPosition(9, 28);
                Console.Write("|-----------------------------------------------------------| ");
            }

            Console.SetCursorPosition(10, 27);
            Color(8);
            if (Snake.cheatList.Count > 0)
            {
                Color(6);
                for (int i = 0; i < Snake.cheatList.Count - 1; i++)
                {
                    Console.SetCursorPosition(10, 26 - (Snake.cheatList.Count - i - 1));
                    Console.Write(Snake.cheatList[i]);
                }
                if (f)
                    Color(2);
                else
                    Color(6);

                for (int i = 0; i < Snake.cheatList.Count; i++)
                {
                    Console.SetCursorPosition(10, 26 - (Snake.cheatList.Count - i));
                    Console.Write(Snake.cheatList[i]);
                }
                Color(8);
                Console.SetCursorPosition(10, 27);
            }
            if (!f)
                Color(0);
        }
    }

    //Среда змеи
    class EnvironmentSnake : EnvironmentBase
    {
        //Рамка
        public static void Frame()
        {
            Color(0);
            for (byte j = 0; j < height + 1; j++)
            {
                if (j == 0 || j == height)
                {
                    for (byte i = 0; i < width + 1; i++)
                        Console.Write('#');
                }
                else
                {
                    Console.Write('#');
                    Console.Write("{0," + width + "}", '#');
                }
            }
            Color(1);
            Console.Write("Score:"); Color(2); Console.Write(" {0}", Food.Score); Color(1);
            Console.Write("        P"); Color(2); Console.Write(" - Pause"); Color(1);
            Console.Write("        Q"); Color(2); Console.Write(" - Quit"); Color(0);
        }
    }
}

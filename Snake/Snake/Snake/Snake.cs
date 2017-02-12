//#define Teting
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SnakeProjectGame
{
    class Snake
    {
        #region InitialData
        List<BodyParts> snakeBody = new List<BodyParts>
        {
            new BodyParts(15, 15),
            new BodyParts(16, 15),
            new BodyParts(17, 15)
        };
        public static List<string> cheatList = new List<string>();
        public static bool isAlive = false, isPause = false, chetsNewLife = false, chetsSpeed = false;
        static ConsoleKeyInfo k;
        static ConsoleKeyInfo kOld;
        BodyParts old;
        StringBuilder sb;
        byte snakeColor;
        byte foodColor = 3;
        Random rand = new Random();

        //StringBuilder sb;
        Thread thread;

        Type sn = typeof(Snake);
        Type st = typeof(string);
        Type it = typeof(int);

        Food food = new Food();

        static int speed = 75;

        //Свойство скорости
        public static int Speed
        {
            get { return speed; }
            set
            {
                if (value > 19 && value < 201)
                    speed = value;
            }
        }
        #endregion

        #region Reset
        //Обновить данные
        void Reset()
        {
            snakeColor = 0;
            snakeBody = new List<BodyParts>
        {
            new BodyParts(15, 15),
            new BodyParts(16, 15),
            new BodyParts(17, 15)
        };
            isAlive = true;
            
            food = new Food();

            if((int)EnvironmentBase.enModeBeg == 0)
                Speed = (int)EnvironmentBase.enumModeBegin.Easy;
            else if ((int)EnvironmentBase.enModeBeg == 1)
                Speed = (int)EnvironmentBase.enumModeBegin.Normal;
            else if ((int)EnvironmentBase.enModeBeg == 2)
                Speed = (int)EnvironmentBase.enumModeBegin.Hard;

            Food.Score = 0;
        }
        #endregion

        public void FirstPlace()
        {
            do
            {
                Reset();
                k = new ConsoleKeyInfo();
                kOld = new ConsoleKeyInfo();
                Console.Clear();

                //Рамка
                EnvironmentSnake.Frame();
                isAlive = true;

                //Тело змеи и еда
                AllPartsOfBody(0, foodColor);
                BeginNewFood();
                do
                    Move();
                while (chetsNewLife);
            } while (k.Key != ConsoleKey.Q);
        }

        bool DirectionOriginalMove()
        {
            if ((kOld.Key == 0 && k.Key == ConsoleKey.LeftArrow) || (kOld.Key == ConsoleKey.RightArrow && k.Key == ConsoleKey.LeftArrow)
                || (kOld.Key == ConsoleKey.LeftArrow && k.Key == ConsoleKey.RightArrow) ||
                (kOld.Key == ConsoleKey.UpArrow && k.Key == ConsoleKey.DownArrow) ||
                (kOld.Key == ConsoleKey.DownArrow && k.Key == ConsoleKey.UpArrow))
                return true;
            else
                return false;
        }

        #region Move
        //Начальные движения
        public void Move()
        {
            chetsNewLife = false;
            chetsSpeed = false;
            //Первоначальное направление движения
            do
                k = Console.ReadKey(true);
             while (DirectionOriginalMove());
            while (isAlive)  //Пока живой
            {
                //Поток
                thread = new Thread(ThreadChoice);
                thread.Start();

                while (thread.IsAlive && isAlive)
                {
                    //Направление движения
                    if (k.Key == ConsoleKey.UpArrow)
                        CoordUp(ref k);
                    else if (k.Key == ConsoleKey.DownArrow)
                        CoordDown(ref k);
                    else if (k.Key == ConsoleKey.LeftArrow)
                        CoordLeft(ref k);
                    else if (k.Key == ConsoleKey.RightArrow)
                        CoordRight(ref k);
                    
                    //Проверка на нажатие рабочих клавиш
                    if (k.Key != ConsoleKey.UpArrow && k.Key != ConsoleKey.DownArrow
                        && k.Key != ConsoleKey.LeftArrow && k.Key != ConsoleKey.RightArrow &&
                        k.Key != ConsoleKey.P && k.Key != ConsoleKey.Q && k.Key != ConsoleKey.Tab)
                        k = Console.ReadKey(true);

                    //Quit
                    else if (k.Key == ConsoleKey.Q)
                    {
                        isAlive = false;
                        return;
                    }
                }
                 //Выбор Паузы
                if (k.Key == ConsoleKey.P)
                {
                    AllPartsOfBody(0, 0);

                    isPause = true;
                    EnvironmentBase.Pause(isPause);
                    Pause();
                    k = kOld;
                    isPause = false;
                    EnvironmentBase.Pause(isPause);
                    AllPartsOfBody(snakeColor, foodColor);
                }

                if (k.Key == ConsoleKey.Tab)
                {
                    TabEnter();
                }
            }
            AllPartsOfBody(0, 0);

            //Прерывание для клавиши Q
            if (k.Key == ConsoleKey.Q)
            {
                return;
            }
            else
            {
                while (k.Key != ConsoleKey.Enter && k.Key != ConsoleKey.Q)
                {
                    k = new ConsoleKeyInfo();
                    EndEnter();

                    if (k.Key == ConsoleKey.Tab)
                    {
                        TabEnter();
                    }
                    if (chetsNewLife)
                    {
                        Console.Clear();

                        //Рамка
                        EnvironmentSnake.Frame();
                        isAlive = true;

                        //Тело змеи и еда
                        AllPartsOfBody(snakeColor, foodColor);
                        BeginNewFood();
                        return;
                    }
                }
            }
        }

        void TabEnter()
        {
            AllPartsOfBody(0, 0);

            k = new ConsoleKeyInfo();
            while (k.Key != ConsoleKey.Tab)
            {
                if ((cheatList.Count) > 5)
                    cheatList.RemoveAt(0);

                if (Cheats() == 1 && !chetsNewLife && !isAlive)
                {
                    chetsNewLife = true;
                }
            }
            if (!chetsNewLife && isAlive)
            {
                k = kOld;
                //AllPartsOfBody();
                if (isPause)
                    EnvironmentBase.Pause(true);
            }
            else if (!chetsNewLife && !isAlive)
            {
                //AllPartsOfBody();
                EnvironmentBase.End();
                k = new ConsoleKeyInfo();
            }
            AllPartsOfBody(snakeColor, foodColor);
        }

        void EndEnter()
        {
            //Нажмите ЕНТЕР
            while ((k.Key != ConsoleKey.Enter && k.Key != ConsoleKey.Tab && k.Key != ConsoleKey.Q))
            {
                if (!thread.IsAlive)
                {
                    k = Console.ReadKey(true);
                }
            };
        }
        #endregion

        #region Thread
        //Поток
        static void ThreadChoice()
        {
#if Teting
            Console.SetCursorPosition(2, 2);
            Console.Write("               ");
            Console.SetCursorPosition(2, 2);
            Console.Write("Thread.IsAlive");
#endif
            do
            {
                //Если нажаты действующие клавиши и не нажата Пауза
                if ((k.Key == ConsoleKey.UpArrow || k.Key == ConsoleKey.DownArrow
                    || k.Key == ConsoleKey.LeftArrow || k.Key == ConsoleKey.RightArrow) && !isPause)
                    kOld = k;

                k = Console.ReadKey(true);
            } while (isAlive && k.Key != ConsoleKey.Tab && k.Key != ConsoleKey.Q && k.Key != ConsoleKey.P); //ДуВайл при Паузе
#if Teting
            Console.SetCursorPosition(2, 2);
            Console.Write("                ");
            Console.SetCursorPosition(2, 2);
            Console.Write("Thread.NotAlive");
#endif
            //((k.Key == ConsoleKey.UpArrow || k.Key == ConsoleKey.DownArrow) && (kOld.Key == ConsoleKey.UpArrow || kOld.Key == ConsoleKey.DownArrow)) || //ДуВайл при нажатии одинаковых или противоположных клавиш по вертикали
            //((k.Key == ConsoleKey.RightArrow || k.Key == ConsoleKey.LeftArrow) && (kOld.Key == ConsoleKey.RightArrow || kOld.Key == ConsoleKey.LeftArrow)) ||    //ДуВайл при нажатии одинаковых или противоположных клавиш по горизонтали
            //((k.Key != ConsoleKey.UpArrow && k.Key != ConsoleKey.DownArrow && k.Key != ConsoleKey.LeftArrow && k.Key != ConsoleKey.RightArrow))                  //ДуВайл при нажатии не рабочих клавиш и живой змейке
            //&&

        }
        #endregion

        #region CoordUpDownLeftRight
        //ПРОДОЛЖИТЕЛЬНОСТЬ ДВИЖЕНИЯ
        //Вверх
        void CoordUp(ref ConsoleKeyInfo k)
        {
            do
            {
                FirstAndLastBody(0, -1);
            } while (((k.Key == ConsoleKey.UpArrow || k.Key == ConsoleKey.DownArrow) || (k.Key != ConsoleKey.LeftArrow 
            && k.Key != ConsoleKey.RightArrow && k.Key != ConsoleKey.P && k.Key != ConsoleKey.Q && k.Key != ConsoleKey.Tab)) && isAlive);
        }
        //Вниз
        void CoordDown(ref ConsoleKeyInfo k)
        {
            do
            {
                FirstAndLastBody(0, 1);
            } while (((k.Key == ConsoleKey.DownArrow || k.Key == ConsoleKey.UpArrow) || (k.Key != ConsoleKey.LeftArrow 
            && k.Key != ConsoleKey.RightArrow && k.Key != ConsoleKey.P && k.Key != ConsoleKey.Q && k.Key != ConsoleKey.Tab)) && isAlive);
        }
        //Влево
        void CoordLeft(ref ConsoleKeyInfo k)
        {
            do
            {
                FirstAndLastBody(-1, 0);
            } while (((k.Key == ConsoleKey.LeftArrow || k.Key == ConsoleKey.RightArrow) || (k.Key != ConsoleKey.UpArrow 
            && k.Key != ConsoleKey.DownArrow && k.Key != ConsoleKey.P && k.Key != ConsoleKey.Q && k.Key != ConsoleKey.Tab)) && isAlive);
        }
        //Вправо
        void CoordRight(ref ConsoleKeyInfo k)
        {
            do
            {
                FirstAndLastBody(1, 0);
            } while (((k.Key == ConsoleKey.RightArrow || k.Key == ConsoleKey.LeftArrow) || (k.Key != ConsoleKey.UpArrow 
            && k.Key != ConsoleKey.DownArrow && k.Key != ConsoleKey.P && k.Key != ConsoleKey.Q && k.Key != ConsoleKey.Tab)) && isAlive);
        }
        #endregion

        #region FirstAndLast
        //Первая и последняя часть тела змеи
        void FirstAndLastBody(sbyte q, sbyte w)
        {

            BodyParts a = snakeBody[0];
            //Если еда находится в конце змеи, то эта часть не затирается чтобы дать змее вырасити
            //в нужный момент
            if(snakeBody[0].X != snakeBody[1].X || snakeBody[0].Y != snakeBody[1].Y)
                a.Body = ' ';

            //Добавление и удаление первой и последней части змеи
            old = snakeBody[0];
            snakeBody.RemoveAt(0);
            snakeBody.Add(new BodyParts(snakeBody[snakeBody.Count - 1].X + q, snakeBody[snakeBody.Count - 1].Y + w));

            //Жива ли змейка
            if (IsDeath())
            {
                snakeBody.RemoveAt(snakeBody.Count - 1);
                snakeBody.Insert(0, old);

                return;
            }
            EnvironmentSnake.ColorSnake(snakeColor);
            //Последняя часть тела затирается
            Console.SetCursorPosition(a.X, a.Y);
            Console.Write(a.Body);
            //Рисуется голова
            Console.SetCursorPosition(snakeBody.Last().X, snakeBody.Last().Y);
            Console.Write(snakeBody.Last().Body);

            //Новая еда
            BeginNewFood();
        }
        #endregion

        #region AllParts
        //Полный рисунок змеи и еды
        void AllPartsOfBody()
        {
            //EnvironmentBase.Color(1);
            foreach (BodyParts f in snakeBody)
            {
                Console.SetCursorPosition(f.X, f.Y);
                Console.Write(f.Body);
            }
            Console.SetCursorPosition(food.X, food.Y);
            Console.Write(food.apple);
        }
        void AllPartsOfBody(byte snakeColor, byte foodColor)
        {
            EnvironmentSnake.ColorSnake(snakeColor);
            foreach (BodyParts f in snakeBody)
            {
                Console.SetCursorPosition(f.X, f.Y);
                Console.Write(f.Body);
            }
            EnvironmentSnake.ColorSnake(foodColor);
            Console.SetCursorPosition(food.X, food.Y);
            Console.Write(food.apple);
        }

        #endregion

        #region NewFood
        //Новая еда
        void BeginNewFood()
        {
            if(snakeBody[snakeBody.Count - 1].X == food.X && snakeBody[snakeBody.Count - 1].Y == food.Y)
            {

                snakeColor = foodColor;
                //Score
                Console.SetCursorPosition(7, 30);
                EnvironmentBase.Color(2);  Console.Write("{0}", ++Food.Score); EnvironmentBase.Color(0);

                //Добавление размеров змеи м координатами съеденой еды
                snakeBody.Add(new BodyParts(food.X, food.Y));

                //Новая еда
                food.NewFood(snakeBody);
                foodColor = (byte)rand.Next(1, 11);
                EnvironmentSnake.ColorSnake(foodColor);
                Console.SetCursorPosition(food.X, food.Y);
                Console.Write(food.apple);

                //Увеличение скорости
                if (Food.Score % 10 == 0)
                    Speed -= 10;
            }

            //Задержка
            Thread.Sleep(Speed);
        }
        #endregion

        #region IsDeath
        //Проверяет жива ли змея
        bool IsDeath()
        {
            for (int i = 0; i < snakeBody.Count - 1; i++)
                //Врезалась ли змея сама в себя
                if (snakeBody[snakeBody.Count - 1].X == snakeBody[i].X && snakeBody[snakeBody.Count - 1].Y == snakeBody[i].Y || isAlive == false)
                {
                    //Окружение конца
                    EnvironmentBase.End();
                    isAlive = false;
                    return true;
                }
            return false;
        }
        #endregion

        #region Pause
        //Пауза
        void Pause()
        {
            k = new ConsoleKeyInfo();

            while (k.Key != ConsoleKey.P && k.Key != ConsoleKey.Q)
            {
                if (!thread.IsAlive)
                    k = Console.ReadKey(true);

                if(k.Key == ConsoleKey.Tab)
                    TabEnter();
                if (k.Key == ConsoleKey.Q)
                    kOld = k;
            }
        }
        #endregion

        byte Cheats()
        {
            EnvironmentSnake.Cheats(true);

            sb = new StringBuilder();
            while ((k = Console.ReadKey(true)).Key != ConsoleKey.Q && k.Key != ConsoleKey.Tab && k.Key != ConsoleKey.Enter)
            {
                if (k.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
                else if (k.Key != ConsoleKey.Backspace && sb.Length < 58)
                {
                    sb.Append(Convert.ToChar(k.KeyChar));
                }
                EnvironmentBase.Cheats(sb);
            }

            if (k.Key == ConsoleKey.Tab)
            {
                EnvironmentSnake.Cheats(false);
                return 0;
            }

            string cheat = Convert.ToString(sb);

            if (cheat == "new")
            {
                cheatList.Add(sn.FullName + "%" + st.FullName + "&" + cheat.GetHashCode() + "^" + cheat);
                return 1;
            }
            else
                try
                {
                    int sp = Convert.ToInt32(cheat);
                    speed = sp;
                    chetsSpeed = true;
                    cheatList.Add(sn.FullName + "%" + it.FullName + "&" + cheat.GetHashCode() + "^" + cheat);
                }
                catch(Exception e)
                {
                    cheatList.Add("Invalid input$" + e.Message + "^");
                }
            return 2;
        }
    }
}

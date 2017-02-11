using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SnakeProjectGame
{
    class Tetris
    {
        static int[,] cheatCoord;

        delegate void Move(ref ConsoleKeyInfo key);
        Dictionary<ConsoleKey, Move> dict;
        static byte[,] arrayAllOldBlocks;
        static BlockBody.EnumTetris enumTetris;

        static int[] arrBlocksLineClear;
        static int count = 1;
        static int[,] arrayOldBlock = new int[2, 4];

        static bool isNew = true;

        static List<BlockParts> blockParts;

        static ConsoleKeyInfo k;
        static ConsoleKeyInfo kOld;

        static BlockBody blockB;
        static byte counter = 0;

        Thread thread;
        public void MoveMethod()
        {
            blockB = new BlockBody();
            EnvironmentTetris.Frame();
            arrayAllOldBlocks = new byte[30, 28];

            thread = new Thread(ThreadChoice);
            thread.Start();
            do
            {
                enumTetris = blockB.NewBody(ref blockParts);
                count = 1;
                isNew = false;
                WriteBlock();
                Thread.Sleep(200);
                ClearBlock();
                do
                {
                    DefinDict(ref k);
                } while (!isNew);
                //thread.Join();
            } while (blockParts[0].Y != 1);
        }

        public Tetris()
        {
            cheatCoord = new int[2, 4];

            arrBlocksLineClear = new int[28];
            dict = new Dictionary<ConsoleKey, Move>
            {
                { ConsoleKey.LeftArrow, MoveLeft },
                { ConsoleKey.RightArrow, MoveRight },
                { 0, MoveDown },
                { ConsoleKey.Enter, EnterDown },
                { ConsoleKey.Spacebar, SpaceMove }
            };
        }

        //Enter
        static void EnterDown(ref ConsoleKeyInfo k)
        {
            k = new ConsoleKeyInfo();
            bool lo = true;
            while (lo)
            {
                if (BlockBody.IsDownStop(blockParts))
                    break;

                foreach (BlockParts a in blockParts)
                {
                    if (arrayAllOldBlocks[a.X - 25, a.Y] == 1)
                    {
                        lo = false;
                        break;
                    }                     
                }
                if(lo)
                    foreach (BlockParts a in blockParts)
                        a.Y++;
            }
            IsNew();
        }

        static void IsNew()
        {
            WriteBlock();
            Array.Clear(cheatCoord, 0, 4);
            isNew = true;
            foreach (BlockParts a in blockParts)
            {
                arrayAllOldBlocks[a.X - 25, a.Y - 1] = 1;
                arrBlocksLineClear[a.Y - 1]++;
            }
            foreach (BlockParts a in blockParts)
            {
                if (arrBlocksLineClear[a.Y - 1] == 30)
                {
                    Thread.Sleep(200);
                    ClearLine(a.Y); Thread.Sleep(200);
                    ClearAllUpBlocks(a.Y - 1);
                    LineShiftDown(a.Y - 1);
                    WriteAllUpBlocks(a.Y - 1); Thread.Sleep(200);
                }
            }
        }

        //DownArrow
        static void MoveDown(ref ConsoleKeyInfo k)
        {
            counter = 0;
            do
            {
                if (IsStopMoveBlock())
                {
                    IsNew();
                    return;
                }

                Cheat();

                foreach (BlockParts a in blockParts)
                {
                    Console.SetCursorPosition(a.X, ++a.Y);
                    Console.WriteLine(a.Body);
                }

                Thread.Sleep(300);

                ClearBlock();
            } while (k.Key != ConsoleKey.LeftArrow && k.Key != ConsoleKey.RightArrow && k.Key != ConsoleKey.Enter && k.Key != ConsoleKey.Spacebar);
        }

        //LeftArrow
        static void MoveLeft(ref ConsoleKeyInfo k)
        {
            k = new ConsoleKeyInfo();

            if (counter > 15)
                return;

            counter++;

            if (BlockBody.IsLeftStop(blockParts))
                return;
            else if (IsStopLeftRightMoveBlock())
            {
                BlockBody.MoveRecoil(blockParts, 1);
                return;
            }

            Cheat();

            foreach (BlockParts a in blockParts)
            {
                Console.SetCursorPosition(a.X, a.Y);
                Console.WriteLine(a.Body);
            }

            if (counter == 1)
                Thread.Sleep(200);
            else
                Thread.Sleep(50);

            ClearBlock();

        }

        //RightArrow
        static void MoveRight(ref ConsoleKeyInfo k)
        {
            k = new ConsoleKeyInfo();

            if (counter > 15)
                return;

            counter++;

            if (BlockBody.IsRightStop(blockParts))
                return;
            else if (IsStopLeftRightMoveBlock())
            {
                BlockBody.MoveRecoil(blockParts, -1);
                return;
            }

            Cheat();

            foreach (BlockParts a in blockParts)
            {
                Console.SetCursorPosition(a.X, a.Y);
                Console.WriteLine(a.Body);
            }

            if (counter == 1)
                Thread.Sleep(200);
            else
                Thread.Sleep(40);

            ClearBlock();

        }

        static void SpaceMove(ref ConsoleKeyInfo k)
        {
            k = new ConsoleKeyInfo();
            int ind = 0;
            foreach (BlockParts a in blockParts)
            {
                arrayOldBlock[0, ind] = a.X;
                arrayOldBlock[1, ind++] = a.Y;
            }
            count++;

            switch (enumTetris)
            {
                case BlockBody.EnumTetris.L:
                    {
                        if (count > 4)
                            count = 1;

                        //L
                        switch (count)
                        {
                            case 1: blockParts[0].Y += 2; blockParts[2].X += 1; blockParts[2].Y += 1; blockParts[3].X += 1; blockParts[3].Y += 1; break;
                            case 2: blockParts[0].X += 1; blockParts[0].Y -= 2; blockParts[2].X -= 1; blockParts[2].Y -= 1; blockParts[3].Y += 1; break;
                            case 3: blockParts[0].X -= 1; blockParts[0].Y += 1; blockParts[1].X -= 1; blockParts[3].Y -= 1; break;
                            case 4: blockParts[0].Y -= 1; blockParts[1].X += 1; blockParts[3].X -= 1; blockParts[3].Y -= 1; break;
                        }
                        break;
                    }
                case BlockBody.EnumTetris.T:
                    {
                        if (count > 4)
                            count = 1;

                        //T
                        switch (count)
                        {
                            case 1: blockParts[0].Y += 1; blockParts[3].X += 1; blockParts[3].Y += 2; break;
                            case 2: blockParts[0].X += 1; blockParts[0].Y -= 2; blockParts[3].Y -= 1; break;
                            case 3: blockParts[0].X -= 1; blockParts[0].Y += 1; break;
                            case 4: blockParts[3].X -= 1; blockParts[3].Y -= 1; break;
                        }
                        break;
                    }
                case BlockBody.EnumTetris.I:
                    {
                        if (count > 2)
                            count = 1;

                        //I
                        switch (count)
                        {
                            case 1: blockParts[0].X -= 1; blockParts[0].Y += 1; blockParts[1].Y -= 2; blockParts[2].X += 1; blockParts[3].X += 2; blockParts[3].Y -= 1; break;
                            case 2: blockParts[0].X += 1; blockParts[0].Y -= 1; blockParts[1].Y += 2; blockParts[2].X -= 1; blockParts[3].X -= 2; blockParts[3].Y += 1; break;
                        }
                        break;
                    }
                case BlockBody.EnumTetris.G:
                    {
                        if (count > 2)
                            count = 1;

                        //G
                        switch (count)
                        {
                            case 1: blockParts[0].Y += 1; blockParts[1].Y -= 1; blockParts[2].X += 1; blockParts[2].Y -= 1; blockParts[3].X += 1; blockParts[3].Y -= 1; break;
                            case 2: blockParts[0].Y -= 1; blockParts[1].Y += 1; blockParts[2].X -= 1; blockParts[2].Y += 1; blockParts[3].X -= 1; blockParts[3].Y += 1; break;
                        }
                        break;
                    }
            }

            foreach(BlockParts bl in blockParts)
            {
                if(bl.isLeftRightStopBlock || bl.isStopBlock)
                {
                    ActionRollback();
                    return;
                }
            }
            if (IsStopLeftRightMoveBlock())
                ActionRollback();
        }

        //RollBack
        static void ActionRollback()
        {
            count--;
            int ind = 0;
            foreach(BlockParts a in blockParts)
            {
                a.X = arrayOldBlock[0, ind];
                a.Y = arrayOldBlock[1, ind++];
            }
        }
        /// <summary>
        /// Cheat
        /// </summary>
        static void Cheat()
        {
            if (cheatCoord[0, 0] != 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Console.SetCursorPosition(cheatCoord[0, i], cheatCoord[1, i]);
                    Console.Write(' ');
                }
            }
            int count = -1;
            bool wh = true;
            while (wh)
            {
                count++;

                foreach (BlockParts bl in blockParts)
                {
                    if(bl.Y == 28 || (bl.Y + count) == 28)
                    {
                        wh = false;
                        break;
                    }
                    else if (arrayAllOldBlocks[bl.X - 25, bl.Y + count] != 0)
                    {
                        wh = false;
                        break;
                    }
                }
            }

            if (blockParts[1].Y < 28)
            {
                EnvironmentBase.Color(3);
                int ind = 0;
                foreach (BlockParts bl in blockParts)
                {
                    cheatCoord[0, ind] = bl.X;
                    cheatCoord[1, ind++] = bl.Y + count;
                    Console.SetCursorPosition(bl.X, bl.Y + count);
                    Console.Write(bl.Body);
                }
                EnvironmentBase.Color(0);
            }
        }

        //Clear Line
        static void ClearLine(int yCoord)
        {
            for (int i = 0; i < 30; i++)
                arrayAllOldBlocks[i, yCoord - 1] = 0;
            Console.SetCursorPosition(25, yCoord);
            Console.Write(new string (' ', 30));
        }

        //Line Down
        static void LineShiftDown(int yCoord)
        {
            for(int i = 0; i < 30; i++)
                for(int j = yCoord - 1; j > 0; j--)
                    if(arrayAllOldBlocks[i, j] != 0)
                    {
                        arrayAllOldBlocks[i, j + 1] = arrayAllOldBlocks[i, j];
                        arrayAllOldBlocks[i, j] = 0;
                    }
            for (int i = yCoord; i > 0; i--)
                arrBlocksLineClear[i] = arrBlocksLineClear[i - 1];
            arrBlocksLineClear[0] = 0;
        }

        //Write All Blocks
        static void WriteAllUpBlocks(int yCoord)
        {
            for(int i = 0; i < 30; i++)
                for (int j = 27; j > 0; j--)
                    if (arrayAllOldBlocks[i, j] != 0)
                    {
                        Console.SetCursorPosition(i + 25, j + 1);
                        Console.Write('#');
                    }
        }

        //Clear All Blocks
        static void ClearAllUpBlocks(int yCoord)
        {
            for (int i = 0; i < 30; i++)
                for (int j = yCoord - 1; j >= 0; j--)
                    if (arrayAllOldBlocks[i, j] != 0)
                    {
                        Console.SetCursorPosition(i + 25, j + 1);
                        Console.Write(' ');
                    }
        }

        //Write Block
        static void WriteBlock()
        {
            foreach (BlockParts a in blockParts)
            {
                Console.SetCursorPosition(a.X, a.Y);
                Console.Write(a.Body);
            }
        }
        //Clear Block
        static void ClearBlock()
        {
            foreach (BlockParts a in blockParts)
            {
                Console.SetCursorPosition(a.X, a.Y);
                Console.Write(' ');
            }
        }

        #region Thread
        //Поток
        static void ThreadChoice()
        {
            do
            {
                k = Console.ReadKey(true);
            } while (k.Key != ConsoleKey.Tab  && k.Key != ConsoleKey.Q && k.Key != ConsoleKey.P);
        }
        #endregion

        //Check
        static bool IsStopMoveBlock()
        {
            if (BlockBody.IsDownStop(blockParts))
                return true;

            foreach (BlockParts a in blockParts)
                if (arrayAllOldBlocks[a.X - 25, a.Y] == 1)
                    return true;
            return false;
        }
        static bool IsStopLeftRightMoveBlock()
        {
            foreach (BlockParts a in blockParts)
                if (arrayAllOldBlocks[a.X - 25, a.Y - 1] == 1)
                    return true;
            return false;
        }
        
        //Dictionary
        public void DefinDict(ref ConsoleKeyInfo k)
        {
            if (dict.ContainsKey(k.Key))
                dict[k.Key](ref k);
        }
    }
}

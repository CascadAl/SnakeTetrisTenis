using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeProjectGame
{
    class Food
    {
        int x, y;
        static int score;
        byte[,] array = new byte[EnvironmentBase.width, EnvironmentBase.height];
        List<Food> f = new List<Food>();

        Random rand = new Random();
        public readonly char apple = '@';

        public int X
        {
            get { return x; }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get { return y; }
            set
            {
                y = value;
            }
        }

        public static int Score
        {
            get { return score; }
            set
            {
                score = value;
            }
        }

        public Food()
        {
            X = rand.Next(1, EnvironmentBase.width);
            Y = rand.Next(1, EnvironmentBase.height);
        }
        private Food(int u, int m)
        {
            X = u;
            Y = m;
        }


        public void NewFood(List<BodyParts> body)
        {
            foreach (BodyParts b in body)
                array[b.X, b.Y] = 1;

            X = rand.Next(1, EnvironmentBase.width);
            Y = rand.Next(1, EnvironmentBase.height);

            if (array[X, Y] == 1)
            {
                for (byte i = 1; i < EnvironmentBase.width; i++)
                    for (byte j = 1; j < EnvironmentBase.height; j++)
                        if (array[i, j] == 0)
                            f.Add(new Food(i, j));

                int foodList = rand.Next(0, f.Count);
                X = f[foodList].X;
                Y = f[foodList].Y; 
            }
        }
    }
}

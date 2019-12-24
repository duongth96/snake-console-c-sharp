using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeConsole
{
    class Program
    {
        private const int SCREEN_WIDTH = 50;
        private const int SCREEN_HEIGHT = 20;
        private static int ScreenSleep = 200;
        static Snake snake = new Snake(new List<XY> { new XY(0, 0), new XY(0, 1), new XY(0, 1) }, Status.RIGHT);
        static Girl vp = new Girl(12, 12);
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            while (true)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.White;
                // ti so
                Console.SetCursorPosition(SCREEN_WIDTH + 2,0);
                Console.Write($"Score: {snake.node.Count-3}");
                // wall
                for (var i = 0; i < SCREEN_WIDTH; i++)
                {
                    Console.SetCursorPosition(i, SCREEN_HEIGHT);
                    Console.Write("_");
                }
                for (var i = 0; i < SCREEN_HEIGHT; i++)
                {
                    Console.SetCursorPosition(SCREEN_WIDTH, i);
                    Console.Write("|");
                }

                // read key and control
                Thread newTh = new Thread(ReadCh);
                newTh.Start();

                // hien thi
                Console.SetCursorPosition(vp.x, vp.y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("G");

                foreach (var s in snake.node)
                {
                    Console.SetCursorPosition(s.x==-1?0:s.x, s.y);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("o");
                }

                // check cham bien
                if (snake.node[0].y > SCREEN_HEIGHT || snake.node[0].y < 0 || snake.node[0].x > SCREEN_WIDTH || snake.node[0].x < 0)
                {
                    GameOver(snake.node.Count);
                }
                // check can
                for (var i = 1; i < snake.node.Count; i++)
                {
                    if (snake.node[0].y == snake.node[i].y && snake.node[0].x == snake.node[i].x)
                    {
                        GameOver(snake.node.Count);
                    }
                }


                // set lai pos node
                for (var i = snake.node.Count - 1; i > 0; i--)
                {
                    snake.node[i].x = snake.node[i - 1].x;
                    snake.node[i].y = snake.node[i - 1].y;
                }

                // action control head snake
                if (snake.tt == Status.DOWN)
                    snake.node[0].y++;
                else if (snake.tt == Status.UP)
                    snake.node[0].y--;
                else if (snake.tt == Status.RIGHT)
                    snake.node[0].x++;
                else if (snake.tt == Status.LEFT)
                    snake.node[0].x--;

                if (snake.node[0].x == vp.x && snake.node[0].y == vp.y)
                {
                    if (snake.tt == Status.DOWN || snake.tt == Status.UP)
                    {
                        var xy = new XY(snake.node[0].x, snake.node[0].y + 1);
                        snake.node.Add(xy);
                    }
                    else if (snake.tt == Status.RIGHT || snake.tt == Status.LEFT)
                    {
                        var xy = new XY(snake.node[0].x + 1, snake.node[0].y);
                        snake.node.Add(xy);
                    }

                    Random r = new Random();
                    vp.x = r.Next(0, SCREEN_WIDTH);
                    vp.y = r.Next(0, SCREEN_HEIGHT);
                    ScreenSleep = ScreenSleep -2;
                }

                Thread.Sleep(ScreenSleep);
            }
        }
        public static void GameOver(int sc = 0)
        {
            string text = "Gameover!";
            string textScore = $"Your score: {sc - 3}";
            Console.SetCursorPosition(SCREEN_WIDTH / 2 - text.Length, SCREEN_HEIGHT / 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(text);
            Console.SetCursorPosition(SCREEN_WIDTH / 2 - textScore.ToString().Length, SCREEN_HEIGHT / 2 + 1);
            Console.Write(textScore);
            var x = Console.ReadKey();
            if (x.Key == ConsoleKey.Y)
            {
                snake = new Snake(new List<XY> { new XY(0, 0), new XY(0, 1), new XY(0, 1) }, Status.RIGHT);
            }
            else
            {
                Environment.Exit(0);
            }
        }
        public static void ReadCh()
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.W)
            {
                snake.tt = Status.UP;
            }
            else if (key.Key == ConsoleKey.S)
            {
                snake.tt = Status.DOWN;
            }
            else if (key.Key == ConsoleKey.D)
            {
                snake.tt = Status.RIGHT;
            }
            else if (key.Key == ConsoleKey.A)
            {
                snake.tt = Status.LEFT;
            }
        }
    }
    class XY
    {
        public XY(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x { get; set; }
        public int y { get; set; }
    }
    class Snake
    {
        public List<XY> node { get; set; }
        public Status tt { get; set; }
        public Snake(List<XY> node, Status tt)
        {
            this.node = node;
            this.tt = tt;
        }
    }
    class Girl
    {
        public Girl(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x { get; set; }
        public int y { get; set; }
    }
    enum Status { UP, DOWN, LEFT, RIGHT }
}

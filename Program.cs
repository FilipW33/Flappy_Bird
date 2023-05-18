namespace FlappyBird


{
    class program
    {
        enum BirdState { alive, dead };
        static char[,] world;
        static int width;
        static int height;

        const char SPACE = ' ';
        const char WALL = '*';
        const char player = '^';
        const char obstacle = '#';


        const int fall_rate = 1;
        const int jump = 2;
        const int FPS = 60;

        static int playerX;
        static int playerPos;
        static BirdState birdState;
        static int FrameCounter;


        const int ObFreq = 30;


        static void Init()
        {
            birdState = BirdState.alive;
            FrameCounter = 0;

            width = Console.WindowWidth; // creation and display game board 
            height = Console.WindowHeight;

            world = new char[width, height];
            for (int w = 0; w < width; ++w)
            {
                for (int h = 0; h < height; ++h)
                {
                    world[w, h] = SPACE;
                }
            }

            for (int w = 0; w < width; ++w)
            {
                world[w, 0] = WALL;
                world[w, height - 1] = WALL;
            }
            playerX = height / 3;   // start bird position
            playerPos = height / 2;

            world[playerX, playerPos] = player;
            Console.CursorVisible = false; // disable cursor 
        }
        static void render() // this function making game stable, beacuse it's limited to the size of console
        {
            string buffer = "";
            for (int h = 0; h < height; ++h)
            {
                for (int w = 0; w < width; ++w)
                {
                    buffer += world[w, h];
                    //Console.Write(world[w, h]);
                }
                if (h != (height - 1))
                {
                    buffer += "\n";
                }
                //Console.Write("\n");
            }
            Thread.Sleep(1000 / FPS);
            Console.SetCursorPosition(0, 0);
            Console.Write(buffer);
            FrameCounter++;
        }
        static void procesUserinput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Spacebar)
                {
                    //take bird up 
                    ChangePlayerPos(playerPos - jump);
                }
            }
        }
        static bool CheckCollision(int newPost)
        {
            if (newPost < 0)
            {
                return true;
            }
            if (newPost >= height)
            {
                return true;
            }
            if (world[playerX, newPost] == WALL)
            {
                return true;
            }
            if (world[player, newPost] == obstacle)
            {
                return true;
            }
            return false;
        }
        static void fall()
        {
            ChangePlayerPos(playerPos + fall_rate);
        }
        /*  abstract class obj()  
          {
                lenght=x;
                 height=x; 
          }*/

        static void ChangePlayerPos(int newPos)
        {
            world[playerX, playerPos] = SPACE;
            if (CheckCollision(newPos))
            {
                birdState = BirdState.dead;
            }
            else
            {
                world[playerX, newPos] = player;
                playerPos = newPos;
            }

        }
        static void GenerateObstracle()
        {

            int obstacleHeight = height / 2 - 5;

            // int obstangleWidth = width / 3;
            // dodać długość przeszkody
            Random randomGenerator = new Random();
            bool upOrDown = randomGenerator.Next(2) % 2 == 0;
            int obstacleWidth = randomGenerator.Next(5, 11); /* I tryed to add another random generator to generate to generate a end of the obstacle */

            /* int obstacleWidth = width / 2 - 15;
            Random radnomgenerat2 = new Random();                   here is a result of this try
            bool lenght = radnomgenerat2.Next(2) % 2 == 0; */

            if (upOrDown)
            {
                for (int i = 1; i < obstacleHeight; ++i)
                {
                    world[width - 1, i] = obstacle;

                    for (int j = 1; j < obstacleWidth; ++j)
                    {
                        world[width - 1 - j, j] = obstacle;  // if I copy a loop as same as hight loop does give me back lenght?
                    }
                }

                // tutaj byl odał pętlę taką samą jak na height czy dostałbym wtedy długość przeszkody??
            }
            else
            {
                for (int i = 0; i < obstacleHeight; ++i)
                {
                    world[width - 1, height - 2 - i] = obstacle;
                    for (int j = 0; j < obstacleWidth; ++j)
                    {
                        world[width - 1 - j, height - 2 - i] = obstacle;
                    }

                }
            }





        }
        static void MoveObstracle()
        {
            for (int w = 1; w < width - 1; ++w)
            {
                for (int h = 1; h < height; ++h)
                {
                    if (world[w, h] == player || world[w + 1, h] == player)
                    {
                        continue;
                    }

                    world[w, h] = world[w + 1, h];
                }
            }
        }
        static bool IsGameRunning()
        {
            return birdState == BirdState.alive;
        }

        static int Main(string[] args)
        {

            while (true)
            {
                Init();
                while (IsGameRunning())
                {
                    procesUserinput();
                    render();
                    if (FrameCounter % 5 == 0)
                    {
                        fall();
                        if (FrameCounter % ObFreq == 0) // if FrameCounte is divisible by ObFreq if is so generate a new obstacle if no then not 
                        {
                            Random randomGenerator = new Random();
                            int randomNum = randomGenerator.Next(0, 2);
                            if (randomNum == 0)
                            {
                                GenerateObstracle();
                            }
                        }
                        MoveObstracle();
                    }
                }
            }
        }
    }
}
using System.Windows.Media;

namespace WpfApp1
{
    public class GameEngine
    {
        private readonly Random random = new Random();
        private Direction[] directions;
        private readonly int gameWidth;
        private readonly int gameHeight;

        public int getGameWidth()
        {
            return gameWidth;
        }

        public int getGameHight()
        { 
            return gameHeight;
        }

        private List<Direction> ClusterDirections;
        private HashSet<Direction> TakenPositions;
        private int pos_x, pos_y, directionsIndex;
        private double speed;

        private Cell[,] matrix;
        private int cellsNum, porosity;

        public GameEngine(int width, int porosity)
        {
            this.gameWidth = this.gameHeight = width;
            this.porosity = porosity;

            matrix = new Cell[gameWidth, gameHeight];

            directions =
            [
                new Direction { X = -1, Y = 0 },
                new Direction { X = 1, Y = 0 },
                new Direction { X = 0, Y = -1 },
                new Direction { X = 0, Y = 1 },
                new Direction { X = -1, Y = -1 },
                new Direction { X = 1, Y = 1 },
                new Direction { X = -1, Y = 1 },
                new Direction { X = 1, Y = -1 }
            ];
            InitializeTheValues();
        }

        private void InitializeTheValues()
        {
            cellsNum = GetRandomNumber(40, 45);
            ClusterDirections = new List<Direction>();

            while (ClusterDirections.Count != cellsNum)
            {
                var ps_x = GetRandomNumber(0, gameHeight - 1);
                var ps_y = GetRandomNumber(0, gameWidth - 1);
                var position = new Direction { X = ps_x, Y = ps_y };

                if (!ClusterDirections.Any(d => d.X == position.X && d.Y == position.Y))
                {
                    ClusterDirections.Add(position);
                }
            }
            TakenPositions = new HashSet<Direction>(ClusterDirections);
            
            do
            {
                pos_x = random.Next(gameHeight);
                pos_y = random.Next(gameHeight);
            } while (TakenPositions.Contains(new Direction { X = pos_x, Y = pos_y }));

            Build();
        }

        private void GenerateNewCell()
        {
            do
            {
                pos_x = random.Next(gameHeight);
                pos_y = random.Next(gameHeight);
            } while (TakenPositions.Contains(new Direction { X = pos_x, Y = pos_y }));

            directionsIndex = GetRandomNumber(0, directions.Length - 1);

            Cell cell = new Cell(
                direct: directions[directionsIndex],
                speed: GetRandomDouble(7.2, 9.1)
            );
            cell.State = Brushes.Purple;
            speed = cell.Speed;

            matrix[pos_x, pos_y] = cell;
        }

        public double GetCurrentCellSpeed()
        {
            return speed;
        }

        public void PlayGame()
        {
            if (cellsNum != porosity)
            {
                var currentDirection = matrix[pos_x, pos_y].Directions;

                int next_x = pos_x + currentDirection.X;
                int next_y = pos_y + currentDirection.Y;

                if (directions.Any(direction =>
                    pos_x + direction.X >= 0 && pos_x + direction.X < gameHeight &&
                    pos_y + direction.Y >= 0 && pos_y + direction.Y < gameWidth &&
                    (matrix[pos_x + direction.X, pos_y + direction.Y] != null)))
                {
                    TakenPositions.Add(new Direction { X = pos_x, Y = pos_y });
                    cellsNum++;
                    GenerateNewCell();
                }
                else if (((currentDirection.X == -1 && next_x < 0) ||
                        (currentDirection.X == 1 && next_x >= gameHeight) ||
                        (currentDirection.Y == -1 && next_y < 0) ||
                        (currentDirection.Y == 1 && next_y >= gameWidth)))
                {
                    matrix[pos_x, pos_y] = null;
                    GenerateNewCell();
                }
                else
                {
                    (matrix[pos_x, pos_y], matrix[next_x, next_y]) =
                     (matrix[next_x, next_y], matrix[pos_x, pos_y]);
                    pos_x += currentDirection.X;
                    pos_y += currentDirection.Y;
                    directionsIndex = GetRandomNumber(0, directions.Length - 1);
                    matrix[pos_x, pos_y].Directions = directions[directionsIndex];
                    speed = GetRandomDouble(7.2, 9.1);
                    matrix[pos_x, pos_y].Speed = speed;
                }
            }
            else
            {
                return;
            }
        }

        public void Build()
        {
            for (int x = 0; x < gameHeight; x++)
            {
                for (int y = 0; y < gameWidth; y++)
                {
                    if ((ClusterDirections.FindIndex(d => d.X == x && d.Y == y)) != -1)
                    {
                        Cell cell = new Cell(
                            direct: null,
                            speed: 0
                        );
                        cell.State = Brushes.Red;

                        matrix[x, y] = cell;
                    }
                    else
                    {
                        matrix[x, y] = null;
                    }
                }
            }
            GenerateNewCell();
        }

        public Cell[,] GetMatrix()
        {
            return matrix;
        }

        private int GetRandomNumber(int min, int max) => random.Next(min, max + 1);
        public double GetRandomDouble(double a, double b)
        {
            if (a > b)
            {
                (a, b) = (b, a);
            }
            return random.NextDouble() * (b - a) + a;
        }
    }
}
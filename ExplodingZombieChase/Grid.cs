using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingZombieChase
{
    public class Grid
    {
        public List<List<GridSquare>> GridMap;
        Random ChanceToPlace;
        public const int OPEN = 0;
        public const int CHARACTER = 1;
        public const int BARRIER = 2;
        public const int ZOMBIE = 3;
        public const int ESCAPE = 4;
        public bool GameLost = false;
        public bool ResetTurn = false;
        public List<Zombie> ZombieList = [];
        public Player? Player1;
        
        public Grid(int rows, int columns, double percentZombies, double percentBarriers)
        {
            GridMap = [];
            ChanceToPlace = new Random();
            InitializeGrid(rows, columns);
            PlaceAllPieces(percentZombies, percentBarriers);
        }

        public Grid InitializeGrid(int rows = 14, int columns = 18)
        {
            for (int i = 0; i < rows; i++)
            {
                GridMap.Add([]);
                for (int j = 0; j < columns; j++)
                {
                    GridMap[i].Add(new GridSquare());
                }
            }
            return this;
        }

        public Grid PlaceAllPieces(double percentZombies = 0.1, double percentBarriers = 0.1)
        {
            GridMap[0][0].Status = CHARACTER;
            Player1 = new Player(0, 0);
            GridMap[GridMap.Count - 1][GridMap[0].Count - 1].Status = ESCAPE;
            double randomNumber;
            for (int i = 2; i < GridMap.Count - 1; i++)
            {
                for (int j = 2; j < GridMap[i].Count - 1; j++)
                {
                    randomNumber = ChanceToPlace.NextDouble();
                    if (randomNumber <= percentBarriers)
                    {
                        GridMap[i][j].Status = BARRIER;
                    }
                    else if (randomNumber <= percentBarriers + percentZombies)
                    {
                        Zombie zombie = new Zombie(i, j);
                        ZombieList.Add(zombie);
                        GridMap[i][j].Status = ZOMBIE;
                    }
                }
            }
            return this;
        }

        public bool IsValidCoord(int rowNum, int colNum, int rowModifier = 1, int colModifier = 1)
        {
            if (rowNum < 0 || rowNum >= GridMap.Count || colNum < 0 || colNum >= GridMap[0].Count || (rowModifier == 0 && colModifier == 0))
            {
                return false;
            }
            return true;
        }

        public void DisplayGrid()
        {
            Console.WriteLine();
            Console.Write("  ");
            for (int i = 0; i < GridMap[0].Count; i++)
            {
               Console.Write("___");
            }
            Console.WriteLine("_");
            var rowShade = ConsoleColor.DarkGreen;
            for (int i = 0; i < GridMap.Count; i++)
            {
                Console.Write("  | ");
                string spacing = "  ";
                for (int j = 0; j < GridMap[i].Count; j++)
                {
                    if (j == GridMap[i].Count - 1)
                    {
                        spacing = " ";
                    }
                    GridSquare square = GridMap[i][j];
                    int status = square.Status;
                    if (status == OPEN)
                    {
                        Console.ForegroundColor = rowShade;
                        Console.Write($" {spacing}");
                        Console.ResetColor();
                    }
                    else if (status == BARRIER)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write($"0{spacing}");
                        Console.ResetColor();
                    }
                    else if (status == ZOMBIE)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"*{spacing}");
                        Console.ResetColor();
                    }
                    else if (status == CHARACTER)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write($"P{spacing}");
                        Console.ResetColor();
                    }
                    else if (status == ESCAPE)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write($" {spacing}");
                        Console.ResetColor();
                    }
                    else
                    {
                        throw new Exception("Somehow square has a status out of range");
                    }
                }
                Console.WriteLine($"| ");
            }
            Console.Write("  |");
            for (int i = 0; i < GridMap[0].Count; i++)
            {
                Console.Write("\u0304\u0304\u0304");
                if (i == GridMap[0].Count - 1)
                {
                    Console.Write("|\n     ");
                }
            }
            Console.WriteLine();
        }
        /*
        public void ExploreSquare(int row, int column)
        {
            if (IsValidCoord(row, column) && GridMap[row][column].Status == UNEXPLORED)
            {
                GridMap[row][column].Status = EXPLORED;
                if (GridMap[row][column].IsBomb)
                {
                    GameLost = true;
                    return;
                }

                if (GridMap[row][column].NumBombsAround == 0)
                {
                    ExploreSquare(row - 1, column - 1);
                    ExploreSquare(row, column - 1);
                    ExploreSquare(row + 1, column - 1);
                    ExploreSquare(row + 1, column);
                    ExploreSquare(row + 1, column + 1);
                    ExploreSquare(row, column + 1);
                    ExploreSquare(row - 1, column + 1);
                    ExploreSquare(row - 1, column);
                }
            }
        }

        public Grid? MarkSquare(int exploreOrFlag, int row, int column)
        {
            if (GridMap[row][column].Status == EXPLORED)
            {
                Console.WriteLine($"Square at row {row}, column {column} is already explored. Try again");
                return this;
            }
            if (exploreOrFlag == EXPLORED)
            {
                ExploreSquare(row, column);
            }
            else if (exploreOrFlag == FLAGGED)
            {
                FlagSquare(row, column);
            }

            return this;
        }

        public bool CheckIfWinner()
        {
            for (int i = 0; i < GridMap.Count; i++)
            {
                for (int j = 0; j < GridMap[i].Count; j++)
                {
                    if (GridMap[i][j].Status == UNEXPLORED)
                    {
                        return false;
                    }
                    else if (GridMap[i][j].Status == FLAGGED & !GridMap[i][j].IsBomb)
                    {
                        return false;
                    }
                }
            }
            return true;
        }*/
    }
}

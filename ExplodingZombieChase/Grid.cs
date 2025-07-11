using System;
using System.Collections.Generic;
using System.Data;
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
        public const int OPENANDDEAD = 5;
        public const int ZOMBIEANDKILLED = 6;
        public bool GameLost = false;
        public bool ResetTurn = false;
        public List<Zombie> ZombieList = [];
        public Player Character;
        
        public Grid(int rows, int columns, double percentZombies, double percentBarriers)
        {
            GridMap = [];
            ChanceToPlace = new Random();
            InitializeGrid(rows, columns);
            PlaceAllPieces(percentZombies, percentBarriers);
            Character = new Player(11, 0);
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

        public Grid PlaceAllPieces(double percentZombies = 0, double percentBarriers = 0.1)
        {
            
            GridMap[GridMap.Count - 1][GridMap[0].Count - 1].PieceType = ESCAPE;
            double randomNumber;
            for (int i = 2; i < GridMap.Count - 1; i++)
            {
                for (int j = 2; j < GridMap[i].Count - 1; j++)
                {
                    randomNumber = ChanceToPlace.NextDouble();
                    if (randomNumber <= percentBarriers)
                    {
                        GridMap[i][j].PieceType = BARRIER;
                    }
                    else if (randomNumber <= percentBarriers + percentZombies)
                    {
                        /*Zombie zombie = new Zombie(i, j);
                        ZombieList.Add(zombie);
                        GridMap[i][j].PieceType = ZOMBIE;*/
                    }
                }
            }
            GridMap[11][0].PieceType = CHARACTER;
            GridMap[12][17].PieceType = ZOMBIE;
            Zombie zombie = new Zombie(12, 17);
            ZombieList.Add(zombie);
            GridMap[10][17].PieceType = ZOMBIE;
            zombie = new Zombie(10, 17);
            ZombieList.Add(zombie);
            return this;
        }

        public bool IsValidCoord(int rowNum, int colNum)
        {
            if (rowNum < 0 || rowNum >= GridMap.Count || colNum < 0 || colNum >= GridMap[0].Count)
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
               Console.Write("__");
            }
            Console.WriteLine("_");
            var rowShade = ConsoleColor.DarkGreen;
            for (int i = 0; i < GridMap.Count; i++)
            {
                Console.Write("  |");
                string spacing = " ";
                for (int j = 0; j < GridMap[i].Count; j++)
                {
                    if (j == GridMap[i].Count - 1)
                    {
                        spacing = "";
                    }
                    GridSquare square = GridMap[i][j];
                    int status = square.PieceType;
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
                    else if (status == OPENANDDEAD)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.Write($" {spacing}");
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
                Console.WriteLine($"|");
            }
            Console.Write("  |");
            bool writeSecondCharacter = true;
            for (int i = 0; i < GridMap[0].Count; i++)
            {
                Console.Write("\u0304");
                if (i == GridMap[0].Count - 1)
                {
                    writeSecondCharacter = false;
                    Console.Write("|\n");
                }
                if (writeSecondCharacter)
                { 
                    Console.Write("\u0304");
                }
            }
            Console.WriteLine();
        }

        public void MoveCharacter(int rowMove, int colMove)
        {
            int row = Character.row + rowMove;
            int column = Character.column + colMove;
            if (!IsValidCoord(row, column))
            {
                Console.WriteLine("You have tried to move out of bounds. Try again");
                ResetTurn = true;
                return;
            }
            GridSquare square = GridMap[row][column];
            if (square.PieceType == OPEN)
            {
                GridMap[Character.row][Character.column].PieceType = OPEN;
                Character.row = row;
                Character.column = column;
                GridMap[row][column].PieceType = CHARACTER;
            }
            else if (square.PieceType == BARRIER)
            {
                Console.WriteLine("You cannot move into a barrier. Try again");
                ResetTurn = true;
            }
            else if (square.PieceType == ZOMBIE)
            {
                Console.WriteLine("You've hit a zombie! You died and your guts exploded everywhere");
                ResetTurn = true;
                GameLost = true;
            }
        }

        public void MoveZombie(Zombie zombie)
        {
            int row = zombie.row;
            int column = zombie.column;
            int rowDifference = Character.row - row;
            int colDifference = Character.column - column;
            if (rowDifference < 0)
            {
                row--;
            }
            else if (rowDifference > 0)
            {
                row++;
            }
            if (colDifference < 0)
            {
                column--;
            }
            else if (colDifference > 0)
            {
                column++;
            }
            if (!IsValidCoord(row, column))
            {
                return;
            }
            switch (GridMap[row][column].PieceType)
            {
                case OPEN:
                    GridMap[zombie.row][zombie.column].PieceType = OPEN;
                    zombie. row = row;
                    zombie. column = column;
                    GridMap[row][column].PieceType = ZOMBIE;
                    break;
                case BARRIER:
                    break;
                case ZOMBIE:
                    GridMap[zombie.row][zombie.column].PieceType = OPEN;
                    FindAndKillZombie(row, column);
                    zombie.row = row;
                    zombie.column = column;
                    zombie.isAlive = false;
                    GridMap[row][column].PieceType = OPENANDDEAD;
                    break;
                case OPENANDDEAD:
                    GridMap[zombie.row][zombie.column].PieceType = OPEN;
                    zombie.row = row;
                    zombie.column = column;
                    GridMap[row][column].PieceType = ZOMBIE;
                    break;
                case CHARACTER:
                    GridMap[zombie.row][zombie.column].PieceType = OPEN;
                    FindAndKillZombie(row, column);
                    zombie.row = row;
                    zombie.column = column;
                    GridMap[row][column].PieceType = ZOMBIEANDKILLED;
                    break;
            }

        }

        public void MoveAllZombies()
        {
            Zombie zombie;
            for (int i = 0; i < ZombieList.Count; i++)
            {
                zombie = ZombieList[i];
                if (zombie.isAlive)
                {
                    MoveZombie(zombie);
                }
            }
        }

        public void FindAndKillZombie(int row, int column)
        {
            for (int i = 0; i < ZombieList.Count; i++)
            {
                Zombie zombie = ZombieList[i];
                if (zombie.row == row && zombie.column == column)
                {
                    zombie.isAlive = false;
                    return;
                }
            }
        }
        /*
        public void ExploreSquare(int row, int column)
        {
            if (IsValidCoord(row, column) && GridMap[row][column].PieceType == UNEXPLORED)
            {
                GridMap[row][column].PieceType = EXPLORED;
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
            if (GridMap[row][column].PieceType == EXPLORED)
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
                    if (GridMap[i][j].PieceType == UNEXPLORED)
                    {
                        return false;
                    }
                    else if (GridMap[i][j].PieceType == FLAGGED & !GridMap[i][j].IsBomb)
                    {
                        return false;
                    }
                }
            }
            return true;
        }*/
    }
}

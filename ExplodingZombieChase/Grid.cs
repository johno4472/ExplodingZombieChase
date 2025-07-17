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
        public const int ZOMBIEANDPLAYERKILLED = 6;
        public const int ZOMBIEANDDEAD = 7;
        public const int JUSTDIEDANDWILLDIE = 8;
        public const int CHARACTERANDDEAD = 9;
        public bool GameLost = false;
        public bool ResetTurn = false;
        public bool GameWon = false;
        public List<Zombie> ZombieList = [];
        public Player Character;
        public int Turns = 0;
        
        public Grid(int rows = 14, int columns = 18, double percentZombies = .1, double percentBarriers = .2, bool initialize = true)
        {           
            if (initialize)
            {
                GridMap = [];
                ChanceToPlace = new Random();
                InitializeGrid(rows, columns);
                PlaceAllPieces(percentZombies, percentBarriers);
                Character = new Player(0, 0);
            }
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
            
            GridMap[GridMap.Count - 1][GridMap[0].Count - 1].PieceType = ESCAPE;
            double randomNumber;
            for (int i = 1; i < GridMap.Count - 1; i++)
            {
                for (int j = 1; j < GridMap[i].Count - 1; j++)
                {
                    randomNumber = ChanceToPlace.NextDouble();
                    if (randomNumber <= percentBarriers)
                    {
                        GridMap[i][j].PieceType = BARRIER;
                    }
                    else if (randomNumber <= percentZombies + percentBarriers)
                    {
                        if ((i <= 4 && j <= 4) || i <= 1 || j <= 1 )
                        {
                            continue;
                        }
                        Zombie zombie = new Zombie(i, j);
                        ZombieList.Add(zombie);
                        GridMap[i][j].PieceType = ZOMBIE;
                    }
                }
            }
            GridMap[0][0].PieceType = CHARACTER;
            GridMap[2][2].PieceType = BARRIER;
            GridMap[1][2].PieceType = BARRIER;
            GridMap[2][1].PieceType = BARRIER;
            GridMap[1][1].PieceType = OPEN;
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

        public int DisplayGrid()
        {
            int iterationCounter = 0;
            Console.WriteLine();
            Console.Write("  ");
            for (int i = 0; i < GridMap[0].Count; i++)
            {
               Console.Write("__");
            }
            Console.WriteLine("_");
            string characterOrEmpty = " ";
            var rowShade = ConsoleColor.DarkGreen;
            for (int i = 0; i < GridMap.Count; i++)
            {
                Console.Write("  |");
                string spacing = " ";
                for (int j = 0; j < GridMap[i].Count; j++)
                {
                    iterationCounter += 1;
                    if (j == GridMap[i].Count - 1)
                    {
                        spacing = "";
                    }
                    GridSquare square = GridMap[i][j];
                    int status = square.PieceType;
                    switch (status)
                    {
                        case OPEN:
                            Console.ForegroundColor = rowShade;
                            Console.Write($" {spacing}");
                            Console.ResetColor();
                            break;
                        case BARRIER:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write($"0{spacing}");
                            Console.ResetColor();
                            break;
                        case ZOMBIE:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write($"*{spacing}");
                            Console.ResetColor();
                            break;
                        case OPENANDDEAD: case JUSTDIEDANDWILLDIE:
                            GridMap[i][j].PieceType = OPENANDDEAD;
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.Write($" {spacing}");
                            Console.ResetColor();
                            break;
                        case ZOMBIEANDDEAD:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.Write($"*{spacing}");
                            Console.ResetColor();
                            break;
                        case CHARACTER:
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write($"P{spacing}");
                            Console.ResetColor();
                            break;
                        case CHARACTERANDDEAD:
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write($"P{spacing}");
                            Console.ResetColor();
                            break;
                        case ESCAPE:
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            if (GameWon)
                            {
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.ForegroundColor = ConsoleColor.Blue;
                                characterOrEmpty = "P";
                            }
                            Console.Write($"{characterOrEmpty}{spacing}");
                            Console.ResetColor();
                            break;
                        case ZOMBIEANDPLAYERKILLED:
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write($"*{spacing}");
                            Console.ResetColor();
                            break;
                        default:
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
            return iterationCounter;
        }

        public bool MoveCharacter(int rowMove, int colMove, bool test = false)
        {
            int row = Character.row + rowMove;
            int column = Character.column + colMove;
            if (!IsValidCoord(row, column))
            {
                if (!test)
                {
                    Console.WriteLine("You have tried to move out of bounds. Try again");
                    Thread.Sleep(1000);
                }
                ResetTurn = true;
                return false;
            }
            GridSquare square = GridMap[row][column];
            switch (square.PieceType)
            {
                case OPEN:
                    GridMap[Character.row][Character.column].PieceType = OPEN;
                    Character.row = row;
                    Character.column = column;
                    GridMap[row][column].PieceType = CHARACTER;
                    break;
                case OPENANDDEAD:
                    GridMap[Character.row][Character.column].PieceType = OPEN;
                    Character.row = row;
                    Character.column = column;
                    GridMap[row][column].PieceType = CHARACTERANDDEAD;
                    break;
                case BARRIER:
                    if (!test)
                    {
                        Console.WriteLine("You cannot move into a barrier. Try again");
                        Thread.Sleep(1000);
                    }
                    ResetTurn = true;
                    return false;
                case ZOMBIE:
                    GameLost = true;
                    GridMap[Character.row][Character.column].PieceType = OPEN;
                    Character.row = row;
                    Character.column = column;
                    GridMap[row][column].PieceType = ZOMBIEANDPLAYERKILLED;
                    break;
                case ESCAPE:
                    GridMap[Character.row][Character.column].PieceType = OPEN;
                    GameWon = true;
                    Character.row = row;
                    Character.column = column;
                    break;
            }
            return true;
        }

        public bool MoveZombie(Zombie zombie)
        {
            int row = zombie.Row;
            int column = zombie.Column;
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
                return false;
            }
            GridSquare oldSquare = GridMap[zombie.Row][zombie.Column];
            int pieceType = GridMap[row][column].PieceType;
            if (pieceType != BARRIER && pieceType != ESCAPE)
            {
                if (oldSquare.PieceType == ZOMBIE)
                {
                    oldSquare.PieceType = OPEN;
                }
                else if (oldSquare.PieceType == ZOMBIEANDDEAD)
                {
                    oldSquare.PieceType = OPENANDDEAD;
                }
                else if (oldSquare.PieceType == JUSTDIEDANDWILLDIE)
                {
                    oldSquare.PieceType = JUSTDIEDANDWILLDIE;
                }
               // else
               // {
               //     
               //     //throw (new Exception());
               // }
            }
            switch (pieceType)
            {
                case BARRIER:
                case ESCAPE:
                    return false;
                case OPEN:
                    zombie.Row = row;
                    zombie.Column = column;
                    GridMap[row][column].PieceType = ZOMBIE;
                    break;
                case ZOMBIE:
                case JUSTDIEDANDWILLDIE:
                case ZOMBIEANDDEAD:
                    FindAndKillZombie(row, column);
                    zombie.Row = row;
                    zombie.Column = column;
                    zombie.IsAlive = false;
                    GridMap[row][column].PieceType = JUSTDIEDANDWILLDIE;
                    break;
                case OPENANDDEAD:
                    zombie.Row = row;
                    zombie.Column = column;
                    GridMap[row][column].PieceType = ZOMBIEANDDEAD;
                    break;
                case CHARACTER:
                    zombie.Row = row;
                    zombie.Column = column;
                    GridMap[row][column].PieceType = ZOMBIEANDPLAYERKILLED;
                    GameLost = true;
                    break;
            }
            return true;

        }

        public bool MoveAllZombies()
        {
            Zombie zombie;
            bool zombiesMoved = false;
            for (int i = 0; i < ZombieList.Count; i++)
            {
                zombie = ZombieList[i];
                if (zombie.IsAlive)
                {
                    if (MoveZombie(zombie))
                    {
                        zombiesMoved = true;
                    }
                }
            }
            return zombiesMoved;
        }

        public void FindAndKillZombie(int row, int column)
        {
            for (int i = 0; i < ZombieList.Count; i++)
            {
                Zombie zombie = ZombieList[i];
                if (zombie.Row == row && zombie.Column == column && zombie.IsAlive)
                {
                    zombie.IsAlive = false;
                    return;
                }
            }
        }

        public Grid Clone()
        {
            Grid newGrid = new Grid(initialize: false);

            newGrid.GridMap = new List<List<GridSquare>>();
            foreach (var row in this.GridMap)
            {
                var newRow = new List<GridSquare>();
                foreach (var square in row)
                {
                    newRow.Add(square.Clone());
                }
                newGrid.GridMap.Add(newRow);
            }

            newGrid.Character = this.Character.Clone();

            newGrid.ZombieList = new List<Zombie>();

            foreach ( var zombie in this.ZombieList)
            {
                newGrid.ZombieList.Add(zombie.Clone());
            }
            return newGrid;
        }
    }
}

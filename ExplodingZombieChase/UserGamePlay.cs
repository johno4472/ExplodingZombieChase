using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingZombieChase
{
    public class UserGamePlay
    {
        public int numRows = 14;
        public int numCols = 18;
        public double zombieDensity = .15;
        public double barrierDensity = .29;
        bool leaveGame = false;
        bool checkIfCanWin = false;
        bool checkIfWillDie = false;
        public int rowMove;
        public int colMove;
        WinPossibility possible = new WinPossibility();

        public void AllowUserToCustomize()
        {
            Console.WriteLine("Do you want to customize the game (c) or play default (d)?");
            string response;
            bool beginGameplay = false;
            string customizeResponse = Console.ReadLine() ?? "";
            switch (customizeResponse)
            {
                case "d":
                    Console.WriteLine("Playing default gameplay");
                    break;
                case "c":
                    while (true)
                    {
                        Console.WriteLine($"Your game setup has:\nRows: {numRows}\nColumns: {numCols}\nZombie density: {zombieDensity}\nBarrier density: {barrierDensity}");
                        Console.WriteLine("Do you want to play with those settings? (y) or customize something? (r, c, z, or b)");
                        response = Console.ReadLine() ?? "";
                        try
                        {
                            switch (response)
                            {
                                case "y":
                                    beginGameplay = true;
                                    break;
                                case "r":
                                    Console.WriteLine("How many rows do you want? (5-50)");
                                    numRows = Convert.ToInt32(Console.ReadLine());
                                    break;
                                case "c":
                                    Console.WriteLine("How many rows do you want? (5-30)");
                                    numCols = Convert.ToInt32(Console.ReadLine());
                                    break;
                                case "z":
                                    Console.WriteLine("What zombie density do you want? (0-1)");
                                    zombieDensity = Convert.ToDouble(Console.ReadLine());
                                    break;
                                case "b":
                                    Console.WriteLine("What barrier density do you want? (0-1)");
                                    barrierDensity = Convert.ToDouble(Console.ReadLine());
                                    break;
                                default:
                                    Console.WriteLine("Invalid response. Type only one of the following: y, r, c, z, b");
                                    Thread.Sleep(500);
                                    break;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("You gave an invalide numerical response. Look at the limits and try again");
                        }
                        if (beginGameplay)
                        {
                            break;
                        }
                    }
                    break;
                default:
                    Console.WriteLine("No valid answer given so we're setting you up with default settings");
                    break;
            }
        }

        public void GetPlayerMove(Grid grid)
        {
            leaveGame = false;
            string moveResponse = "";
            grid.ResetTurn = false;
            Console.WriteLine("Your move?");
            moveResponse = Console.ReadLine() ?? "";
            rowMove = 0;
            colMove = 0;
            switch (moveResponse)
            {
                case "w":
                    rowMove = -1;
                    break;
                case "s":
                    rowMove = 1;
                    break;
                case "a":
                    colMove = -1;
                    break;
                case "d":
                    colMove = 1;
                    break;
                case "":
                    break;
                case "q":
                    leaveGame = true;
                    break;
                case "pw":
                    CheckIfCanWin(grid);
                    break;
                case "pd":
                    CheckIfWillDie(grid);
                    break;
                default:
                    Console.WriteLine("Invalid entry. Enter w, a, s, d, or (just hit enter).");
                    grid.ResetTurn = true;
                    Thread.Sleep(1000);
                    break;
            }
        }
        
        public void CheckIfCanWin(Grid grid)
        {
            if (possible.CheckIfPossibleToWin(grid))
            {
                Console.WriteLine("It's still possible to win in less than 30 turns!");
                possible.PrintSuccessPath();
            }
            else
            {
                Console.WriteLine("Not possible to win in 30 turns");
            }
            grid.ResetTurn = true;
            return;
        }

        public void CheckIfWillDie(Grid grid)
        {
            if (possible.CheckIfPossibleToWin(grid, true))
            {
                Console.WriteLine("It's still possible to not be dead in less than 30 turns!");
                possible.PrintSuccessPath();
            }
            else
            {
                Console.WriteLine("Not possible to not be dead in 30 turns, considering you don't sit and skip turns while zombies don't move");
            }
            grid.ResetTurn = true;
            return;
        }

        public bool ExecutePlayerTurn(Grid grid)
        {
            if (leaveGame)
            {
                Console.WriteLine("Leaving game now");
                Thread.Sleep(1000);
                return true;
            }
            if (!grid.ResetTurn)
            {
                grid.MoveCharacter(rowMove, colMove);
                grid.DisplayGrid();
                if (grid.GameWon)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You have successfully escaped!");
                    Thread.Sleep(1000);
                    Console.ResetColor();
                    return true;
                }
                else if (grid.GameLost)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You've hit a zombie! You died and your guts exploded everywhere");
                    Thread.Sleep(1000);
                    Console.ResetColor();
                    return true;
                }
                if (!grid.ResetTurn)
                {
                    grid.MoveAllZombies();
                    grid.DisplayGrid();
                    grid.Turns += 1;
                }
                if (grid.GameLost)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You've hit a zombie! You died and your guts exploded everywhere");
                    Thread.Sleep(1000);
                    Console.ResetColor();
                    return true;
                }
            }
            return false;
        }

        public bool PlayAgain()
        {
            Console.WriteLine("Want to play again? y/n");
            string playAgain = Console.ReadLine() ?? "";
            if (playAgain == "y")
            {
                Console.WriteLine("Okay let's give it another go!");
            }
            else if (playAgain == "n")
            {
                Console.WriteLine("Got it. Shutting down now");
                return false;
            }
            else
            {
                Console.WriteLine("Didn't give an appropriate answer, so I'll take that as a yes");
            }
            return true;
        }

        public void Introduction()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            string instructions = "Here's how Exploding Zombie Chase works:\n" +
                "You wake up in a sketchy warehouse somewhere, and just need to make your way to the escape hatch at the opposite corner.\n" +
                "There's just one problem...\n\nZombies!!\n\n" +
                "Between you and the hatch are multiple zombies that can sense your every move, and move closer to you every second.\n" +
                "Your disadvantage: You can only move up horizontally or vertically, and zombies can move diagonally.\n" +
                "Your advantage: You know exactly what the zombies will do. And this is how:\n" +
                "- Zombies will ALWAYS try to move diagonally in your direction, unless you are exactly in line with them vertically\n " +
                "  or horizontally, in that case they will move one space in your direction\n" +
                "- That means that if a barrier stands between the zombie's path to you, it will stand there and wait until the \n" +
                "  path to you no longer is on the other side of a barrier\n" +
                "- Also, these are exploding zombies! If two zombies collide on their way to you, they explode!\n" +
                "\n See if you can survive the zombies, and get them to explode so they open up a path to safety!\n" +
                "Basic controls are:\nw - up\na - left\nd - right\ns - down\nq - quit.\n\nBest of luck to you!";

            Console.WriteLine(instructions);
        }
    }
}

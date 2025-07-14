// See https://aka.ms/new-console-template for more information
using ExplodingZombieChase;

Console.ResetColor();
Console.WriteLine("Hello, World!");

Console.WriteLine("Do you want to customize the game (c) or play default (d)?");
int numRows = 14;
int numCols = 18;
double zombieDensity = .1;
double barrierDensity = .2;
string response;
bool beginGameplay = false;
string customizeResponse = Console.ReadLine();
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

Grid game = new Grid(numRows, numCols, zombieDensity, barrierDensity);
string moveResponse = "";
int rowMove;
int colMove;
game.DisplayGrid();
bool leaveGame = false;

while (true)
{
    game.ResetTurn = false;
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
        default:
            Console.WriteLine("Invalid entry. Enter w, a, s, d, or (just hit enter).");
            game.ResetTurn = true;
            Thread.Sleep(1000);
            break;
    }
    if (leaveGame)
    {
        Console.WriteLine("Leaving game now");
        Thread.Sleep(1000);
        break;
    }
    if (!game.ResetTurn)
    {
        game.MoveCharacter(rowMove, colMove);
        game.DisplayGrid();
        if (game.GameWon)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You have successfully escaped!");
            Thread.Sleep(1000);
            Console.ResetColor();
            break;
        } 
        else if (game.GameLost)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You've hit a zombie! You died and your guts exploded everywhere");
            Thread.Sleep(1000);
            Console.ResetColor();
            break;
        }
        if (!game.ResetTurn)
        {
            game.MoveAllZombies();
            game.DisplayGrid();
        }
        if (game.GameLost)
        {
            Console.WriteLine("You've hit a zombie! You died and your guts exploded everywhere");
            Thread.Sleep(1000);
            break;
        }
    }
    
}




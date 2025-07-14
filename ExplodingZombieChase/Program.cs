// See https://aka.ms/new-console-template for more information
using ExplodingZombieChase;

Console.ResetColor();
Console.WriteLine("Hello, World!");

Grid game = new Grid(14, 18, .01, .2);
string moveResponse = "";
int rowMove;
int colMove;
game.DisplayGrid();

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
        default:
            Console.WriteLine("Invalid entry. Enter w, a, s, or d");
            Thread.Sleep(1000);
            break;
    }
    if (!game.ResetTurn)
    {
        game.MoveCharacter(rowMove, colMove);
        game.DisplayGrid();
        if (game.GameWon)
        {
            Console.WriteLine("You have successfully escaped!");
            Thread.Sleep(1000);
            break;
        } 
        else if (game.GameLost)
        {
            Console.WriteLine("You've hit a zombie! You died and your guts exploded everywhere");
            Thread.Sleep(1000);
            break;
        }
            game.MoveAllZombies();
            game.DisplayGrid();
        if (game.GameLost)
        {
            Console.WriteLine("You've hit a zombie! You died and your guts exploded everywhere");
            Thread.Sleep(1000);
            break;
        }
    }
    
}




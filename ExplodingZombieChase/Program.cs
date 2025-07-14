// See https://aka.ms/new-console-template for more information
using ExplodingZombieChase;

Console.ResetColor();
Console.WriteLine("Hello, World!");

Grid game = new Grid(14, 18, .1, .15);
string moveResponse = "";
int rowMove;
int colMove;

while (true)
{
    game.DisplayGrid();
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
            break;
    }
    game.MoveCharacter(rowMove, colMove);
    game.DisplayGrid();
    game.MoveAllZombies();
}




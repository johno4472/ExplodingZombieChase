// See https://aka.ms/new-console-template for more information
using System.ComponentModel.Design;
using ExplodingZombieChase;




UserGamePlay gamePlay = new UserGamePlay();

gamePlay.Introduction();

while (true)
{
    gamePlay.AllowUserToCustomize();

    Grid grid = new Grid(gamePlay.numRows, gamePlay.numCols, gamePlay.zombieDensity, gamePlay.barrierDensity);
    
    grid.DisplayGrid();

    while (true)
    {
        gamePlay.GetPlayerMove(grid);

        if (gamePlay.ExecutePlayerTurn(grid))
        {
            break;
        }
    }
    if (!gamePlay.PlayAgain())
    {
        break;
    }
}





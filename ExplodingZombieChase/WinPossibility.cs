using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingZombieChase
{
    public class WinPossibility()
    {
        public int NumCols { get; set; }
        public int NumRows { get; set; }
        public List<string> SuccessPath = [];


        public bool PossibleToWin(Grid game, int rowMove, int colMove, int numIterations = 0, bool notDie = false, bool zombiesMustMove = false)
        {
            bool validMove = game.MoveCharacter(rowMove, colMove, true);
            if (!validMove)
            {
                return false;
            }
            else if (!notDie && numIterations + (NumRows - game.Character.row) + (NumCols - game.Character.column) >= 30)
            {
                return false;
            }
            else if (notDie && numIterations == 30)
            {
                return true;
            }
            if (game.GameWon == true)
            {
                return true;
            }
            if (!game.MoveAllZombies() && zombiesMustMove)
            {
                return false;
            }
            
            if (game.GameLost == true)
            {
                return false;
            }
            if (PossibleToWin(game.Clone(), 1, 0, numIterations + 1, notDie))
            {
                SuccessPath.Add("d");
                return true;
            }
            else if (PossibleToWin(game.Clone(), 0, 1, numIterations + 1, notDie))
            {
                SuccessPath.Add("r");
                return true;
            }
            else if (PossibleToWin(game.Clone(), 0, 0, numIterations + 1, notDie, true))
            {
                SuccessPath.Add("s");
                return true;
            }
            else if (PossibleToWin(game.Clone(), -1, 0, numIterations + 1, notDie))
            {
                SuccessPath.Add("u");
                return true;
            }
            else if (PossibleToWin(game.Clone(), 0, -1, numIterations + 1, notDie))
            {
                SuccessPath.Add("l");
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckIfPossibleToWin(Grid game, bool notDie = false)
        {
            NumRows = game.GridMap.Count;
            NumCols = game.GridMap[0].Count;
            if (PossibleToWin(game.Clone(), 1, 0, 0, notDie))
            {
                SuccessPath.Add("d");
                return true; 
            }
            if (PossibleToWin(game.Clone(), 0, 1, 0, notDie))
            {
                SuccessPath.Add("r");
                return true; 
            }
            if (PossibleToWin(game.Clone(), 0, 0, 0, notDie, true))
            {
                SuccessPath.Add("s");
                return true; 
            }
            if (PossibleToWin(game.Clone(), -1, 0, 0, notDie))
            {
                SuccessPath.Add("u");
                return true; 
            }
            if (PossibleToWin(game.Clone(), 0, -1, 0, notDie))
            {
                SuccessPath.Add("l");
                return true; 
            }
            return false;
        }

        public void PrintSuccessPath()
        {
            foreach (string move in SuccessPath)
            {
                Console.Write($"{move} ");
            }
            Console.WriteLine();
        }
    }
}

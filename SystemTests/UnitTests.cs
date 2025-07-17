using AwesomeAssertions;
using ExplodingZombieChase;
using Xunit.Sdk;

namespace SystemTests
{
    public class UnitTests
    {
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

        [Theory]
        [InlineData(0, -1)]
        [InlineData(-1, 0)]
        public void MoveOutOfBoundsNotPermitted(int rowMove, int colMove)
        {
            //Arrange
            Grid game = new Grid(14, 18);

            //Act
            bool failResult = game.MoveCharacter(rowMove, colMove);
            bool succeedResult = game.MoveCharacter(0, 0);

            //Assert
            failResult.Should().BeFalse();
            succeedResult.Should().BeTrue();
            game.ResetTurn.Should().BeTrue();
        }

        [Fact]
        public void HittingZombieEndsGame()
        {
            //Arrange
            Grid game = new Grid(14, 18);
            game.ZombieList.Add(new Zombie(0, 1));
            game.GridMap[0][1].PieceType = ZOMBIE;

            //Act
            bool move = game.MoveCharacter(0, 1);

            //Assert
            move.Should().BeTrue();
            game.GameLost.Should().BeTrue();
        }

        [Fact]
        public void HittingBarrierFailsAndResetsTurn()
        {
            //Arrange
            Grid game = new Grid(14, 18);
            game.GridMap[0][1].PieceType = BARRIER;

            //Act
            bool move = game.MoveCharacter(0, 1);
            bool goodMove = game.MoveCharacter(1, 0);

            //Assert
            move.Should().BeFalse();
            game.ResetTurn.Should().BeTrue();
            goodMove.Should().BeTrue();
        }

        [Fact]
        public void GridCloneSuccessful()
        {
            //Arrange
            Grid game = new Grid();
            game.Character.row = 7;
            game.ZombieList.Add(new Zombie(7, 9));
            Grid newGame;

            //Act
            newGame = game.Clone();
            game.Character.row = 5;
            game.ZombieList[^1].Row = 1;

            //Assert
            newGame.Character.row.Should().Be(7);
            newGame.ZombieList[^1].Row.Should().Be(7);
        }

        [Fact]
        public void ZombieHittingCharacterEndsGame()
        {
            //Arrange
            Grid game = new Grid(14, 18);
            game.ZombieList.Add(new Zombie(0, 1));
            game.GridMap[0][1].PieceType = ZOMBIE;

            //Act
            bool move = game.MoveAllZombies();

            //Assert
            move.Should().BeTrue();
            game.GameLost.Should().BeTrue();
        }

        [Fact]
        public void ZombiesCollidingKillsThem()
        {
            //Arrange
            Grid game = new Grid();
            Zombie zombie1 = new Zombie(0, 2);
            game.ZombieList.Add(zombie1);
            game.GridMap[0][2].PieceType = ZOMBIE;
            Zombie zombie2 = new Zombie(2, 2);
            game.ZombieList.Add(zombie2);
            game.GridMap[2][2].PieceType = ZOMBIE;
            game.Character.row = 1;
            game.GridMap[1][1].PieceType = OPEN;

            //Act
            game.MoveAllZombies();

            //Assert
            zombie1.IsAlive.Should().BeFalse();
            zombie1.Row.Should().Be(1);
            zombie1.Column.Should().Be(1);
            zombie2.IsAlive.Should().BeFalse();
            zombie2.Row.Should().Be(1);
            zombie2.Column.Should().Be(1);
        }

        [Fact]
        public void NoErrorsWhileDisplayingGrid()
        {
            //Arrange
            int numRows = 14;
            int numCols = 18;
            Grid game = new Grid(14, 18);

            //Act
            int iterations = game.DisplayGrid();

            //Assert
            iterations.Should().Be(numRows * numCols);
        }

        [Fact]
        public void WinPossibilityFailsWhenTooFar()
        {
            //Arrange
            Grid game = new Grid(14, 18);
            WinPossibility possible = new WinPossibility();

            //Act
            bool canWin = possible.CheckIfPossibleToWin(game);

            //Assert
            canWin.Should().BeFalse();
        }

        [Fact]
        public void WinPossibleIfByEscape()
        {
            //Arrange
            Grid game = new Grid(14, 18);
            game.Character.row = 12;
            game.Character.column = 17;
            WinPossibility possible = new WinPossibility();

            //Act
            bool canWin = possible.CheckIfPossibleToWin(game);

            //Assert
            canWin.Should().BeTrue();
        }

        [Fact]
        public void NeverHasToDieInBeginning()
        {
            //Arrange
            Grid game = new Grid(14, 18);
            WinPossibility possible = new WinPossibility();

            //Act
            bool canWin = possible.CheckIfPossibleToWin(game, true);

            //Assert
            canWin.Should().BeTrue();
        }

        [Fact]
        public void WinPossibleIfAllDirectionsNeeded()
        {
            //Arrange
            Grid game = new Grid(14, 18, percentZombies: 0, percentBarriers: 0);
            game.Character.row = 11;
            game.Character.column = 17;
            game.ZombieList.Clear();
            game.GridMap[12][17].PieceType = BARRIER;
            game.GridMap[12][16].PieceType = BARRIER;
            game.GridMap[11][16].PieceType = BARRIER;
            game.GridMap[9][17].PieceType = BARRIER;
            game.GridMap[9][16].PieceType = BARRIER;
            game.GridMap[9][15].PieceType = BARRIER;
            game.GridMap[10][14].PieceType = BARRIER;
            game.GridMap[11][14].PieceType = BARRIER;
            game.GridMap[12][14].PieceType = BARRIER;
            game.GridMap[13][14].PieceType = BARRIER;
            game.DisplayGrid();
            WinPossibility possible = new WinPossibility();

            //Act
            bool canWin = possible.CheckIfPossibleToWin(game);

            //Assert
            canWin.Should().BeTrue();
        }
    }
}
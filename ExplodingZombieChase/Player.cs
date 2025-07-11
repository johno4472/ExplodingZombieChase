using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingZombieChase
{
    public class Player
    {
        public int row;
        public int column;
        public const int OPEN = 0;
        public const int CHARACTER = 1;

        public Player(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }
}

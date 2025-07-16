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

        public Player(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public Player Clone()
        {
            return new Player(this.row, this.column);
        }
    }
}

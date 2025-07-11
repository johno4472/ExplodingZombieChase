using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingZombieChase
{
    public class Zombie
    {
        public int row {  get; set; }
        public int column { get; set; }
        public bool isAlive { get; set; }

        public Zombie(int row, int column, bool isAlive = true) 
        {
            this.row = row;
            this.column = column;
            this.isAlive = isAlive;
        }
    }
}

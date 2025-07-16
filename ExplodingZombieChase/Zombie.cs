using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingZombieChase
{
    public class Zombie
    {
        public int Row {  get; set; }
        public int Column { get; set; }
        public bool IsAlive { get; set; }

        public Zombie(int row, int column, bool isAlive = true) 
        {
            this.Row = row;
            this.Column = column;
            this.IsAlive = isAlive;
        }

        public Zombie Clone()
        {
            return new Zombie(Row, Column, IsAlive);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingZombieChase
{
    public class GridSquare
    {

        //0 - empty, 1 - character present, 2 - block, 3 - zombie, 4- escape, 5 - open and dead, 6 - open and killed; all set to empty
        public int PieceType { get; set; } = 0;

        public bool DeadZombiePresent { get; set; } = false;
    }
}

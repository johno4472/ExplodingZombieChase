using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingZombieChase
{
    public class GridSquare
    {

        //0 - empty, 1 - character present, 2 - block, 3 - zombie; all set to empty
        public int Status { get; set; } = 0;
    }
}

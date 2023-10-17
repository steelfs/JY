using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testris;

namespace Tetris
{
    internal class Cell
    {
        public CellType cellType;
        int x;
        int y;
        public Cell(int x, int y )
        {
            this.x = x;
            this.y = y;
        }
    }
}

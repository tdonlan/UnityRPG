using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SimpleRPG2
{
    public class Tile
    {
        public char TileChar { get; set; }
        public char TempChar { get; set; }
        public bool empty { get; set; }
        public int x { get; set; }
        public int y { get; set; }


        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
            TileChar = '.';
            empty = true;
        }

        public override string ToString()
        {
            return TileChar.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
    public class Tile
    {

        public static float TILE_SIZE = 2f;

        public char TileChar { get; set; }
        public char TempChar { get; set; }

        public bool empty { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public string tileSheetName { get; set; }
        public int tileSpriteIndex { get; set; }

        public string tempSheetName { get; set; }
        public int tempSpriteIndex { get; set; }


        public TileSpriteLookup tileSpriteLookup { get; set; }
             


        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
            TileChar = '.';
            empty = true;
        }

        public Tile(int x, int y, bool empty)
        {
            this.x = x;
            this.y = y;
            this.empty = empty;
        }

        public override string ToString()
        {
            return TileChar.ToString();
        }
    }
}

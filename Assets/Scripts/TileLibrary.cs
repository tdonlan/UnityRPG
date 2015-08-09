using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityRPG.Scripts
{

    public class TileSpriteLookup
    {
        public char tileChar { get; set; }
        public string tileName { get; set; }
        public string spritesheetName { get; set; }
        public int spritesheetIndex { get; set; }
        public bool isSolid { get; set; }
        public TileSpriteType tileSpriteType { get; set; }

    }

    public class TileLibrary
    {
        public List<TileSpriteLookup> tileSpriteLookupList { get; set; }
        public string levelName { get; set; }

        public TileLibrary()
        {

        }
    }


}

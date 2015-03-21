using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityRPG
{

    public class BoardMetaData
    {
        //list of enemies
        //list of loot, etc
        //where to after completion / update quest data, etc
    }

    public class BoardData
    {
        public int size { get; set; } //currently only square boards
        public char[,] boardTileData { get; set; }
        public List<TileSpriteLookup> tileSpriteLookupList { get; set; }
        public string levelName {get;set;}
        public BoardMetaData boardMetaData { get; set; }

        //store where to go once battle is complete
        //references to enemies

        public BoardData(char[,] boardTileData, List<TileSpriteLookup> tileSpriteLookupList, string name, BoardMetaData metaData)
        {
            this.boardTileData = boardTileData;
            this.tileSpriteLookupList = tileSpriteLookupList;
            this.levelName = name;
            this.boardMetaData = metaData;
        }

        //Need error handling if we dont find
        public TileSpriteLookup getTileSpriteData(char c)
        {
            var tileSpriteData = (from data in tileSpriteLookupList
                                  where data.tileChar == c
                                  select data).FirstOrDefault();

            return tileSpriteData;
        }
        
    }

   
}

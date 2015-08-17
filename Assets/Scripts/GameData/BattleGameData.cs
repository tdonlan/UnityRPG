using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

//JSON.NET NOT PART OF UNITY.  NEED TO PURCHASE, OR FIND ANOTHER SOLUTION FOR UNITY.

namespace UnityRPG
{
    public class BattleGameData
    {
        public AssetLibrary assetLibrary { get; set; }

        public List<GameCharacter> gameCharacterList { get; set; }
        public TileMapData tileMapData { get; set; }

        public Board gameBoard { get; set; }

        public BattleGameData()
        {
            assetLibrary = new AssetLibrary();

            gameCharacterList = new List<GameCharacter>();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace UnityRPG
{
    public class BattleGameData
    {

        public List<GameCharacter> gameCharacterList { get; set; }
        public TileMapData tileMapData { get; set; }

        public Board gameBoard { get; set; }

        public BattleGameData()
        {
            gameCharacterList = new List<GameCharacter>();
        }

    }
}

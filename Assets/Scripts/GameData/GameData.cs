using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

//JSON.NET NOT PART OF UNITY.  NEED TO PURCHASE, OR FIND ANOTHER SOLUTION FOR UNITY.

namespace UnityRPG
{
    public class GameData
    {
        public Dictionary<string, BoardData> BoardDataDictionary { get; set; }

        public AssetLibrary assetLibrary { get; set; }

        public Dictionary<string, string> fileManifest { get; set; }
        public List<Ability> masterAbilityList { get; set; }
        public List<Item> masterItemList { get; set; }

        public List<GameCharacter> gameCharacterList { get; set; }
        public Board gameBoard { get; set; }

        public GameData()
        {
            assetLibrary = new AssetLibrary();

            LoadBoardDataDictionary();

            masterAbilityList = new List<Ability>();
            masterItemList = new List<Item>();
            gameCharacterList = new List<GameCharacter>();
        }


        public void LoadBoardDataDictionary()
        {
            BoardDataDictionary = new Dictionary<string, BoardData>();
            BoardDataDictionary.Add("Board1", new BoardData("Map1", "Dungeon", "Board1", null));
        }


    }
}

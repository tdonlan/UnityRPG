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
           
            //load manifest dictionary
            //string manifestPath = @"DataFiles\manifest.json";
            //LoadManifest(manifestPath);

            //load master list from json files
        }

        public void LoadManifest(string manifestPath)
        {
            fileManifest = LoadJsonToDictionary(manifestPath);

            foreach(var key in fileManifest.Keys)
            {
                LoadList((MasterListType)Enum.Parse(typeof(MasterListType), key), fileManifest[key]);
            }

        }

        public void LoadBoardDataDictionary()
        {
            BoardDataDictionary = new Dictionary<string, BoardData>();
            BoardDataDictionary.Add("Board1", new BoardData("Map1", "Dungeon", "Board1", null));
        }

        public void LoadList(MasterListType type, string path)
        {
            path = string.Format(@"DataFiles\{0}",path);
            switch(type)
            {
                case MasterListType.Abilities:
                   // masterAbilityList = JsonConvert.DeserializeObject<List<Ability>>(File.ReadAllText(path));
                    break;
                case MasterListType.Characters:
                    break;
                case MasterListType.Items:
                    break;
                case MasterListType.Maps:
                    break;
                default:
                    break;
            }
        }

        private object LoadFromJson(string path)
        {
           // string jsonStr = File.ReadAllText(path);

           // return JsonConvert.DeserializeObject(jsonStr);

            return null;
        }

        private Dictionary<string,string> LoadJsonToDictionary(string path)
        {
            //string jsonStr = File.ReadAllText(path);
           // return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr);

            return null;

        }

    }
}

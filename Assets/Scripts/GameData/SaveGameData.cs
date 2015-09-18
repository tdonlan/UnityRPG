using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityRPG;


    [Serializable]
    public class SaveGameData
    {
        public DateTime timestamp { get; set; }
        public GameCharacter playerGameCharacter {get;set;}
        public List<GameCharacter> partyList { get; set; }
        public GlobalFlags globalFlags { get; set; }
        public long treeLink { get; set; }

        public SaveGameData()
        {
            timestamp = DateTime.Now;
        }
    }


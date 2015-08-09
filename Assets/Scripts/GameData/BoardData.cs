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
        public string boardLayoutName { get; set; }
        public string tileLookupName { get; set; }
        public string levelName {get;set;}
        public BoardMetaData boardMetaData { get; set; }

        //store where to go once battle is complete
        //references to enemies

        public BoardData(string layoutName, string tileLookupName, string levelName, BoardMetaData metaData)
        {
            this.boardLayoutName = layoutName;
            this.tileLookupName = tileLookupName;
            this.levelName = levelName;
            this.boardMetaData = metaData;
        }
    }

   
}

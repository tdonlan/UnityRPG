using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG

{
    public class BattleFactory
    {

        public static BattleGameData getBattleGameDataFromZoneTree(List<GameCharacter> playerCharacterList, BattleTree battleTree, GameDataSet gameDataSet, TileMapData tileMapData)
        {
            BattleGameData retval = new BattleGameData();

            retval.tileMapData = tileMapData;

            //load player

            retval.gameCharacterList.AddRange(playerCharacterList);

            //load enemies
            foreach (var enemyNode in battleTree.getEnemyNodeList())
            {
                if(gameDataSet.gameCharacterDataDictionary.ContainsKey(enemyNode.content.linkIndex)){
                       var enemyData = gameDataSet.gameCharacterDataDictionary[enemyNode.content.linkIndex];
                       retval.gameCharacterList.Add(CharacterFactory.getGameCharacterFromGameCharacterData(enemyData, gameDataSet));
                }
             
            }
            return retval;
        }
    }
}

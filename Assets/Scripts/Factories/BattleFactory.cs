using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG

{
    public class BattleFactory
    {
        //DEPRECATED
        public static BattleGameData getGameData(int battleindex, Random r)
        {
            BattleGameData retval = new BattleGameData();
            switch(battleindex)
            {
                case 0:
                    retval.gameCharacterList = getCharacterList(new List<string>(){"Warrior","Priest","Rogue","Goblin","Goblin","Goblin","Goblin"},r);
                    break;
                case 1:
                    retval.gameCharacterList = getCharacterList(new List<string>() { "Warrior", "Priest", "Rogue", "Mage", "EnemyWarrior", "EnemyPriest", "EnemyMage", "EnemyArcher" }, r);
                    break;
                case 2:
                    retval.gameCharacterList = getCharacterList(new List<string>() { "Warrior", "Priest", "Rogue","Mage","Archer", "Dragon" }, r);
                    break;
                default:
                    break;
            }

            return retval;
        }

        public static List<GameCharacter> getBattleGameCharacterList(int battleIndex, Random r)
        {
            switch (battleIndex)
            {
                case 0:
                    return getCharacterList(new List<string>() { "Warrior", "Priest", "Rogue", "Goblin", "Goblin", "Goblin", "Goblin" }, r);
                   
                case 1:
                    return getCharacterList(new List<string>() { "Warrior", "Priest", "Rogue", "Mage", "EnemyWarrior", "EnemyPriest", "EnemyMage", "EnemyArcher" }, r);
              
                case 2:
                    return getCharacterList(new List<string>() { "Warrior", "Priest", "Rogue", "Mage", "Archer", "Dragon" }, r);
                  
                default:
                    return null;
            }
        }

        //given a list of character names, return a list of GameCharacters
        private static List<GameCharacter> getCharacterList(List<string> characterNames,Random r)
        {
            List<GameCharacter> retval = new List<GameCharacter>();
            foreach(var name in characterNames)
            {
                switch(name)
                {
                    case "Warrior":
                        retval.Add(CharacterFactory.getWarrior(r));
                        break;
                    case "Rogue":
                        retval.Add(CharacterFactory.getRogue(r));
                        break;
                    case "Mage":
                        retval.Add(CharacterFactory.getMage(r));
                        break; 
                    case "Priest":
                        retval.Add(CharacterFactory.getPriest(r));
                        break;
                    case "Archer":
                        retval.Add(CharacterFactory.getArcher(r));
                        break;
                    case "Goblin":
                        retval.Add(CharacterFactory.getGoblin(r));
                        break;
                    case "EnemyWarrior":
                        retval.Add(CharacterFactory.getEnemyWarrior(r));
                        break;
                    case "EnemyMage":
                        retval.Add(CharacterFactory.getEnemyMage(r));
                        break;
                    case "EnemyPriest":
                        retval.Add(CharacterFactory.getEnemyPriest(r));
                        break;
                    case "EnemyArcher":
                        retval.Add(CharacterFactory.getEnemyArcher(r));
                        break;
                    case "Dragon":
                        retval.Add(CharacterFactory.getDragon(r));
                        break;
                    default:
                        break;
                }
            }

            return retval;
        }
    }
}

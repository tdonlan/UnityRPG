using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SimpleRPG2
{
    public class AIActor
    {
        public GameCharacter character {get;set;}
        public EnemyType enemyType { get; set; }
        public Dictionary<AIActionType, int> actionWeightDictionary { get; set; } //static for life of character
        public List<AIAction> AIActionList { get; set; } //updates every turn

        public AIActor(GameCharacter character, EnemyType type)
        {
            this.character = character;
            this.enemyType = type;

            InitActionWeight();

        }

        private void InitActionWeight()
        {
            actionWeightDictionary = AIFactory.getEnemyActionDictionary(enemyType);
        }


        public List<BattleAction> getBattleActionList(BattleGame game)
        {
            List<BattleAction> retval = new List<BattleAction>();

            //reset the AIActionList
            AIActionList = new List<AIAction>();
            //update the weight and cost lists
            foreach(var type in actionWeightDictionary.Keys)
            {
                AIActionList.AddRange(getAIActions(game, type));
            }

            if (AIActionList.Count > 0)
            {
                AIActionList.Sort((x1, x2) => (x1.cost * (100 - x1.weight)).CompareTo(x2.cost * (100 - x2.weight)));

                //get the top weighted action
                retval.AddRange(AIActionList[0].battleActionList);
            }

            return retval;
       
        }

        private List<AIAction> getAIActions(BattleGame game, AIActionType type)
        {
            List<AIAction> retvalList = new List<AIAction>();
            switch(type)
            {
                case AIActionType.Attack:
                    retvalList.AddRange(getAIAttackActions(game));
                    break;
                case AIActionType.RangedAttack:
                    retvalList.AddRange(getAIRangedAttackActions(game));
                    break;
                case AIActionType.Heal:
                    retvalList.AddRange(getAIHealActions(game));
                    break;
                default:
                    break;
            }
            return retvalList;
        }

        #region WeightHeuristics

        //function of current health + default heal weight
        private int getAIHealWeight(BattleGame game)
        {
            int curWeight = 100 - ((character.hp / character.totalHP) * 100);
            return (int)Math.Round((actionWeightDictionary[AIActionType.Heal] + (float)curWeight) / 2);
        }

        #endregion

        #region CostHeuristics

        //if we have a melee weapon, calculate nearest enemy + weapon ap
        private List<AIAction> getAIAttackActions(BattleGame game)
        {
            List<AIAction> aiActionList = new List<AIAction>();
            if (character.weapon != null)
            {
                if (character.weapon.weaponType == WeaponType.OneHandMelee || character.weapon.weaponType == WeaponType.TwoHandMelee)
                {
                    GameCharacter targetCharacter = AI.findNearestPlayer(character, game.board, game.characterList);

                    List<Point> pointList = PathFind.Pathfind(game.board, character.x, character.y, targetCharacter.x, targetCharacter.y);
                    pointList.RemoveAt(0); //remove the character from pathfind.
                    pointList.RemoveAt(pointList.Count - 1); //remove the target from pathfind.

                    int dist = pointList.Count;

                    int cost = dist + character.weapon.actionPoints;

                    List<BattleAction> battleActionList = new List<BattleAction>();

                    foreach (var p in pointList)
                    {
                        battleActionList.Add(new BattleAction() { character = character, actionType = BattleActionType.Move, targetTile = game.board.getTileFromPoint(p) });
                    }

                    battleActionList.Add(new BattleAction() { character = character, targetCharacter = targetCharacter, targetTile = game.board.getTileFromLocation(targetCharacter.x, targetCharacter.y), actionType = BattleActionType.Attack });
                    
                    
                    aiActionList.Add(new AIAction() {actionType=AIActionType.Attack,cost=cost,battleActionList=battleActionList });
                }
            }

            return aiActionList;
        }

        //same as Melee attack, but add movement until we are LOS to nearest enemy
        private List<AIAction> getAIRangedAttackActions(BattleGame game)
        {
            List<AIAction> aiActionList = new List<AIAction>();
            if (character.weapon != null)
            {
                if (character.weapon.weaponType == WeaponType.OneHandRanged || character.weapon.weaponType == WeaponType.TwoHandRanged)
                {
                   
                    //should actually find who has easiest LOS
                    GameCharacter targetCharacter = AI.findNearestPlayer(character, game.board, game.characterList);

                    Tile characterTile = game.board.getTileFromLocation(character.x, character.y);
                    Tile targetTile = game.board.getTileFromLocation(targetCharacter.x, targetCharacter.y);
                    List<Point> pointList = game.board.getPathToLOS(characterTile, targetTile);

                    int dist = pointList.Count;

                    int cost = dist + character.weapon.actionPoints;

                    List<BattleAction> battleActionList = new List<BattleAction>();

                    foreach (var p in pointList)
                    {
                        battleActionList.Add(new BattleAction() { character = character, actionType = BattleActionType.Move, targetTile = game.board.getTileFromPoint(p) });
                    }

                    battleActionList.Add(new BattleAction() { character = character, targetCharacter = targetCharacter, targetTile = game.board.getTileFromLocation(targetCharacter.x, targetCharacter.y), actionType = BattleActionType.RangedAttack });


                    aiActionList.Add(new AIAction() { actionType = AIActionType.Attack, cost = cost, battleActionList = battleActionList });
                }
            }

            return aiActionList;
        }

        //get a list of all healing abilities / items.  
        //currently only abilities that are for self
        private List<AIAction> getAIHealActions(BattleGame game)
        {
         
            List<AIAction> aiActionList = new List<AIAction>();
          
                Tile targetTile = game.board.getTileFromLocation(character.x, character.y);

                foreach (var a in character.abilityList)
                {
                    if (a.uses > 0 && a.canUseSelf())
                    {
                        if (a.activeEffects.Select(x => x.statType == StatType.Heal) != null)
                        {
                            List<BattleAction> battleActionList = new List<BattleAction>() { new BattleAction() { ability = a, character = character, targetCharacter = character, targetTile = targetTile, actionType = BattleActionType.UseAbility } };
                            aiActionList.Add(new AIAction() { actionType = AIActionType.Heal, cost = a.ap, battleActionList = battleActionList });
                        }
                    }
                }

                var usableItems = from data in character.inventory
                                  where data is UsableItem
                                  select data;

                foreach (var i in usableItems.ToList())
                {

                    UsableItem tempItem = (UsableItem)i;

                    if (tempItem.activeEffects != null)
                    {
                        if (i.activeEffects.Select(x => x.statType == StatType.Heal) != null)
                        {
                            List<BattleAction> battleActionList = new List<BattleAction>() { new BattleAction() { item = tempItem, character = character, targetCharacter = character, targetTile = targetTile, actionType = BattleActionType.UseItem } };
                            aiActionList.Add(new AIAction() { actionType = AIActionType.Heal, cost = tempItem.actionPoints, battleActionList = battleActionList });
                        }
                    }
                }
            
   

            return AIActionList;
        }

        private List<AIAction> getAIBuffActions()
        {
            List<AIAction> aiActionList = new List<AIAction>();
            return AIActionList;
        }

        private List<AIAction> getAINukeActions()
        {
            List<AIAction> aiActionList = new List<AIAction>();
            return AIActionList;
        }

        private List<AIAction> getAIFleeActions()
        {
            List<AIAction> aiActionList = new List<AIAction>();
            return AIActionList;
        }


        #endregion

    }
}

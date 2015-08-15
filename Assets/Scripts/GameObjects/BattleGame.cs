using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
    public class BattleGame
    {

        public BattleSceneControllerScript gameControllerScript;


        public BattleGameData gameData;
      
        public Board board;
        public List<GameCharacter> characterList;
        public int currentCharacter = 0;

        public BattleLog battleLog;

        public Random r;

        public int TurnCounter;
        public bool NewTurn;

        public List<BattleAction> actionQueue = new List<BattleAction>();

        public GameCharacter ActiveCharacter
        {
            get
            {
                return characterList[currentCharacter];
            }
        }

        public Tile ActiveTile
        {
            get
            {
                return board.getTileFromLocation(ActiveCharacter.x, ActiveCharacter.y);
            }
        }

        public BattleGame(BattleGameData gameData, Random r, BattleSceneControllerScript gameScript)
        {
            this.gameControllerScript = gameScript;

            this.gameData = gameData;

            this.r = r;

            battleLog = new BattleLog();

            LoadBoardFromData();
            LoadCharactersFromData();

            StartBattle();
            
        }


        private void LoadBoardFromData()
        {
            //randomized board for now
            board = BoardFactory.getBoardFromBattleGameData(this.gameData, this);
            //board = BoardFactory.getBoardFromBoardData(this.gameData,this, gameData.BoardDataDictionary["Board1"]);

            //board = BoardFactory.getRandomBoard(this, 20);
        }


        private void LoadCharactersFromData()
        {
            characterList = new List<GameCharacter>();
            characterList.AddRange(gameData.gameCharacterList);
            battleLog.AddEntry("Characters Initialized");
        }

     


        private void StartBattle()
        {
            TurnCounter = 1;
            NewTurn = true;
            battleLog.AddEntry("Starting Battle");

            SetBattleInitiative();

            placeCharactersInBoard();

            battleLog.AddEntry(string.Format("{0}'s turn", ActiveCharacter.name));

        }

        private void SetBattleInitiative()
        { 
            
            //randomize attack order 
        }

        //Increment Initiative
        public void NextTurn()
        {
            actionQueue.Clear();
            ActiveCharacter.ResetAP();
            TurnCounter++;
            NewTurn = true;

            currentCharacter++;
            if(currentCharacter >= characterList.Count)
            {
                currentCharacter = 0;
            }

            battleLog.AddEntry(string.Format("{0}'s turn", ActiveCharacter.name));
        }

        public void NextTurnActiveDied()
        {
            TurnCounter++;
            NewTurn = true;
            if (currentCharacter >= characterList.Count)
            {
                currentCharacter = 0;
            }

            battleLog.AddEntry(string.Format("{0}'s turn", ActiveCharacter.name));
        }

        //currently, randomize
        private void placeCharactersInBoard()
        {
            foreach(var gc in characterList)
            {
                if(gc.type == CharacterType.Player)
                {
                    var freeTile = board.getFreeTileOfType(TileSpriteType.PlayerStart);
                    board.FillTile(gc, freeTile);
                }
                else
                {
                    var freeTile = board.getFreeTileOfType(TileSpriteType.EnemyStart);
                    board.FillTile(gc, freeTile);
                }

            }
        }

        #region BattleLoop


        //while we have actions in the queue, execute, otherwise return
        //return false if we fail the action queue, otherwise false
        public bool RunActionQueue()
        {
                BattleAction action = actionQueue[0];
                actionQueue.RemoveAt(0);

                switch(action.actionType)
                {
                    case BattleActionType.Move:
                        if(!Move(action.character,action.targetTile.x,action.targetTile.y))
                        {
                            actionQueue.Clear();
                            return false;
                        }
                        break; 
                    case BattleActionType.Attack:
                        if(!Attack(action.character,action.targetTile))
                        {
                            actionQueue.Clear();
                            return false;
                        }
                        break;
                    case BattleActionType.RangedAttack:
                        if (!RangedAttack(action.character, action.targetTile))
                        {
                            actionQueue.Clear();
                            return false;
                        }
                        break;
                    case BattleActionType.UseAbility:
                       
                       if(!UseAbility(action.character,action.ability,action.targetTile))
                       {
                           actionQueue.Clear();
                           return false;
                       }
                        break;
                    case BattleActionType.UseItem:
                        if(!UseItem(action.character,action.item,action.targetTile))
                        {
                            actionQueue.Clear();
                            return false;
                        }
                        break;
                    case BattleActionType.EndTurn:
                        battleLog.AddEntry(string.Format("{0} ended turn.", ActiveCharacter.name));

                        NextTurn();
                        return true;
                        
                    default:
                        return false;
                }

            return true;
        }

        #endregion

        #region BattleStatus


        public BattleStatusType getBattleStatus()
        {
            bool playersDead = true;
            bool enemiesDead = true;
            foreach (var gc in characterList)
            {
                if (gc.type == CharacterType.Player)
                {
                    if (gc.hp > 0)
                    {
                        playersDead = false;
                    }
                }
                if (gc.type == CharacterType.Enemy)
                {
                    if (gc.hp > 0)
                    {
                        enemiesDead = false;
                    }
                }
            }

            if (playersDead)
            {
                return BattleStatusType.PlayersDead;
            }
            else if (enemiesDead)
            {
                return BattleStatusType.EnemiesDead;
            }
            else
            {
                return BattleStatusType.Running;
            }
        }

        private void LoseBattle()
        {
            battleLog.AddEntry("Battle Lost");
            Console.WriteLine("You Lose! Press Enter to continue.\n");
            Console.ReadLine();
            StartBattle();
        }

        private void WinBattle()
        {

            battleLog.AddEntry("Battle Won");
            Console.WriteLine("You Win! Press Enter to continue.\n");
            Console.ReadLine();
            StartBattle();

        }

        #endregion


        public List<BattleAction> GetMoveActionList(int x, int y)
        {
            List<BattleAction> moveList = new List<BattleAction>();

            List<Point> pointList = PathFind.Pathfind(board, ActiveCharacter.x, ActiveCharacter.y, x, y);
            pointList.RemoveAt(0); //remove the character from pathfind.
            foreach (var p in pointList)
            {
                moveList.Add(new BattleAction() { character = ActiveCharacter, actionType = BattleActionType.Move, targetTile = board.getTileFromPoint(p) });
            }

            return moveList;
        }

       
  
        public List<BattleAction> getRangedAttackActionList(int x, int y)
        {
            List<BattleAction> actionList = new List<BattleAction>();
            actionList.Add(new BattleAction() { character = ActiveCharacter, targetTile = board.getTileFromLocation(x, y), actionType = BattleActionType.RangedAttack });
            return actionList;
        }

      
        
        public List<BattleAction> getAttackActionList(int x, int y)
        {
            List<BattleAction> actionList = new List<BattleAction>();
            List<Point> pointList = PathFind.Pathfind(board, ActiveCharacter.x, ActiveCharacter.y, x, y);
            pointList.RemoveAt(0); //remove the character from pathfind.
            pointList.RemoveAt(pointList.Count - 1); //remove the target from pathfind.

            foreach (var p in pointList)
            {
                actionList.Add(new BattleAction() { character = ActiveCharacter, actionType = BattleActionType.Move, targetTile = board.getTileFromPoint(p) });
            }

            actionList.Add(new BattleAction() { character = ActiveCharacter, targetTile = board.getTileFromLocation(x, y), actionType = BattleActionType.Attack });

            return actionList;
        }



       

        public List<BattleAction> getAbilityActionList(Ability a, int x, int y)
        {
            List<BattleAction> actionList = new List<BattleAction>();
            actionList.Add(new BattleAction() { character = ActiveCharacter, actionType = BattleActionType.UseAbility, ability = a, targetTile = board.getTileFromLocation(x, y) });

            return actionList;
        }

        public List<BattleAction> getItemActionList(UsableItem i, int x, int y)
        {
            List<BattleAction> actionList = new List<BattleAction>();
            actionList.Add(new BattleAction() { character = ActiveCharacter, actionType = BattleActionType.UseItem, item = i, targetTile = board.getTileFromLocation(x, y) });
            return actionList;
        }

        #region BoardHelpers
        public GameCharacter getCharacterFromTile(Tile t)
        {
            foreach (var c in characterList)
            {
                if (c.x == t.x && c.y == t.y)
                {
                    return c;
                }
            }
            return null;
        }

        //return the list of chars at this tile list
        public List<GameCharacter> getCharactersFromTileList(List<Tile> tileList)
        {
            List<GameCharacter> retvalList = new List<GameCharacter>();
            foreach(var t in tileList)
            {
                foreach(var c in characterList)
                {
                    if(c.x == t.x && c.y == t.y)
                    {
                        retvalList.Add(c);
                    }
                }
            }

            return retvalList;
        }

        #endregion


        #region Actions
     
        private void Skip()
        {
            battleLog.AddEntry(characterList[currentCharacter].name + " ended turn.");

            NextTurn();
        }

        private bool Move(GameCharacter character, int x, int y)
        {
            if (!CoreHelper.checkEffect(character.activeEffects, character.passiveEffects, StatType.Stun))
            {
               
                bool canMove = board.MoveCharacter(character, board.getTileFromLocation(x, y));
                if(canMove)
                {
                     battleLog.AddEntry(string.Format("{0} moved to {1},{2}", character.name, x, y));
                }
                else
                {
                     battleLog.AddEntry(string.Format("{0} was unable to move to {1},{2}", character.name, x, y));
                }
                return canMove;
            }
            else
            {
                battleLog.AddEntry(string.Format("{0} is stunned and unable to move.", character.name));
            }

            return false;
        }


        private bool RangedAttack(GameCharacter character, Tile destination)
        {
            if (!CoreHelper.checkEffect(character.activeEffects, character.passiveEffects, StatType.Stun))
            {
                GameCharacter target = getCharacterFromTile(destination);

                if (target != null)
                {
                    if (CombatHelper.RangedAttack(character, target, destination, this))
                    {
                        return true;
                    }
                }
            }
            else
            {
                battleLog.AddEntry(string.Format("{0} is stunned and unable to attack.", character.name));
            }

            return false;
        }

        private bool Attack(GameCharacter character, Tile targetTile)
        {
            if (!CoreHelper.checkEffect(character.activeEffects, character.passiveEffects, StatType.Stun))
            {
                GameCharacter target = getCharacterFromTile(targetTile);
                if (target != null)
                {
                    if (character.weapon != null)
                    {
                        if (character.SpendAP(character.weapon.actionPoints))
                        {
                            return CombatHelper.Attack(character, target, this);
                        }
                    }
                }
            }
            else
            {
                battleLog.AddEntry(string.Format("{0} is stunned and unable attack.", character.name));
            }

            return false;
        }
        
        public bool UseItem(GameCharacter character, UsableItem item, Tile targetTile)
        {

            bool usedItem = false;

            if (!CoreHelper.checkEffect(character.activeEffects, character.passiveEffects, StatType.Stun))
            {
                if (character.CheckAP(item.actionPoints))
                {
                    if (item.activeEffects != null)
                    {
                        foreach (var a in item.activeEffects)
                        {
                            character.AddActiveEffect(a, this);
                        }
                        usedItem = true;
                        character.SpendAP(item.actionPoints);
                        item.uses--;
                    }

                    else if(item.itemAbility != null)
                    {
                        bool usedAbility = AbilityHelper.UseAbility(ActiveCharacter, item.itemAbility, targetTile, this);
                        if(usedAbility)
                        {
                            usedItem = true;
                            character.SpendAP(item.actionPoints);
                            item.uses--;
                        }
                    }

                    if (item.uses <= 0)
                    {
                        //should this logic be here?
                        battleLog.AddEntry(string.Format("{0} has no more uses.", item.name));

                        ActiveCharacter.inventory.Remove(item);
                    }
                }
               
                if(usedItem)
                {
                    battleLog.AddEntry(string.Format("{0} used item {1}", character.name, item.name));
                    return true;
                }
                else
                {
                    battleLog.AddEntry(string.Format("{0} was unable to use item {1}", character.name, item.name));
                    return false;
                }
               
            }
            else{
                battleLog.AddEntry(string.Format("{0} is stunned and unable to use {1}.", character.name, item.name));
            }
            
            return false;

        }

        public bool UseAbility(GameCharacter character, Ability ability, Tile target)
        {
            if (!CoreHelper.checkEffect(character.activeEffects, character.passiveEffects, StatType.Stun))
            {
                if (character.abilityList.Contains(ability))
                {
                    if (AbilityHelper.UseAbility(character, ability, target, this))
                    {
                        battleLog.AddEntry(string.Format("{0} used {1}", character.name, ability.name));
                        return true;
                    }
                    else
                    {
                        battleLog.AddEntry(string.Format("{0} failed to use {1}", character.name, ability.name));
                    }
                }
            }
            else
            {
                battleLog.AddEntry(string.Format("{0} is stunned and unable to use {1}.", character.name, ability.name));
            }

            return false;
        }

        #endregion

        #region EnemyAI

        public List<BattleAction> getEnemyActionList()
        {
            List<BattleAction> actionList = new List<BattleAction>();
            if (!CoreHelper.checkEffect(ActiveCharacter.activeEffects, ActiveCharacter.passiveEffects, StatType.Stun))
            {
                actionList = AI.getBattleActionList((EnemyCharacter)ActiveCharacter, this);
               

            }

            return actionList;
        }

        #endregion

        public void CharacterKill(GameCharacter character)
        {
            battleLog.AddEntry(string.Format("{0} was killed", character.name));

            Tile tempTile = board.getTileFromLocation(character.x,character.y);
            board.EmptyTile(tempTile);
            characterList.Remove(character);

        }
    }
}

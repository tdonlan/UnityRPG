using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
    public class AI
    {



        public static List<BattleAction> getBattleActionList(EnemyCharacter character, BattleGame game)
        {
            List<BattleAction> retvalList = character.aiActor.getBattleActionList(game);

            retvalList.Add(getEndTurnAction(character));

            return retvalList;
        }

        private static BattleAction getEndTurnAction(EnemyCharacter character)
        {
            return new BattleAction() { actionType = BattleActionType.EndTurn, character = character, targetCharacter = null, targetTile = null };
            
        }

        public static List<BattleAction> attackNearestPlayer(GameCharacter enemy, BattleGame game)
        {
            List<BattleAction> actionList = new List<BattleAction>();

            var attackTarget = findNearestPlayer(enemy, game.board,game.characterList);

            var targetTile = game.board.getTileFromLocation(attackTarget.x, attackTarget.y);

            //path find to target
            List<Point> pointList = PathFind.Pathfind(game.board, enemy.x, enemy.y, targetTile.x, targetTile.y);
            pointList.RemoveAt(0); //remove the character from pathfind.
            pointList.RemoveAt(pointList.Count - 1); //remove the target from pathfind.

            foreach (var p in pointList)
            {
                actionList.Add(new BattleAction() { character = enemy, actionType = BattleActionType.Move, targetTile = game.board.getTileFromPoint(p) });
            }

            //attack action
            actionList.Add(new BattleAction() { character = enemy, targetTile = targetTile, actionType = BattleActionType.Attack });

            return actionList;
        }


        //DEPRECATED
        /*
        public static void attackNearestPlayer(GameCharacter enemy, BattleGame game)
        {
            
            var attackTarget= getAttackablePlayer(enemy,game);
            if (attackTarget != null)
            {
                attackPlayer(enemy, attackTarget, game);
            }
            else
            {
                var moveTarget = findNearestPlayer(enemy, game.board, game.characterList);
                moveToPlayer(enemy, moveTarget, game.board);
            }
            
        }
         * */

        //DEPRECATED
        private static void attackPlayer(GameCharacter enemy, GameCharacter player, BattleGame game)
        {
            if(enemy.SpendAP(enemy.weapon.actionPoints))
            {
                CombatHelper.Attack(enemy, player, game);
            }
        }

        public static GameCharacter getAttackablePlayer(GameCharacter enemy, BattleGame game)
        {
            Tile curTile = game.board.getTileFromLocation(enemy.x,enemy.y);
            var charList = game.getCharactersFromTileList(game.board.getTileListFromPattern(curTile, TilePatternType.FourAdj));

            var playerList = (from data in charList
                             where data.type == CharacterType.Player
                             select data).ToList();

            //for now, just return a random enemy close 
            if (playerList.Count > 0)
            {
                return charList[game.r.Next(charList.Count - 1)];
            }
            else
            {
                return null;
            }

        }


        public static GameCharacter findNearestPlayer( GameCharacter enemy, Board board,List<GameCharacter> charList)
        {
            GameCharacter retval = null;
            int dist = 999;
            
            foreach(GameCharacter c in charList)
            {
                if (c.type == CharacterType.Player)
                {
                    var pointList = PathFind.Pathfind(board, enemy.x, enemy.y, c.x, c.y);
                    if (pointList.Count < dist)
                    {
                        dist = pointList.Count;
                        retval = c;
                    }
                }
               
            }
            return retval;
        }

        //iterates over the path find and moves single spaces
        public static void moveToPlayer(GameCharacter enemy, GameCharacter target, Board board)
        {
            var pointList = PathFind.Pathfind(board, enemy.x, enemy.y, target.x, target.y);
            foreach(var p in pointList)
            {
                
                board.MoveCharacter(enemy, board.getTileFromLocation(p.x, p.y));
                  
               
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace UnityRPG
{
    public class AbilityHelper
    {
        private static bool UseAbilityLOS(GameCharacter character, Ability ability, Tile target, BattleGame game)
        {
            Tile ActiveTile = game.board.getTileFromLocation(character.x, character.y);
            var tileLOSList = game.board.getBoardLOS(ActiveTile, target);

            if (tileLOSList.Count <= ability.range && tileLOSList[tileLOSList.Count - 1] == target)
            {
                if (character.SpendAP(ability.ap))
                {
                    return UseAbilityAOEHelper(character, ability, target, game);
                }
                return false;
            }
            else
            {
                return false;
            }

        }

        private static bool UseAbilityLOSEmpty(GameCharacter character, Ability ability, Tile target, BattleGame game)
        {
            if (target.empty)
            {
                return UseAbilityLOS(character, ability, target, game);
            }

            return false;

        }

        private static bool UseAbilityAOEHelper(GameCharacter character, Ability ability, Tile target, BattleGame game)
        {

            var tileAOEList = game.board.getTileListFromPattern(target, ability.tilePatternType);

            //draw AOE effect
            foreach (var t in tileAOEList)
            {
                game.board.AddTempChar(t, '*');
                game.board.AddTempEffect(t, ability.sheetname, ability.spriteindex);

                UseAbilityTempEffect(game, character, t, ability);
            }

            var charAOEList = game.getCharactersFromTileList(tileAOEList);

            return UseAbilityOnCharList(character,target, ability, charAOEList, game);

        }

        private static void UseAbilityTempEffect( BattleGame game,GameCharacter character, Tile target, Ability ability)
        {
            foreach (var ae in ability.activeEffects)
            {
                switch (ae.effectType)
                {
                    case TempEffectType.Particle:
                        game.gameControllerScript.StartTempParticles(ae.effectName, new UnityEngine.Vector3(target.x, target.y));
                        break;
                    case TempEffectType.Sprite:
                        game.gameControllerScript.StartTempSprite(new UnityEngine.Vector3(target.x, target.y), ae.effectName, ae.effectIndex);
                        break;
                    case TempEffectType.Text:
                        game.gameControllerScript.StartTempText(new UnityEngine.Vector3(target.x, target.y), UnityEngine.Color.grey, ability.name);
                        break;
                }
            }

        }

        private static void UseAbilityTempEffect(BattleGame game, GameCharacter character, GameCharacter target, Ability ability)
        {
            foreach (var ae in ability.activeEffects)
            {
                switch (ae.effectType)
                {
                    case TempEffectType.Particle:
                        game.gameControllerScript.StartTempParticles(ae.effectName, new UnityEngine.Vector3(target.x, target.y));
                        break;
                    case TempEffectType.Sprite:
                        game.gameControllerScript.StartTempSprite(new UnityEngine.Vector3(target.x, target.y), ae.effectName, ae.effectIndex);
                        break;
                    case TempEffectType.Text:
                        game.gameControllerScript.StartTempText(new UnityEngine.Vector3(target.x, target.y), UnityEngine.Color.grey, ability.name);
                        break;
                }
            }
        }

        private static bool UseAbilityOnCharList(GameCharacter sourceCharacter, Tile target, Ability ability, List<GameCharacter> characterList, BattleGame game)
        {

            //Draw Temp Character
             game.board.AddTempChar(target, 'X');
             game.board.AddTempEffect(target, ability.sheetname, ability.spriteindex);


            foreach(var c in characterList)
            {
                UseAbilityTempEffect(game, sourceCharacter, c, ability);
            }
             

            //special conditions if we're doing something on sourceCharacter
            if(characterList.Count ==0)
            {
                foreach (var ae in ability.activeEffects)
                {
                    if (ae.statType == StatType.Teleport)
                    {
                        game.board.MoveCharacterFree(sourceCharacter, target);
                    }
                }
            }

            foreach (var character in characterList)
            {
                foreach (var ae in ability.activeEffects)
                {
                    if (ae.statType == StatType.Teleport)
                    {
                        game.board.MoveCharacterFree(sourceCharacter, target); //for now, can only teleport self.
                    }
                    else if (ae.statType == StatType.Knockback) //move away from sourceCharacter
                    {
                        Tile sourceTile = game.board.getTileFromLocation(sourceCharacter.x, sourceCharacter.y);
                        Tile charTile = game.board.getTileFromLocation(character.x, character.y);
                        List<Tile> moveTargetList = game.board.getMoveTargetTileList(sourceTile, charTile, ae.minAmount);

                        if (moveTargetList.Count > 0)
                        {
                            Tile moveTile = moveTargetList[moveTargetList.Count - 1];
                            game.board.MoveCharacterFree(character, moveTile);
                        }
                    }
                    else if(ae.statType == StatType.Explode) //move away from target
                    {
                        Tile charTile = game.board.getTileFromLocation(character.x, character.y);
                        List<Tile> moveTargetList = game.board.getMoveTargetTileList(target, charTile, ae.minAmount);
                        if (moveTargetList.Count > 0)
                        {
                            Tile moveTile = moveTargetList[moveTargetList.Count - 1];
                            game.board.MoveCharacterFree(character, moveTile);
                        }
                    }
                    else
                    {
                        character.AddActiveEffect(cloneActiveEffect(ae), game);
                    }
                }
            }

            return true;
        }

        private static bool UseAbilityPoint(GameCharacter character, Ability ability, Tile target, BattleGame game)
        {
            Tile ActiveTile = game.board.getTileFromLocation(character.x, character.y);

            int dist = PlotLine.GetPointsOnLine(character.x, character.y, target.x, target.y).Count();

            if(dist <= ability.range)
            {
                if (character.SpendAP(ability.ap))
                {
                    return UseAbilityAOEHelper(character, ability, target, game);
                }
            }
            return false;
        }
        
        
        private static bool UseAbilityPointEmpty(GameCharacter character, Ability ability, Tile target, BattleGame game)
        {
            if(target.empty)
            {
                return UseAbilityPoint(character, ability, target, game);
            }
          
            return false;
        }


        //Includes Self
        private static bool UseAbilityAllFriends(GameCharacter character, Ability ability, Tile target, BattleGame game)
        {
            if (character.SpendAP(ability.ap))
            {
                var friendList = from data in game.characterList
                                 where data.type == character.type
                                 select data;

                return UseAbilityOnCharList(character,target, ability, friendList.ToList(), game);
            }
            else
            {
                return false;
            }
        }

        private static bool UseAbilityAllFoes(GameCharacter character, Ability ability, Tile target, BattleGame game)
        {
            if (character.SpendAP(ability.ap))
            {
                var foeList = from data in game.characterList
                              where data.type != character.type
                              select data;

                return UseAbilityOnCharList(character,target, ability, foeList.ToList(), game);
            }
            else
            {
                return false;
            }
        }

        private static bool UseAbilitySingleFriend(GameCharacter character, Ability ability, Tile target, BattleGame game)
        {
            if (character.SpendAP(ability.ap))
            {
                GameCharacter targetChar = game.getCharacterFromTile(target);
                if (targetChar != null & targetChar.type == character.type)
                {
                    return UseAbilityOnCharList(character,target, ability, new List<GameCharacter>() { targetChar }, game);
                }
            }
            return false;
        }

        private static bool UseAbilitySingleFoe(GameCharacter character, Ability ability, Tile target, BattleGame game)
        {
            if (character.SpendAP(ability.ap))
            {
                GameCharacter targetChar = game.getCharacterFromTile(target);
                if (targetChar != null & targetChar.type != character.type)
                {
                    return UseAbilityOnCharList(character,target, ability, new List<GameCharacter>() { targetChar }, game);
                }
            }
            return false;
        }

        private static bool UseAbilitySelf(GameCharacter character, Ability ability, Tile target, BattleGame game)
        {
            if (character.SpendAP(ability.ap))
            {
                return UseAbilityOnCharList(character,target, ability, new List<GameCharacter>() { character }, game);
            }
            else
            {
                return false;
            }
        }

        public static bool UseAbility(GameCharacter character, Ability ability, Tile target, BattleGame game)
        {
            if (ability.uses > 0)
            {
                ability.uses--;

                switch (ability.targetType)
                {
                    case AbilityTargetType.Self:
                        return UseAbilitySelf(character, ability, target, game);
                    case AbilityTargetType.SingleFriend:
                        return UseAbilitySingleFriend(character, ability, target, game);
                    case AbilityTargetType.SingleFoe:
                        return UseAbilitySingleFoe(character, ability, target, game);
                    case AbilityTargetType.AllFriends:
                        return UseAbilityAllFriends(character, ability, target, game);
                    case AbilityTargetType.AllFoes:
                        return UseAbilityAllFoes(character, ability, target, game);
                    case AbilityTargetType.PointEmpty:
                        return UseAbilityPointEmpty(character, ability, target, game);
                    case AbilityTargetType.PointTarget:
                        return UseAbilityPoint(character, ability, target, game);
                    case AbilityTargetType.LOSEmpty:
                        return UseAbilityLOSEmpty(character, ability, target, game);
                    case AbilityTargetType.LOSTarget:
                        return UseAbilityLOS(character, ability, target, game);
                    default:
                        return false;
                }
            }
            return false;
        }

        public static Ability getAbilityFree(Ability a)
        {
            Ability tempAbility = cloneAbility(a);
            tempAbility.ap = 0;
            tempAbility.uses = 999;
            return tempAbility;

        }


        //JSON.NET not available in Unity by default
        //NEED TO FIND FIX, OR PAY FOR JSON.NET IN UNITY
        public static Ability cloneAbility(Ability a)
        {
            return a;
            //return JsonConvert.DeserializeObject<Ability>(JsonConvert.SerializeObject(a));
        }


        public static ActiveEffect cloneActiveEffect(ActiveEffect ae)
        {
            return ae;
           // return JsonConvert.DeserializeObject<ActiveEffect>(JsonConvert.SerializeObject(ae));
        }
        

    }
}

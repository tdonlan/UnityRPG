using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
   public enum CharacterType
   {
       Player,
       Enemy,
       NPC
   }

    public enum CharacterClass
    {
        Warrior,
        Mage,
        Priest
    }

    public enum ItemType
    {
        Weapon,
        Potion,
        Armor,
        Thrown,
        Wand,
        Ammo,
    }

    public enum WeaponType
    {
        OneHandMelee,
        OneHandRanged,
        TwoHandMelee,
        TwoHandRanged,
    }

    public enum AmmoType
    {
        Arrows,
        Bolts,
        Stones,

    }

    public enum ArmorType
    {
        Head,
        Chest,
        Gloves,
        Legs,
        Boots,
        Ring,
        Trinket,
    }

    public enum StatType
    {
        ActionPoints,
        Armor,
        Damage, 
        Heal,
        HitPoints, //temp buff to hitpoints
        Attack,
        Teleport, //move character to target
        Knockback, // move targets away from character
        Explode, //move characters away from target
        Stuck, //character cannot move
        Dispell, //remove active effects
        Stun, //player can't do anything
       
    }

    public enum BattleStatusType
    {
        PlayersDead,
        EnemiesDead,
        Running,
        GameOver,
       
    }

    public enum DirectionType
    {
        North,
        South,
        West,
        East,
    }

    //patterns of tiles affected (by attacks, spells, etc)
    public enum TilePatternType
    {
        Single,
        FourAdj,
        EightAdj,
        NineSquare,
        ThreeLineVert,
        ThreeLineHor,
    }

    public enum AbilityTargetType
    {
        Self,
        SingleFriend,
        SingleFoe,
        AllFriends,
        AllFoes,
        PointEmpty,
        PointTarget,
        LOSEmpty,
        LOSTarget,
    }

    public enum MasterListType
    {
        Abilities,
        Items,
        Characters,
        Maps
    }

    public enum BattleActionType
    {
        Move,
        Attack,
        RangedAttack,
        UseItem,
        UseAbility,
        EndTurn
    }

    public enum EnemyType
    {
        Warrior,
        Mage,
        Priest,
        Archer
    }

    public enum AIActionType
    {
        Attack,
        RangedAttack,
        Heal,
        Buff,
        Nuke,
        Flee
    }
        

}


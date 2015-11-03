

public enum UnitySceneIndex
{
    Start = 0,
    World = 1,
    Zone = 2,
    Dialog = 3,
    Battle = 4,
    Store=5,
    CharacterScreen=6,
    CharacterCreationScreen=7,
    Cutscene=8
}

public enum SpritesheetType
{
    Tiles,
    Characters,
    Enemies,
    Items,
    Portraits,
    Particles,

}

public enum TileSpriteType
{
    Wall,
    Floor,
    PlayerStart,
    EnemyStart,
    NPCStart,
    Special,
}


public enum UIStateType
{
    NewTurn,
    PlayerDecide,
    PlayerExecute,
    EnemyDecide,
    EnemyExecute
}


public enum PlayerDecideState
{
    Waiting,
    MovePendingClick,
    AttackPendingClick,
    RangedAttackPendingClick,
    ItemPendingClick,
    ItemPanelShow,
    AbilityPendingClick,
    AbilityPanelShow,

}



    public enum TreeType
    {
        World=0,
        Zone=1,
        Dialog=2,
        Quest=3,
        Battle=4,
        Info=5,
        Store=6,
        Cutscene=7

    }

    public enum ZoneNodeType
    {
        Battle,
        Dialog,
        Store,
        Item,
        Info,
        Puzzle,
        Link,
        Cutscene

    }

    public enum BattleNodeType
    {
        Info,
        Enemy,
        Loot,
        Win
    }

    public enum InfoNodeType
    {
        Info,
        Loot,
        End
    }

    public enum StoreNodeType
    {
        Info,
        ItemClass,
        ItemIndex
    }

    public enum NodeActionType
    {
        AddFlag,
        RemoveFlag,
        AddItem,
        RemoveItem,
        AddEffect,
        RemoveEffect,
        AddCharacter,
        RemoveCharacter,
        AddXP,
    }

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
        Money,
        Weapon,
        Potion,
        Armor,
        Thrown,
        Wand,
        Ammo,
        Quest
    }

    public enum WeaponType
    {
        OneHandMelee,
        OneHandRanged,
        TwoHandMelee,
        TwoHandRanged,
        Thrown
    }

    public enum AmmoType
    {
        Arrows,
        Bolts,
        Stones,
        Thrown
    }

    public enum ArmorType
    {
        Head,
        Chest,
        Gloves,
        Boots,
        Ring,
        Trinket,
    }

    public enum StatType
    {
        Strength,
        Agility,
        Endurance,
        Spirit,
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
        None,
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

    public enum TempEffectType
    {
        Particle,
        Sprite,
        Text,
        ProjectileSprite,
        ProjectileParticle
    }
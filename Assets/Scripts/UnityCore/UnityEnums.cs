using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum UnitySceneIndex
{
    Start=0,
    World=1,
    Zone=2,
    Dialog=3,
    Battle=4,
    BattleGame=5,
    GameOver=6
}

    public enum TreeType
    {
        World=0,
        Zone=1,
        Dialog=2,
        Quest=3,
        Battle=4,
        Info=5

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

    public enum NodeActionType
    {
        AddFlag,
        RemoveFlag,
        AddItem,
        RemoveItem,
        AddEffect,
        RemoveEffect
    }


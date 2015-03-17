using UnityEngine;
using System.Collections;
using System.Collections.Generic;


using System;
using UnityEngine.UI;
using Assets;
using UnityEngine.EventSystems;
using UnityEngine.Events;

using System.Linq;



    public class GameConfig
    {

        public static readonly Vector3 AbilityPanelDisplayLocation = new Vector3(330, -250, -1);
        public static readonly Vector3 ItemPanelDisplayLocation = new Vector3(330, -250, -1);
        public static readonly Vector3 EquipPanelDisplayLocation = new Vector3(0, 0, 0);

        public static readonly Vector3 AbilityPanelHideLocation = new Vector3(300, -600, 0);
        public static readonly Vector3 ItemPanelHideLocation = new Vector3(300, -600, 0);
        public static readonly Vector3 EquipPanelHideLocation = new Vector3(-1400, -0, 0);

        public static readonly Vector3 HoverStatsPanelLocation = new Vector3(0, 315, 0);

        public static readonly float playerUpdateBattleTimer = 0.5f;
        public static readonly float enemyUpdateBattleTimer = 0.25f;


    }


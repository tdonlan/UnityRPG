using UnityEngine;
using System.Collections;
using System.Collections.Generic;


using System;
using UnityEngine.UI;
using UnityRPG;
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


        public static readonly Vector3 PendingActionPanelLocation = new Vector3(-374f, -20.8f, 0f);
        public static readonly Vector3 SelectedCharacterPanelLocation = new Vector3(117f, -20.83f, 0f);

        public static readonly float playerUpdateBattleTimer = 0.5f;
        public static readonly float enemyUpdateBattleTimer = 0.25f;

        public static readonly float PanSpeed = 1f;
        public static readonly float PanLerp = 0.25f;
        public static readonly float ZoomLerp = 0.1f;

        public static readonly float ZoomFactor = 5;
        public static readonly float MinZoom = 2.5f;
        public static readonly float MaxZoom = 10f;

        public static Color transRed = new Color(.8f, 0, 0, .5f);
        public static Color transWhite = new Color(1, 1, 1, .5f);
        public static Color Gold = new Color(1, .78f, 0, 1f);

    }


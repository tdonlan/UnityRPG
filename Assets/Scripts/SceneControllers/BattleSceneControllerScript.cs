using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

using UnityRPG;

using UnityEngine.EventSystems;


    public class BattleSceneControllerScript : MonoBehaviour
    {
        public GameDataObject gameDataObject { get; set; }

        //Prefabs
        private GameObject ActionButtonPrefab;
        private GameObject ActionIconPrefab;
        private GameObject CharacterPanelPrefab;
        private GameObject EffectIconPrefab;
        private GameObject InfoPopupPrefab;
        private GameObject BattleInitiativePanelPrefab;
        private GameObject PendingActionPrefab;

        private GameObject SpritePrefab;
        private GameObject CharacterPrefab;

        //UI References
        public Canvas Canvas;
        public GameObject ActiveCharacterPanel;
        public GameObject InitListPanel;
        public GameObject PendingActionPanel;
        public GameObject SelectedCharacterPanel;
        public GameObject EventLogPanel;
        public GameObject ActionIconPanel;
        public GameObject BattlePanel;
        public GameObject VictoryPanel;

        public Text BattleLogText;
	public Text DebugText;

        public Button MoveActionButton;
        public Button AttackActionButton;
        public Button AbilityActionButton;
        public Button ItemActionButton;
        public Button EndTurnActionButton;

        //UI Data
        private Point hoverPoint;
        private Point clickPoint;
        private GameObject mouseOverTile;

        public List<GameObject> highlightTiles { get; set; }
        public GameObject SelectedTile { get; set; }

        public GameCharacter SelectedCharacter { get; set; }

        private float UITimer { get; set; }
        private float TempEffectTimer { get; set; }

        public UIStateType uiState { get; set; }
        public PlayerDecideState playerDecideState { get; set; }

        public GameObject HoverInfoPanel { get; set; }

        //World Data
        public GameObject tileMapPrefab { get; set; }
        public GameObject tileMapObject { get; set; }

        public TileMapData tileMapData;
        public long parentTreeLink;
        public BattleTree battleTree { get; set; }
        public BattleGameData battleGameData { get; set; }

        public BattleGame battleGame { get; set; }
        public BattleStatusType battleStatus { get; set; }

        public List<GameObject> tileCharacterList { get; set; }
        public List<TempEffect> tempEffectList { get; set; }

        private Ability selectedAbility { get; set; }
        private UsableItem selectedItem { get; set; }

        //Camera Pan / Zoom
        private GameObject cameraObject;
        private Camera mainCamera;
        private BattleSceneCameraData cameraData { get; set; }

        //Misc
        public System.Random r { get; set; }

        void OnLevelWasLoaded(int level)
        {

            loadGameData();
            loadTreeStore();
            loadTileMapData();

            tileCharacterList = new List<GameObject>();
            tempEffectList = new List<TempEffect>();
            highlightTiles = new List<GameObject>();

            this.r = new System.Random();

            this.clickPoint = null;

            this.uiState = UIStateType.NewTurn;

            List<GameCharacter> playerCharacterList = new List<GameCharacter>() { gameDataObject.playerGameCharacter };
            playerCharacterList.AddRange(gameDataObject.partyList);

            this.battleGameData = BattleFactory.getBattleGameDataFromZoneTree(playerCharacterList, battleTree, gameDataObject.gameDataSet, tileMapData);

            this.battleGame = new BattleGame(battleGameData, r, this);

            LoadPrefabs();

            LoadCharacters();

            UpdateUI();

            SetCamera();

            //testing
            displayCollisionSprites();

        }

        #region Loading

        private void loadGameData()
        {
            gameDataObject = GameObject.FindObjectOfType<GameDataObject>();
        }

        private void loadTreeStore()
        {
            //dont select tree, get the tree node from current zone content
            //assuming the parent is a zone type for now
            ZoneTree parentTree = (ZoneTree)gameDataObject.treeStore.getCurrentTree();
            ZoneTreeNode parentTreeNode = (ZoneTreeNode)parentTree.getNode(parentTree.currentIndex);
            long dialogLink = parentTreeNode.content.linkIndex;

            parentTreeLink = gameDataObject.treeStore.currentTreeIndex;

            gameDataObject.treeStore.SelectTree(dialogLink);
            battleTree = (BattleTree)gameDataObject.treeStore.getCurrentTree();
        }

        private void loadTileMapData()
        {
            tileMapPrefab = Resources.Load<GameObject>(battleTree.treeName);
            tileMapObject = (GameObject)Instantiate(tileMapPrefab);
            //tileMapObject.tag = "tileMap";
            tileMapData = new TileMapData(tileMapObject);
        }

        private void LoadPrefabs()
        {
            //UI
            ActionButtonPrefab = Resources.Load<GameObject>("PrefabUI/BattlePrefabs/ActionButtonPrefab");
            ActionIconPrefab = Resources.Load<GameObject>("PrefabUI/BattlePrefabs/ActionIconPrefab");
            CharacterPanelPrefab = Resources.Load<GameObject>("PrefabUI/BattlePrefabs/CharacterPanelPrefab");
            EffectIconPrefab = Resources.Load<GameObject>("PrefabUI/BattlePrefabs/EffectIconPrefab");
            InfoPopupPrefab = Resources.Load<GameObject>("PrefabUI/BattlePrefabs/InfoPopupPrefab");
            PendingActionPrefab = Resources.Load<GameObject>("PrefabUI/BattlePrefabs/PendingActionPrefab");

            BattleInitiativePanelPrefab = Resources.Load<GameObject>("PrefabUI/BattleInitiativePanelPrefab");
          
            //Game
            SpritePrefab = Resources.Load<GameObject>("PrefabGame/SpritePrefab");
            CharacterPrefab = Resources.Load<GameObject>("PrefabGame/CharacterPrefab");
        }

    private void LoadCharacters()
	{
		UpdateActiveCharacterPanel ();

		//clear Characters
		foreach (var c in tileCharacterList) {
			Destroy (c);
		}

		tileCharacterList.Clear ();

		//draw in order of y coordinate, to prevent wierd overlaps
		var orderedCharList = battleGame.characterList;

		orderedCharList = orderedCharList.OrderByDescending (x => x.y).ToList ();

		foreach (var character in orderedCharList) {
			tileCharacterList.Add (LoadCharacter (character));
		}
	}


        //Load Character Avatar Sprite on Tile Map
        private GameObject LoadCharacter(GameCharacter character)
        {
            GameObject characterObject = (GameObject)Instantiate(CharacterPrefab);
            GameObjectHelper.UpdateSprite(characterObject, "CharacterSprite", gameDataObject.assetLibrary.getSprite(character.characterSpritesheetName, character.characterSpriteIndex));
			GameObjectHelper.UpdateSprite(characterObject, "SpriteOutline", gameDataObject.assetLibrary.getSprite(character.characterSpritesheetName, character.characterSpriteIndex));

            if (character.type == CharacterType.Player)
            {
                GameObjectHelper.UpdateSpriteColor(characterObject, "HighlightSprite", Color.green);
			GameObjectHelper.UpdateSpriteColor(characterObject, "SpriteOutline", Color.green);

                if (battleGame.ActiveCharacter.Equals(character))
                {
                    GameObjectHelper.UpdateSpriteColor(characterObject, "HighlightSprite", Color.yellow);
				GameObjectHelper.UpdateSpriteColor(characterObject, "SpriteOutline", Color.yellow);
                }
            }
            else
            {
                GameObjectHelper.UpdateSpriteColor(characterObject, "HighlightSprite", Color.red);
			GameObjectHelper.UpdateSpriteColor(characterObject, "SpriteOutline", Color.red);
            }

            var characterPos = getWorldPosFromTilePoint(new Point(character.x, -character.y));
            characterObject.transform.position = characterPos;

            return characterObject;
        }

        #endregion

        #region UpdateUIElements

        private void UpdateActiveCharacterPanel()
        {
            if (battleGame.ActiveCharacter != null)
            {
                var ac = battleGame.ActiveCharacter;
                UIHelper.UpdateSpriteComponent(ActiveCharacterPanel, "CharacterPortrait", gameDataObject.assetLibrary.getSprite(ac.portraitSpritesheetName, ac.portraitSpriteIndex));
                UIHelper.UpdateTextComponent(ActiveCharacterPanel, "CharacterName", ac.name);
                UIHelper.UpdateTextComponent(ActiveCharacterPanel, "HPText", string.Format("HP:{0}/{1}", ac.hp, ac.totalHP));
                UIHelper.UpdateTextComponent(ActiveCharacterPanel, "APText", string.Format("AP:{0}/{1}", ac.ap, ac.totalAP));

                UIHelper.UpdateSliderValue(ActiveCharacterPanel, "APSlider", (float)ac.ap / (float)ac.totalAP);

                UIHelper.UpdateSliderValue(ActiveCharacterPanel, "HPSlider", (float)ac.hp / (float)ac.totalHP);

                var charPortrait = UIHelper.getChildObject(ActiveCharacterPanel, "CharacterPortrait");

                UIHelper.RemoveEventTriggers(charPortrait);
                UIHelper.AddClickToGameObject(charPortrait, MoveCameraToCharacter, EventTriggerType.PointerClick, ac);

                var effectPanel = UIHelper.getChildObject(ActiveCharacterPanel,"EffectPanel");
                UpdateEffectIcons(effectPanel, ac.activeEffects);

            }
        }

        private void UpdateEffectIcons(GameObject effectPanel, List<ActiveEffect> effectList)
        {
            UIHelper.DestroyAllChildren(effectPanel.transform);

            foreach (var ae in effectList)
            {
                var effectObject = Instantiate(EffectIconPrefab);
                effectObject.transform.SetParent(effectPanel.transform);
                UpdateEffectIcon(effectObject, ae);
            }
        }

        private void UpdateEffectIcon(GameObject effectIcon, ActiveEffect ae)
        {
            UIHelper.UpdateSpriteComponent(effectIcon, "Image", gameDataObject.assetLibrary.getSprite(ae.effectName, ae.effectIndex));
            UIHelper.AddClickToGameObject(effectIcon, AddHoverInfoPanel, EventTriggerType.PointerEnter, ae);
            UIHelper.AddClickToGameObject(effectIcon, RemoveHoverInfoPanel, EventTriggerType.PointerExit);
        }

        public void RemoveHoverInfoPanel()
        {
            if(HoverInfoPanel != null){
                Destroy(HoverInfoPanel);
                HoverInfoPanel = null;
            }
          
        }

        public void AddHoverInfoPanel(System.Object activeEffectObject)
        {
            ActiveEffect activeEffect = (ActiveEffect)activeEffectObject;
            if (HoverInfoPanel != null)
            {
                HoverInfoPanel.transform.position = UIHelper.getMouseHoverPos(Input.mousePosition);
            }
            else
            {
                HoverInfoPanel = Instantiate(InfoPopupPrefab);
                HoverInfoPanel.transform.SetParent(Canvas.transform);
                HoverInfoPanel.transform.position = UIHelper.getMouseHoverPos(Input.mousePosition);
                UpdateInfoPopup(HoverInfoPanel, activeEffect.ToString());
            }
            
        }

        public void UpdateInfoPopup(GameObject infoPopup, string text)
        {
            UIHelper.UpdateTextComponent(infoPopup, "Text", text);
        }


        private void UpdateInitiativePanel()
        {
            //clear current Panel
            for (int i = InitListPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(InitListPanel.transform.GetChild(i).gameObject);
            }

            if (battleGame != null)
            {
                //Only display characters 2-7 in the init list
                var charList = battleGame.characterList;

                List<GameCharacter> curInitiativeList = new List<GameCharacter>();

                var activeCharIndex = charList.IndexOf(battleGame.ActiveCharacter);

                for (int i = activeCharIndex; i < charList.Count; i++)
                {
                    curInitiativeList.Add(charList[i]);
                }

                for (int i = 0; i < activeCharIndex; i++)
                {
                    curInitiativeList.Add(charList[i]);
                }

                curInitiativeList.RemoveAt(0);

                if (curInitiativeList.Count > 6)
                {
                    curInitiativeList.RemoveRange(6, curInitiativeList.Count - 6);
                }

                foreach (var character in curInitiativeList)
                {
                    GameObject initGameObject = (GameObject)Instantiate(BattleInitiativePanelPrefab);
                    initGameObject = updateInitPortrait(initGameObject, character);

                    initGameObject.transform.SetParent(InitListPanel.transform, true);
                }
            }
        }

        private GameObject updateInitPortrait(GameObject charPortrait, GameCharacter character)
        {
            var panelImg = charPortrait.GetComponent<Image>();
            if (character.type == CharacterType.Player)
            {
                panelImg.color = Color.green;

            }
            else
            {
                panelImg.color = Color.red;
            }

            if (character == battleGame.ActiveCharacter)
            {
                panelImg.color = Color.yellow;

            }

            UIHelper.UpdateSpriteComponent(charPortrait, "PortraitImage", gameDataObject.assetLibrary.getSprite(character.portraitSpritesheetName, character.portraitSpriteIndex));
            UIHelper.UpdateSliderValue(charPortrait, "HPSlider", (float)character.hp / (float)character.totalHP);

            UIHelper.AddClickToGameObject(charPortrait, MoveCameraToCharacter, EventTriggerType.PointerClick, character);

            return charPortrait;
        }


        private void UpdateBattleLogText()
        {
            //var battleLogContent = GameObject.FindGameObjectWithTag("BattleLogContent");
            //UIHelper.UpdateTextComponent(EventLogPanel, "BattleLogText", battleGame.battleLog.ToString());

            BattleLogText.text = battleGame.battleLog.ToString();
                

            //var battleLogScrollRect = GameObject.FindGameObjectWithTag("BattleLogScrollRect").GetComponent<ScrollRect>();
            //battleLogScrollRect.verticalNormalizedPosition = 0;

        }

        private void UpdateActionButton()
        {
            switch (uiState)
            {
                case UIStateType.EnemyDecide:
                    UpdateAllButtonStatus(false);
                    break; 
                case UIStateType.EnemyExecute:
                    UpdateAllButtonStatus(false);
                    break; 
                case UIStateType.NewTurn:
                      UpdateAllButtonStatus(false);
                    break; 
                case UIStateType.PlayerDecide:
                    switch (playerDecideState)
                    {
                        case PlayerDecideState.Waiting:
                            UpdateAllButtonStatus(true);
                            break;
                        case PlayerDecideState.MovePendingClick:
                               UpdateAllButtonStatus(false);
                            MoveActionButton.interactable = true;
                            break;
                        case PlayerDecideState.AttackPendingClick:
                        case PlayerDecideState.RangedAttackPendingClick:
                            UpdateAllButtonStatus(false);
                            AttackActionButton.interactable = true;
                            break;
                        case PlayerDecideState.AbilityPendingClick:
                        case PlayerDecideState.AbilityPanelShow:
                            UpdateAllButtonStatus(false);
                            AbilityActionButton.interactable = true;
                            break;
                        case PlayerDecideState.ItemPendingClick:
                        case PlayerDecideState.ItemPanelShow:
                            UpdateAllButtonStatus(false);
                            ItemActionButton.interactable = true;
                            break;
                        default:
                            UpdateAllButtonStatus(false);
                            break;
                    }
                    break; 
                case UIStateType.PlayerExecute:
                    UpdateAllButtonStatus(false);
                    break;
                default:
                    break;
            }
        }

        private void UpdateActionIcons()
        {
            ClearActionIcons();
            switch (playerDecideState)
            {
                case PlayerDecideState.AbilityPendingClick:
                case PlayerDecideState.AbilityPanelShow:
                    UpdateActionIconsAbilities();
                    break;
                case PlayerDecideState.ItemPendingClick:
                case PlayerDecideState.ItemPanelShow:
                    UpdateActionIconsItems();
                    break; 
                default:
                    break;
            }
        }

        private void ClearActionIcons()
        {
            UIHelper.DestroyAllChildren(ActionIconPanel.transform);
        }

        private void UpdateActionIconsAbilities()
        {
            var abilityList = battleGame.ActiveCharacter.abilityList.Take(10);
            foreach (var ability in abilityList)
            {
                Ability a = ability;
                GameObject actionIcon = Instantiate(ActionIconPrefab);
                UIHelper.UpdateSpriteComponent(actionIcon, "Image", gameDataObject.assetLibrary.getSprite(ability.sheetname, ability.spriteindex));
                Button actionButton = actionIcon.GetComponent<Button>();
                actionButton.onClick.AddListener(() => PlayerAbilityStart(a));

                actionIcon.transform.SetParent(ActionIconPanel.transform);
            }
            
        }

        private void UpdateActionIconsItems()
        {
            var itemList = battleGame.ActiveCharacter.usableItemList.Take(10);
            itemList = itemList.GroupBy(x => x.ID).Select(x=>x.First());

            foreach (var item in itemList)
            {
                Item i = item;
                GameObject actionIcon = Instantiate(ActionIconPrefab);
                UIHelper.UpdateSpriteComponent(actionIcon, "Image", gameDataObject.assetLibrary.getSprite(item.sheetname, item.spriteindex));
                Button actionButton = actionIcon.GetComponent<Button>();
                actionButton.onClick.AddListener(() => PlayerItemStart((UsableItem)i));

                actionIcon.transform.SetParent(ActionIconPanel.transform);
            }
        }

        private void UpdatePendingAction()
        {
            ClearPendingAction();
            if (uiState == UIStateType.PlayerDecide)
            {
                switch (playerDecideState)
                {
                    case PlayerDecideState.MovePendingClick:
                        UpdatePendingActionMove();
                        break;
                    case PlayerDecideState.AttackPendingClick:
                        UpdatePendingActionAttack();
                        break;
                    case PlayerDecideState.RangedAttackPendingClick:
                        UpdatePendingRangedActionAttack();
                        break;
                    case PlayerDecideState.AbilityPendingClick:
                        UpdatePendingActionAbility();
                        break;
                    case PlayerDecideState.ItemPendingClick:
                        UpdatePendingItemAbility();
                        break;
                    default:
                        break;
                }
            }
        }

        private void ClearPendingAction()
        {
            Destroy(PendingActionPanel);
        }

        private void UpdatePendingActionData(Sprite icon, string name, string stats, string cost)
        {
            PendingActionPanel = Instantiate(PendingActionPrefab);
            PendingActionPanel.transform.SetParent(BattlePanel.transform);
            UIHelper.UpdateTextComponent(PendingActionPanel, "ActionName", name);
            UIHelper.UpdateTextComponent(PendingActionPanel, "ActionStats", stats);
            UIHelper.UpdateTextComponent(PendingActionPanel, "ActionCost", cost);

            UIHelper.UpdateSpriteComponent(PendingActionPanel, "ActionIcon", icon);

            PendingActionPanel.transform.localPosition = GameConfig.PendingActionPanelLocation;
        }

        private void UpdatePendingActionMove()
        {
            string name = "Move";
            string stats = "";
            string cost = String.Format("AP: {0} / {1}", highlightTiles.Count, battleGame.ActiveCharacter.ap);
            Sprite icon = gameDataObject.assetLibrary.getSprite("Blank", 0);

            UpdatePendingActionData(icon, name, stats, cost);
        }

        private void UpdatePendingActionAttack()
        {
            string name = "Melee Attack";
            Weapon w = battleGame.ActiveCharacter.weapon;
            Sprite icon = gameDataObject.assetLibrary.getSprite(w.sheetname, w.spriteindex);
            string stats = w.ToString();
            string cost = String.Format("AP: {0} / {1}", highlightTiles.Count + w.actionPoints, battleGame.ActiveCharacter.ap);

            UpdatePendingActionData(icon, name, stats, cost);
        }

        private void UpdatePendingRangedActionAttack()
        {
            string name = "Ranged Attack";
            RangedWeapon w = (RangedWeapon)battleGame.ActiveCharacter.weapon;
            Sprite icon = gameDataObject.assetLibrary.getSprite(w.sheetname, w.spriteindex);
            string stats = w.ToString();
            string cost = String.Format("AP: {0} / {1}", w.actionPoints, battleGame.ActiveCharacter.ap);

            UpdatePendingActionData(icon, name, stats, cost);
        }

        private void UpdatePendingActionAbility()
        {
            string name = selectedAbility.name;
         
            string stats = string.Format("{0} Cooldwn: {1} / {2} Rng: {3} Target: {4} ",selectedAbility.description, selectedAbility.cooldownTimer, selectedAbility.cooldown,selectedAbility.range,selectedAbility.targetType.ToString());
            string cost = String.Format("AP: {0} / {1}", selectedAbility.ap, battleGame.ActiveCharacter.ap);
            Sprite icon = gameDataObject.assetLibrary.getSprite(selectedAbility.sheetname, selectedAbility.spriteindex);

            UpdatePendingActionData(icon, name, stats, cost);
        }

        private void UpdatePendingItemAbility()
        {
            string name = selectedItem.name;
            string stats =selectedItem.ToString();

            int count = battleGame.ActiveCharacter.usableItemList.Count(x => x.ID == selectedItem.ID);

            stats += "Count: " + count;

            string cost = String.Format("AP: {0} / {1}", selectedItem.actionPoints, battleGame.ActiveCharacter.ap);
            Sprite icon = gameDataObject.assetLibrary.getSprite(selectedItem.sheetname, selectedItem.spriteindex);

            UpdatePendingActionData(icon, name, stats, cost);
        }

        private void UpdateSelectedCharacter()
        {
            ClearSelectedCharacter();
            if (SelectedCharacter != null)
            {
                SelectedCharacterPanel = Instantiate(CharacterPanelPrefab);

                SelectedCharacterPanel.transform.SetParent(BattlePanel.transform);

                UIHelper.UpdateSpriteComponent(SelectedCharacterPanel, "CharacterPortrait", gameDataObject.assetLibrary.getSprite(SelectedCharacter.portraitSpritesheetName, SelectedCharacter.portraitSpriteIndex));
                UIHelper.UpdateTextComponent(SelectedCharacterPanel, "CharacterName", SelectedCharacter.name);
                UIHelper.UpdateTextComponent(SelectedCharacterPanel, "HPText", string.Format("HP:{0}/{1}", SelectedCharacter.hp, SelectedCharacter.totalHP));
                UIHelper.UpdateTextComponent(SelectedCharacterPanel, "APText", string.Format("AP:{0}/{1}", SelectedCharacter.ap, SelectedCharacter.totalAP));

                UIHelper.UpdateSliderValue(SelectedCharacterPanel, "APSlider", (float)SelectedCharacter.ap / (float)SelectedCharacter.totalAP);

                UIHelper.UpdateSliderValue(SelectedCharacterPanel, "HPSlider", (float)SelectedCharacter.hp / (float)SelectedCharacter.totalHP);

               SelectedCharacterPanel.transform.localPosition = GameConfig.SelectedCharacterPanelLocation;

               var effectPanel = UIHelper.getChildObject(SelectedCharacterPanel, "EffectPanel");
               UpdateEffectIcons(effectPanel, SelectedCharacter.activeEffects);
            }
        }

        private void ClearSelectedCharacter()
        {
            Destroy(SelectedCharacterPanel);
        }

        private void UpdateVictoryPanel()
        {
            BattleTreeNode winNode = battleTree.getWinNode();
       
            UIHelper.UpdateTextComponent(VictoryPanel, "VictoryText", winNode.content.description);
            var itemList = battleTree.getWinItemList(gameDataObject.gameDataSet);
            string itemStr = "";
            foreach(var item in itemList){
                itemStr += item.name + "\n";
            }
            UIHelper.UpdateTextComponent(VictoryPanel, "LootText", itemStr);
            UIHelper.UpdateTextComponent(VictoryPanel, "XPText", battleTree.getWinXP() + " XP");

        }

        private void UpdateGameOverPanel()
        {
            UIHelper.UpdateTextComponent(VictoryPanel, "TitleText", "Defeat!");
            UIHelper.UpdateTextComponent(VictoryPanel, "VictoryText", "All is lost...");
            UIHelper.UpdateTextComponent(VictoryPanel, "LootText", "");
            UIHelper.UpdateTextComponent(VictoryPanel, "XPText","");

            Button closeButton =UIHelper.getButton(VictoryPanel, "CloseButton");
            closeButton.onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            closeButton.onClick.AddListener(CloseDefeatPanel);
        }

        #endregion

        #region Update

        // Update is called once per frame
        void Update()
        {

            //Highlight the selected tile + pending action tiles
            DisplayMouseTile();

            float delta = Time.deltaTime;

            if (battleStatus != BattleStatusType.GameOver)
            {
                battleStatus = battleGame.getBattleStatus();
            }

            if (battleStatus == BattleStatusType.Running)
            {

                UpdateTempEffects(delta);

                UITimer -= Time.deltaTime;
                if (UITimer <= 0)
                {
                    UpdateBattle();
                    UITimer = getUpdateBattleTimer();
                }

                UpdateCamera();
                ClickTile();
            }
            else if (battleStatus == BattleStatusType.PlayersDead)
            {
                LoseBattle();
            }
            else if (battleStatus == BattleStatusType.EnemiesDead)
            {
                WinBattle();
            }

        }

        //Reloads / Displays all UI elements from scratch
        private void UpdateUI()
        {
            //Active Character
            UpdateActiveCharacterPanel();

            //Initiative List
            UpdateInitiativePanel();

            //Battle Log
            UpdateBattleLogText();

            //Action Buttons
            UpdateActionButton();

            //Action Icons
            UpdateActionIcons();

            //Pending Action Panel
            UpdatePendingAction();

            //Selected Character Panel
            UpdateSelectedCharacter();

		//UpdateDebug ();
        }

        private void UpdateAllButtonStatus(bool flag)
        {
            MoveActionButton.interactable = flag;
            AttackActionButton.interactable = flag;
            AbilityActionButton.interactable = flag;
            ItemActionButton.interactable = flag;
            EndTurnActionButton.interactable = flag;
        }

        public void ClickActionButton(int button)
        {
            clickPoint = null;

            switch (button)
            {
                case 0:
                    if (playerDecideState == PlayerDecideState.MovePendingClick)
                    {
                        playerDecideState = PlayerDecideState.Waiting;
                    }
                    else
                    {
                        playerDecideState = PlayerDecideState.MovePendingClick;
                    }
                    break;
                case 1:
                    if (playerDecideState == PlayerDecideState.AttackPendingClick || playerDecideState == PlayerDecideState.RangedAttackPendingClick)
                    {
                        playerDecideState = PlayerDecideState.Waiting;
                    }
                    else
                    {
                        if (battleGame.ActiveCharacter.weapon.weaponType == WeaponType.OneHandRanged || battleGame.ActiveCharacter.weapon.weaponType == WeaponType.TwoHandRanged)
                        {
                            playerDecideState = PlayerDecideState.RangedAttackPendingClick;
                        }
                        else
                        {
                            playerDecideState = PlayerDecideState.AttackPendingClick;
                        }
                    }
                    break;
                case 2:
                    if (playerDecideState == PlayerDecideState.AbilityPanelShow || playerDecideState == PlayerDecideState.AbilityPendingClick)
                    {
                        playerDecideState = PlayerDecideState.Waiting;
                    }
                    else
                    {
                        playerDecideState = PlayerDecideState.AbilityPanelShow;
                    }
                    break;
                case 3:
                    if (playerDecideState == PlayerDecideState.ItemPanelShow || playerDecideState == PlayerDecideState.ItemPendingClick)
                    {
                        playerDecideState = PlayerDecideState.Waiting;
                    }
                    else
                    {
                        playerDecideState = PlayerDecideState.ItemPanelShow;
                    }
                    break;
                case 4:
                    PlayerEndTurn();
                    break;
                default:
                    break;
            }

            UpdateUI();
        }

        private void ClickTile()
        {
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0)) //left click
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    var mouseTilePt = getTileLocationFromVectorPos(mouseWorldPosition);
                    if (mouseTilePt != null)
                    {
                        this.clickPoint = new Point(mouseTilePt.x, -mouseTilePt.y);

                    }
                    if (clickPoint != null)
                    {
                        SelectCharacter(clickPoint);
                        SelectTile();
                    }
                    else
                    {
                        DeselectTile();
                    }
                }

            }
            else if (Input.GetMouseButtonDown(1)) //right click
            {
                if (uiState == UIStateType.PlayerDecide)
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        if (playerDecideState == PlayerDecideState.AbilityPendingClick ||
                           playerDecideState == PlayerDecideState.AttackPendingClick ||
                            playerDecideState == PlayerDecideState.ItemPendingClick ||
                           playerDecideState == PlayerDecideState.MovePendingClick ||
                            playerDecideState == PlayerDecideState.RangedAttackPendingClick
                           )
                        {
                            CancelPendingAction();
                        }
                        else
                        {
                            var mouseTilePt = getTileLocationFromVectorPos(mouseWorldPosition);
                            if (mouseTilePt != null)
                            {
                                this.clickPoint = new Point(mouseTilePt.x, -mouseTilePt.y);
                            }
                            if (clickPoint != null)
                            {
                                SelectTile();
                                PlayerSmartAttackOrMove();
                            }
                        }

                    }
                }

            }
        }

        private void SelectTile()
        {
            Destroy(SelectedTile);
            SelectedTile = new GameObject("SelectedTile");
            SpriteRenderer renderer = SelectedTile.AddComponent<SpriteRenderer>();
            renderer.sortingOrder = 1;

            SelectedTile.transform.position = getWorldPosFromTilePoint(new Point(clickPoint.x, -clickPoint.y));

            renderer.sprite = gameDataObject.assetLibrary.getSprite("HighlightTile", 0);

        }

        public void SelectCharacter(Point p)
        {
            Tile pointTile = battleGame.board.getTileFromLocation(p.x, p.y);
            
            if (pointTile != null)
            {
                SelectedCharacter = battleGame.getCharacterFromTile(pointTile);
                if (SelectedCharacter != null)
                {
                    UpdateUI();
                }
            }

        }

        public void DeselectCharacter()
        {
            SelectedCharacter = null;
            UpdateUI();
        }

        private void DeselectTile()
        {
            Destroy(SelectedTile);
            SelectedTile = null;
        }

        private float getUpdateBattleTimer()
        {
            if (battleGame.ActiveCharacter.type == CharacterType.Enemy)
            {
                return GameConfig.enemyUpdateBattleTimer;
            }
            else
            {
                return GameConfig.playerUpdateBattleTimer;

            }

        }

        public void CloseVictoryPanel()
        {
            //Select win node, run actions
            var winNode = battleTree.getWinNode();
            winNode.SelectNode(battleTree);


            gameDataObject.runActions(winNode.actionList);
            //switch back to parent tree link
            gameDataObject.treeStore.SelectTree(parentTreeLink);
            //go back to the zone view
            Application.LoadLevel((int)UnitySceneIndex.Zone);
        }

        public void CloseDefeatPanel()
        {
            Destroy(gameDataObject);
            Application.LoadLevel((int)UnitySceneIndex.Start);
        }

      
        //DEPRECATED
        void UpdateDebug()
        {
            Vector3 viewPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var canvas = GameObject.FindGameObjectWithTag("FrontCanvas");

            Vector2 canvasPos = Input.mousePosition - canvas.transform.localPosition;

            string mousePos = string.Format("Mouse: {0},{1} | World: {2},{3} | view {4},{5} | canvas {6},{7}",
                Input.mousePosition.x, Input.mousePosition.y, viewPos.x, viewPos.y, worldPos.x, worldPos.y, canvasPos.x, canvasPos.y);
            DebugText.text = mousePos;

        }

        void UpdateBattle()
        {

            battleGame.board.ClearTempTiles();

            //Update Map
            LoadCharacters();

            UpdateUI();

            switch (uiState)
            {
                case UIStateType.NewTurn:

                    UpdateNewTurn();
                    break;
                case UIStateType.PlayerDecide:

                    UpdatePlayerDecide();
                    break;
                case UIStateType.PlayerExecute:

                    UpdateBattleActions();
                    break;
                case UIStateType.EnemyDecide:
                    UpdateEnemyDecide();
                    break;
                case UIStateType.EnemyExecute:

                    UpdateBattleActions();
                    break;
                default:
                    break;
            }
        }

        void UpdateNewTurn()
        {
            FocusCamera(battleGame.ActiveCharacter.x, -battleGame.ActiveCharacter.y);

            //Update Map
            LoadCharacters();

            battleGame.NewTurn = false;
            battleGame.ActiveCharacter.RunActiveEffects(battleGame);

            if (battleGame.ActiveCharacter.type == CharacterType.Player)
            {
                uiState = UIStateType.PlayerDecide;
                playerDecideState = PlayerDecideState.Waiting;
            }
            else
            {
                uiState = UIStateType.EnemyDecide;
            }
        }

        void UpdatePlayerDecide()
        {
            switch (playerDecideState)
            {
                case PlayerDecideState.Waiting:
                    break;
                case PlayerDecideState.MovePendingClick:
                    PlayerMoveSelect();
                    break;
                case PlayerDecideState.AttackPendingClick:
                    PlayerAttackSelect();
                    break;
                case PlayerDecideState.RangedAttackPendingClick:
                    PlayerRangedAttackSelect();
                    break;
                case PlayerDecideState.AbilityPendingClick:
                    PlayerAbilitySelect();
                    break;
                case PlayerDecideState.ItemPendingClick:
                    PlayerItemSelect();
                    break;
                default:
                    break;
            }
        }

        void UpdateEnemyDecide()
        {
            battleGame.actionQueue = battleGame.getEnemyActionList();
            uiState = UIStateType.EnemyExecute;
        }

        void UpdateBattleActions()
        {
            FocusCamera(battleGame.ActiveCharacter.x, -battleGame.ActiveCharacter.y);

            if (battleGame.ActiveCharacter.hp > 0)
            {
                if (battleGame.ActiveCharacter.type == CharacterType.Player)
                {
                    if (battleGame.actionQueue.Count > 0)
                    {
                        battleGame.RunActionQueue();
                    }
                    else
                    {
                        uiState = UIStateType.PlayerDecide;
                    }
                }
                else
                {
                    if (battleGame.actionQueue.Count > 0)
                    {
                        if (!battleGame.RunActionQueue())
                        {
                            battleGame.NextTurn();
                            uiState = UIStateType.NewTurn;
                        }
                    }
                    else
                    {
                        uiState = UIStateType.EnemyDecide;

                        //battleGame.NextTurn();
                        //uiState = UIStateType.NewTurn;
                    }
                }
            }
            else
            {
                battleGame.CharacterKill(battleGame.ActiveCharacter);
                battleGame.NextTurnActiveDied();
                uiState = UIStateType.NewTurn;
            }
        }

        public void LoseBattle()
        {
            battleStatus = BattleStatusType.GameOver;
            UpdateGameOverPanel();
            VictoryPanel.transform.localPosition = new Vector3(0, 0, 0);
        }

        public void WinBattle()
        {
            battleStatus = BattleStatusType.GameOver;
            UpdateVictoryPanel();
            VictoryPanel.transform.localPosition = new Vector3(0, 0, 0);
        }

        #endregion

        #region PlayerActions

        //Create battle actions here, or lower level?
        public void PlayerEndTurn()
        {
            if (uiState == UIStateType.PlayerDecide)
            {
                battleGame.battleLog.AddEntry(string.Format("{0} ended turn.", battleGame.ActiveCharacter.name));

                battleGame.NextTurn();
                uiState = UIStateType.NewTurn;
            }
        }

      
        public void PlayerMoveSelect()
        {
            if (clickPoint != null)
            {
                playerDecideState = PlayerDecideState.Waiting;
                battleGame.actionQueue.AddRange(battleGame.GetMoveActionList(clickPoint.x, clickPoint.y));
                uiState = UIStateType.PlayerExecute;
                
                clickPoint = null;
                UpdateUI();
            }
        }

        //Right Click - smart attack or move
        //Determine if enemy was clicked on to attack, otherwise move to spot
        public void PlayerSmartAttackOrMove()
        {
            if (clickPoint != null)
            {
                Tile clickTile = battleGame.board.getTileFromPoint(clickPoint);
                GameCharacter enemyChar = battleGame.getCharacterFromTile(clickTile);
                if (enemyChar != null && enemyChar.type == CharacterType.Enemy)
                {
                    if (battleGame.ActiveCharacter.weapon != null)
                    {
                        if (battleGame.ActiveCharacter.weapon.weaponType == WeaponType.OneHandRanged || battleGame.ActiveCharacter.weapon.weaponType == WeaponType.TwoHandRanged)
                        {
                            PlayerRangedAttackSelect();
                        }
                        else
                        {
                            PlayerAttackSelect();
                        }

                    }
                }
                else
                {
                    PlayerMoveSelect();
                }
            }
        }

       
      

        public void PlayerAttackSelect()
        {
            if (clickPoint != null)
            {
                playerDecideState = PlayerDecideState.Waiting;
                battleGame.actionQueue.AddRange(battleGame.getAttackActionList(clickPoint.x, clickPoint.y));
                uiState = UIStateType.PlayerExecute;
            }
        }

     

        public void PlayerRangedAttackSelect()
        {
            if (clickPoint != null)
            {
                playerDecideState = PlayerDecideState.Waiting;
                battleGame.actionQueue.AddRange(battleGame.getRangedAttackActionList(clickPoint.x, clickPoint.y));
                uiState = UIStateType.PlayerExecute;
            }
        }

        public void PlayerItemStart(UsableItem selectedItem)
        {
            if (uiState == UIStateType.PlayerDecide)
            {
                clickPoint = null;
                this.selectedItem = selectedItem;
                playerDecideState = PlayerDecideState.ItemPendingClick;

                UpdateUI();
            }
        }

        public void PlayerItemSelect()
        {
            if (clickPoint != null)
            {
                playerDecideState = PlayerDecideState.Waiting;
                battleGame.actionQueue.AddRange(battleGame.getItemActionList(selectedItem, clickPoint.x, clickPoint.y));
                uiState = UIStateType.PlayerExecute;
            }
        }


        public void PlayerAbilityStart(Ability selectedAbility)
        {
            if (uiState == UIStateType.PlayerDecide)
            {
                clickPoint = null;
                this.selectedAbility = selectedAbility;
                playerDecideState = PlayerDecideState.AbilityPendingClick;

                UpdateUI();
            }
        }

        public void PlayerAbilitySelect()
        {
            if (clickPoint != null)
            {
                playerDecideState = PlayerDecideState.Waiting;
                battleGame.actionQueue.AddRange(battleGame.getAbilityActionList(selectedAbility, clickPoint.x, clickPoint.y));
                uiState = UIStateType.PlayerExecute;
            }
        }

        public void CancelPendingAction()
        {
            uiState = UIStateType.PlayerDecide;
            playerDecideState = PlayerDecideState.Waiting;
        }

        #endregion


        #region Camera

        //Init the camera to middle of game board
        private void SetCamera()
        {
            cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
            mainCamera = cameraObject.GetComponent<Camera>();

            int x = (int)Mathf.Round(battleGame.board.board.GetLength(0) / 2f);
            int y = -(int)Mathf.Round(battleGame.board.board.GetLength(1) / 2f);

            cameraObject.transform.position = new Vector3(x, y, -10);

            cameraData = new BattleSceneCameraData(x, y, mainCamera.orthographicSize, GameConfig.PanLerp, GameConfig.ZoomLerp);

        }

        public void MoveCameraToCharacter(System.Object characterObject)
        {
            GameCharacter gc = (GameCharacter)characterObject;
            FocusCamera(gc.x, -gc.y);

            clickPoint = new Point(gc.x, gc.y);
            SelectTile();
            SelectCharacter(clickPoint);
            clickPoint = null;
        }

        private void UpdateCamera()
        {
            var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

            float vert = Input.GetAxis("Vertical") * GameConfig.PanSpeed;
            float hor = Input.GetAxis("Horizontal") * GameConfig.PanSpeed;

            var destination = new Vector2(cameraData.CameraX + hor, cameraData.CameraY + vert);
            //var fixedDestination = MoveCamera(mainCamera, mainCamera.transform.position, destination);
            cameraData.SetCamera(destination.x, destination.y);
            //cameraData.SetCamera(fixedDestination.x, fixedDestination.y);

            mainCamera.transform.position = cameraData.UpdateCamera(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);

            UpdateCameraScroll(mainCamera);


        }

        private void UpdateCameraScroll(GameObject mainCameraObject)
        {
            var camera = mainCameraObject.GetComponent<Camera>();

            var newZoom = Input.GetAxis("Mouse ScrollWheel") * -1;

            cameraData.SetZoom(Mathf.Clamp(cameraData.Zoom + newZoom * GameConfig.ZoomFactor, GameConfig.MinZoom, GameConfig.MaxZoom));

            camera.orthographicSize = cameraData.UpdateZoom(camera.orthographicSize);

        }

        private void FocusCamera(int x, int y)
        {
            var cameraPos = getWorldPosFromTilePoint(new Point(x, y));
            cameraData.SetCamera(cameraPos.x, cameraPos.y);
        }

        //http://answers.unity3d.com/questions/501893/calculating-2d-camera-bounds.html
        private Vector3 MoveCamera(GameObject camera, Vector3 oldPos, Vector3 newPos)
        {

            float vertExtent = Camera.main.GetComponent<Camera>().orthographicSize;
            float horzExtent = vertExtent * Screen.width / Screen.height;

            var vertExtentTop = vertExtent - 0.5f;
            var vertExtentBottom = vertExtent * .4f;

            horzExtent -= 1;

            float mapX = battleGame.board.board.GetLength(0);
            float mapY = battleGame.board.board.GetLength(1);

            float minX = horzExtent + -0.5f;
            float maxX = (mapX + -0.5f) - horzExtent;

            float minY = vertExtentBottom + -0.5f;
            float maxY = (mapY + -0.5f) - vertExtentTop;

            var v3 = oldPos;

            if (mapX > horzExtent * 2)
            {
                v3.x = Mathf.Clamp(newPos.x, minX, maxX);
            }

            if (mapY > vertExtent)
            {
                v3.y = Mathf.Clamp(newPos.y, minY, maxY);
            }

            return v3;
        }


        #endregion



        #region HighlightTiles

        //testing
        private void displayCollisionSprites()
        {


            for (int i = 0; i < battleGame.board.board.GetLength(0); i++)
            {
                for (int j = 0; j < battleGame.board.board.GetLength(1); j++)
                {
                    var tileSquare = Instantiate(SpritePrefab);
                    tileSquare.transform.position = getWorldPosFromTilePoint(new Point(i, -j));

                    var tileSquareSprite = tileSquare.GetComponent<SpriteRenderer>();
                    tileSquareSprite.sprite = gameDataObject.assetLibrary.getSprite("Tile64", 0);

                    if (!battleGame.board.board[i, j].empty)
                    {

                        tileSquareSprite.color = GameConfig.transRed;

                    }
                    else
                    {
                        tileSquareSprite.color = GameConfig.transWhite;
                    }
                }
            }
        }

        private void DisplayMouseTile()
        {
            Destroy(mouseOverTile);

            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var mouseTilePt = getTileLocationFromVectorPos(mouseWorldPosition);
            if (mouseTilePt != null)
            {
                hoverPoint = new Point(mouseTilePt.x, mouseTilePt.y);

                mouseOverTile = Instantiate(SpritePrefab);
                mouseOverTile.transform.position = getWorldPosFromTilePoint(hoverPoint);
                var tileSquareSprite = mouseOverTile.GetComponent<SpriteRenderer>();
                tileSquareSprite.sprite = gameDataObject.assetLibrary.getSprite("Tile64", 0);

                if (uiState == UIStateType.PlayerDecide &&
                    (playerDecideState == PlayerDecideState.AbilityPendingClick ||
                    playerDecideState == PlayerDecideState.AttackPendingClick ||
                    playerDecideState == PlayerDecideState.ItemPendingClick ||
                    playerDecideState == PlayerDecideState.MovePendingClick ||
                    playerDecideState == PlayerDecideState.RangedAttackPendingClick))
                {
                    tileSquareSprite.color = Color.cyan;

                }
                else
                {
                    tileSquareSprite.color = Color.blue;
                }

            }

            DisplayHighlightedTiles();

        }

        //when actions are pending, show the highlighted tiles this ability/item/action will take
        //abstract the tileList stuff to battle game?
        private void DisplayHighlightedTiles()
        {
            foreach (var t in highlightTiles)
            {
                Destroy(t);
            }
            highlightTiles.Clear();

            Point fixedHoverPoint = new Point(hoverPoint.x, -hoverPoint.y);
            List<Point> pathFind;
            Tile origin;
            Tile dest;

            if (uiState == UIStateType.PlayerDecide)
            {
                switch (playerDecideState)
                {
                    case PlayerDecideState.MovePendingClick:

                        pathFind = PathFind.Pathfind(battleGame.board, battleGame.ActiveCharacter.x, battleGame.ActiveCharacter.y, hoverPoint.x, -hoverPoint.y);
                        if (pathFind.Count > battleGame.ActiveCharacter.ap)
                        {
                            AddHighlightTiles(pathFind, Color.red);
                        }
                        else
                        {
                            AddHighlightTiles(pathFind, Color.green);
                        }

                        break;
                    case PlayerDecideState.AttackPendingClick:
                        pathFind = PathFind.Pathfind(battleGame.board, battleGame.ActiveCharacter.x, battleGame.ActiveCharacter.y, hoverPoint.x, -hoverPoint.y);
                        if (pathFind.Count + battleGame.ActiveCharacter.weapon.actionPoints > battleGame.ActiveCharacter.ap)
                        {
                            AddHighlightTiles(pathFind, Color.red);
                        }
                        else
                        {
                            AddHighlightTiles(pathFind, Color.green);
                        }

                        break;
                    case PlayerDecideState.RangedAttackPendingClick:
                        RangedWeapon w = (RangedWeapon)battleGame.ActiveCharacter.weapon;
                        origin = battleGame.board.getTileFromLocation(battleGame.ActiveCharacter.x, battleGame.ActiveCharacter.y);
                        dest = battleGame.board.getTileFromPoint(fixedHoverPoint);
                        pathFind = battleGame.board.getBoardLOSPointList(origin, dest);

                        if (battleGame.board.checkLOS(origin, dest) && pathFind.Count <= w.range && w.actionPoints <= battleGame.ActiveCharacter.ap)
                        {
                            AddHighlightTiles(pathFind, Color.green);
                        }
                        else { AddHighlightTiles(pathFind, Color.red); }
                        break;
                    case PlayerDecideState.AbilityPendingClick:
                        DisplayHighlightTilesForAbility(selectedAbility);
                        break;
                    case PlayerDecideState.ItemPendingClick:
                        DisplayHighlightTilesForItem(selectedItem);
                        break;
                    default:
                        break;
                }
            }
        }

        private void DisplayHighlightTilesForItem(UsableItem item)
        {
            Point fixedHoverPoint = new Point(hoverPoint.x, -hoverPoint.y);
            Tile origin;
            Tile dest;
            List<Point> pointList = new List<Point>();

            if (item.itemAbility != null)
            {


                switch (item.itemAbility.targetType)
                {
                    case AbilityTargetType.AllFoes:
                        break;
                    case AbilityTargetType.AllFriends:
                        break;
                    case AbilityTargetType.LOSEmpty:
                        origin = battleGame.board.getTileFromLocation(battleGame.ActiveCharacter.x, battleGame.ActiveCharacter.y);
                        dest = battleGame.board.getTileFromPoint(fixedHoverPoint);
                        pointList = battleGame.board.getBoardLOSPointList(origin, dest);
                        if (battleGame.board.checkLOS(origin, dest) && pointList.Count <= item.itemAbility.range && item.actionPoints <= battleGame.ActiveCharacter.ap)
                        {
                            AddHighlightTiles(pointList, Color.green);
                        }
                        else { AddHighlightTiles(pointList, Color.red); }
                        break;
                    case AbilityTargetType.LOSTarget:
                        //Also need to make sure destination is a game character
                        origin = battleGame.board.getTileFromLocation(battleGame.ActiveCharacter.x, battleGame.ActiveCharacter.y);
                        dest = battleGame.board.getTileFromPoint(fixedHoverPoint);
                        pointList = battleGame.board.getBoardLOSPointList(origin, dest);
                        if (battleGame.board.checkLOS(origin, dest) && pointList.Count <= item.itemAbility.range && item.actionPoints <= battleGame.ActiveCharacter.ap)
                        {
                            AddHighlightTiles(pointList, Color.green);
                        }
                        else { AddHighlightTiles(pointList, Color.red); }
                        break;

                    case AbilityTargetType.PointEmpty:
                        pointList.Add(fixedHoverPoint);
                        AddHighlightTiles(pointList, Color.green);
                        break;
                    case AbilityTargetType.PointTarget:
                        //verify target is character
                        pointList.Add(fixedHoverPoint);
                        AddHighlightTiles(pointList, Color.green);
                        break;
                    case AbilityTargetType.Self:
                        break;
                    case AbilityTargetType.SingleFoe:
                        break;
                    case AbilityTargetType.SingleFriend:
                        break;
                    default: break;
                }
            }

        }

        private void DisplayHighlightTilesForAbility(Ability ability)
        {
            Point fixedHoverPoint = new Point(hoverPoint.x, -hoverPoint.y);
            Tile origin;
            Tile dest;
            List<Point> pointList = new List<Point>();

            switch (ability.targetType)
            {
                case AbilityTargetType.AllFoes:
                    break;
                case AbilityTargetType.AllFriends:
                    break;
                case AbilityTargetType.LOSEmpty:
                    origin = battleGame.board.getTileFromLocation(battleGame.ActiveCharacter.x, battleGame.ActiveCharacter.y);
                    dest = battleGame.board.getTileFromPoint(fixedHoverPoint);
                    pointList = battleGame.board.getBoardLOSPointList(origin, dest);
                    if (battleGame.board.checkLOS(origin, dest) && pointList.Count <= ability.range && ability.ap <= battleGame.ActiveCharacter.ap)
                    {
                        AddHighlightTiles(pointList, Color.green);
                    }
                    else { AddHighlightTiles(pointList, Color.red); }
                    break;
                case AbilityTargetType.LOSTarget:
                    //Also need to make sure destination is a game character
                    origin = battleGame.board.getTileFromLocation(battleGame.ActiveCharacter.x, battleGame.ActiveCharacter.y);
                    dest = battleGame.board.getTileFromPoint(fixedHoverPoint);
                    pointList = battleGame.board.getBoardLOSPointList(origin, dest);
                    if (battleGame.board.checkLOS(origin, dest) && pointList.Count <= ability.range && ability.ap <= battleGame.ActiveCharacter.ap)
                    {
                        AddHighlightTiles(pointList, Color.green);
                    }
                    else { AddHighlightTiles(pointList, Color.red); }
                    break;

                case AbilityTargetType.PointEmpty:
                    pointList.Add(fixedHoverPoint);
                    AddHighlightTiles(pointList, Color.green);
                    break;
                case AbilityTargetType.PointTarget:
                    //verify target is character
                    pointList.Add(fixedHoverPoint);
                    AddHighlightTiles(pointList, Color.green);
                    break;
                case AbilityTargetType.Self:
                    break;
                case AbilityTargetType.SingleFoe:
                    break;
                case AbilityTargetType.SingleFriend:
                    break;
                default: break;
            }
        }

        private void AddHighlightTiles(List<Point> tilePointList, Color c)
        {
            foreach (var t in tilePointList)
            {
                Point fixPoint = new Point(t.x, -t.y);
                GameObject highlightTile = Instantiate(SpritePrefab);
                highlightTile.transform.position = getWorldPosFromTilePoint(fixPoint);
                SpriteRenderer tileSquareSprite = highlightTile.GetComponent<SpriteRenderer>();
                tileSquareSprite.sprite = gameDataObject.assetLibrary.getSprite("Tile64", 0);
                tileSquareSprite.color = c;

                highlightTiles.Add(highlightTile);
            }
        }


        #endregion

        #region Helpers

        private Bounds getTileBounds(int x, int y)
        {
            Vector3 center = new UnityEngine.Vector3(x, y, 0);
            Vector3 size = new UnityEngine.Vector3(Tile.TILE_SIZE, Tile.TILE_SIZE);
            Bounds b = new UnityEngine.Bounds(center, size);
            return b;
        }

        private Point getTileLocationFromVectorPos(Vector3 pos)
        {
		int x = (int)Math.Ceiling (pos.x / Tile.TILE_SIZE - (Tile.TILE_SIZE/2));
		int y = (int)Math.Ceiling (pos.y / Tile.TILE_SIZE);
		//int x = Mathf.RoundToInt(pos.x /  Tile.TILE_SIZE - (Tile.TILE_SIZE/2));
		//int y = Mathf.RoundToInt(pos.y /  Tile.TILE_SIZE + (Tile.TILE_SIZE/2));

		DebugText.text = string.Format ("{0}, {1}", x, y);


            Point retval = null;

            if (x >= 0 && x <= tileMapData.battleTileArray.GetLength(0) && y <= 0 && y >= -tileMapData.battleTileArray.GetLength(1))
            {
                retval = new Point() { x = (int)x, y = (int)y };
            }
            return retval;
        }

        private Vector3 getWorldPosFromTilePoint(Point p)
        {
		return new Vector3(p.x * Tile.TILE_SIZE + (Tile.TILE_SIZE/2), p.y * Tile.TILE_SIZE -(Tile.TILE_SIZE/2) , 0);
        }

        private Vector3 getWorldPosOffset(Vector3 pos)
        {
		return new Vector3(pos.x * Tile.TILE_SIZE, pos.y * Tile.TILE_SIZE, pos.z);
        }

        private Point getBoardPointFromLocation(float x, float y)
        {
			x = x + Tile.TILE_SIZE;
			y = y + Tile.TILE_SIZE;

            Point retval = null;

            if (x >= 0 && x <= battleGame.board.board.GetLength(0) && y >= 0 && y <= battleGame.board.board.GetLength(1))
            {
                retval = new Point() { x = (int)x, y = (int)y };
            }
            return retval;
        }

        private Vector3 getBoardPositionFromScreenPosition(Vector3 pos)
        {
            var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

            var camera = mainCamera.GetComponent<Camera>();

            return camera.ScreenToWorldPoint(pos);
        }

        private Vector3 getMouseViewportCoords()
        {
            var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

            var camera = mainCamera.GetComponent<Camera>();

            return camera.ScreenToViewportPoint(Input.mousePosition);
        }



        #endregion

        #region TempEffects

        private void UpdateTempEffects(float delta)
        {
            for (int i = tempEffectList.Count - 1; i >= 0; i--)
            {
                tempEffectList[i].Update(delta);
                if (tempEffectList[i].isExpired)
                {
                    Destroy(tempEffectList[i].gameObject);
                    tempEffectList.RemoveAt(i);
                }
            }
        }

        private void AddEffect(TempEffectType type, float duration, Vector3 pos, Vector3 dest, GameObject gameObject)
        {
            TempEffect te = new TempEffect(type, gameObject, duration, pos, dest);

            tempEffectList.Add(te);
        }



        public void StartTempParticles(string particleName, Vector3 pos)
        {
            pos = getWorldPosOffset(pos);
            var particleObj = gameDataObject.assetLibrary.getPrefabGameObject(particleName);

            var rend = particleObj.GetComponentInChildren<Renderer>();
            rend.sortingLayerName = "Foreground";

            AddEffect(TempEffectType.Particle, 1, pos, pos, particleObj);

        }

        public void StartTempTextOnChar(GameCharacter gameCharacter, int amount, bool isDamage)
        {
            Color c = Color.green;
            if (isDamage)
            {
                c = Color.red;
            }

            Vector3 charPos = new Vector3(gameCharacter.x, -gameCharacter.y + 0.5f, 0);
            battleGame.battleLog.AddEntry(charPos.ToString());

            StartTempText(charPos, c, amount.ToString());
        }

        public void StartTempSpriteOnChar(GameCharacter gameCharacter, string spritesheet, int spriteindex)
        {
            Vector3 charPos = new Vector3(gameCharacter.x, -gameCharacter.y, 0);
            StartTempSprite(charPos, charPos, spritesheet, spriteindex);
        }

        public void StartTempParticleOnChar(GameCharacter gameCharacter, string particleName)
        {
            Vector3 charPos = new Vector3(gameCharacter.x, -gameCharacter.y, 0);
            StartTempParticles(particleName, charPos);
        }


        public void StartTempText(Vector3 pos, Color c, string text)
        {
            pos = getWorldPosOffset(pos);

            var textObj = gameDataObject.assetLibrary.getPrefabGameObject("TextPopup");
            UpdateTextPopup(textObj, text, c);
            textObj.transform.position = pos;


            Vector3 endPos = new Vector3(pos.x, pos.y + .2f, pos.z);

            AddEffect(TempEffectType.Text, 1, pos, endPos, textObj);
        }

        private void UpdateTextPopup(GameObject textPopup, string text, Color c)
        {
            var textMesh = textPopup.GetComponentInChildren<TextMesh>();
            textMesh.text = text;
            textMesh.color = c;

            var meshRenderer = textPopup.GetComponentInChildren<MeshRenderer>();
            meshRenderer.sortingLayerName = "Foreground";

        }

        public void StartTempSprite(Vector3 pos, Vector3 pos2, string spritesheetName, int spriteIndex)
        {
            pos = getWorldPosOffset(pos);
            pos2 = getWorldPosOffset(pos2);

            var sprite = gameDataObject.assetLibrary.getSprite(spritesheetName, spriteIndex);
            var spriteObj = gameDataObject.assetLibrary.getPrefabGameObject("Sprite");
            spriteObj.transform.position = pos;
            var spriteRenderer = spriteObj.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 3;
            spriteRenderer.sprite = sprite;

            AddEffect(TempEffectType.Sprite, 1, pos, pos2, spriteObj);
        }

        public void StartTempSpriteProjectile(Vector3 pos, Vector3 pos2, string spritesheetName, int spriteIndex)
        {
            pos = getWorldPosOffset(pos);
            pos2 = getWorldPosOffset(pos2);

            var dist = Vector3.Distance(pos, pos2);
            var dir = pos2 - pos;
            float rot = (float)Math.Atan2(dir.y, dir.x);

            var sprite = gameDataObject.assetLibrary.getSprite(spritesheetName, spriteIndex);
            var spriteObj = gameDataObject.assetLibrary.getPrefabGameObject("Sprite");
            spriteObj.transform.position = pos;
            spriteObj.transform.rotation = Quaternion.Euler(0f, 0f, rot * Mathf.Rad2Deg);

            var spriteRenderer = spriteObj.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 3;
            spriteRenderer.sprite = sprite;

            AddEffect(TempEffectType.Sprite, .25f, pos, pos2, spriteObj);
        }

        #endregion

    }




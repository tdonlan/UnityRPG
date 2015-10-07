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

    public GameObject tileMapPrefab;
    public GameObject tileMapObject;

    public TileMapData tileMapData;
    public long parentTreeLink;
    public BattleTree battleTree { get; set; }


    public BattleGameData battleGameData { get; set; }

    public BattleGame battleGame { get; set; }
    public BattleStatusType battleStatus { get; set; }

    public List<GameObject> tileCharacterList { get; set; }
    public List<TempEffect> tempEffectList { get; set; }

    Text DebugText = null;

    public System.Random r { get; set; }

    public Point hoverPoint { get; set; }
    public Point clickPoint { get; set; }
    public List<GameObject> highlightTiles { get; set; }
    GameObject SelectedTile { get; set; }
    
    private GameObject mouseOverTile { get; set; }

    public GameObject HoverStatsObject { get; set; }
    public GameObject CharacterHover { get; set; }
    
    public GameObject PendingActionHover { get; set; }

    public bool isStatsDisplay { get; set; }

    private float UITimer { get; set; }
    private float TempEffectTimer { get; set; }

    private Ability selectedAbility { get; set; }
    private UsableItem selectedItem { get; set; }
    public UIStateType uiState { get; set; }

    public PlayerDecideState playerDecideState { get; set; }

    //Camera Pan / Zoom
    private GameObject cameraObject;
    private Camera mainCamera;
    private BattleSceneCameraData cameraData { get; set; }

    //UI Prefabs
    public Canvas canvas { get; set; }
    public GameObject ItemPrefab { get; set; }
    public GameObject AbilityItemPrefab { get; set; }
    public GameObject HoverPrefab { get; set; }
    public GameObject InitiativePanel { get; set; }
    private GameObject InitPrefab { get; set; }
    private GameObject PendingActionPrefab;

    private GameObject CharacterPrefab { get; set; }

    private GameObject SpritePrefab { get; set; }


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

        List<GameCharacter> playerCharacterList = new List<GameCharacter>() { gameDataObject.playerGameCharacter};
        playerCharacterList.AddRange(gameDataObject.partyList);

        this.battleGameData = BattleFactory.getBattleGameDataFromZoneTree(playerCharacterList, battleTree, gameDataObject.gameDataSet, tileMapData);

        this.battleGame = new BattleGame(battleGameData, r, this);

        LoadPrefabs();

        LoadCharacters();

        LoadUI();

        SetCamera();

        //testing
        displayCollisionSprites();

    }

   
    

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


    private void LoadUI()
    {
        LoadInitiative();
    }

    private void LoadPrefabs()
    {
  
        canvas = GameObject.FindObjectOfType<Canvas>();
             
        ItemPrefab = Resources.Load<GameObject>("PrefabUI/BattleItemPrefab");
        AbilityItemPrefab = Resources.Load<GameObject>("PrefabUI/BattleAbilityPrefab");
        InitPrefab = Resources.Load<GameObject>("PrefabUI/BattleInitiativePanelPrefab");
        HoverPrefab = Resources.Load<GameObject>("PrefabUI/BattleHoverPrefab");
        PendingActionPrefab = Resources.Load<GameObject>("PrefabUI/BattlePendingActionPrefab");


        CharacterPrefab = Resources.Load<GameObject>("PrefabGame/CharacterPrefab");

        InitiativePanel = GameObject.FindGameObjectWithTag("InitiativePanel");

        DebugText = GameObject.FindGameObjectWithTag("DebugText").GetComponent<Text>();

        SpritePrefab = Resources.Load<GameObject>("PrefabGame/SpritePrefab");
    }

    private void LoadCharacters()
    {
        LoadActiveCharacter();

        //clear Characters
        foreach( var c in tileCharacterList)
        {
            Destroy(c);
        }

        tileCharacterList.Clear();
       
        foreach(var character in battleGame.characterList)
        {
            tileCharacterList.Add(LoadCharacter(character));

        }
    }

    private void LoadActiveCharacter()
    {
        if (battleGame.ActiveCharacter != null)
        {
            var ac = battleGame.ActiveCharacter;
            var ActiveCharacterPanel = GameObject.FindGameObjectWithTag("ActiveCharacterPanel");
            UIHelper.UpdateSpriteComponent(ActiveCharacterPanel, "ActiveCharacterPortrait", gameDataObject.assetLibrary.getSprite(ac.portraitSpritesheetName,ac.portraitSpriteIndex));
            UIHelper.UpdateTextComponent(ActiveCharacterPanel, "ActiveCharacterName", ac.name);
            UIHelper.UpdateTextComponent(ActiveCharacterPanel,"ActiveCharacterAPText",string.Format("AP:{0}/{1}",ac.ap,ac.totalAP));

            UIHelper.UpdateSliderValue(ActiveCharacterPanel,"ActiveCharacterAPSlider",ac.ap);

            UIHelper.UpdateSliderValue(ActiveCharacterPanel, "ActiveCharacterHPSlider", (float)ac.hp / (float)ac.totalHP);

        }
    }


    private GameObject LoadCharacter(GameCharacter character)
    {
        GameObject characterObject = (GameObject)Instantiate(CharacterPrefab);
        GameObjectHelper.UpdateSprite(characterObject, "CharacterSprite", gameDataObject.assetLibrary.getSprite(character.characterSpritesheetName, character.characterSpriteIndex));

        if (character.type == CharacterType.Player)
        {
            GameObjectHelper.UpdateSpriteColor(characterObject, "HighlightSprite", Color.green);

            if (battleGame.ActiveCharacter.Equals(character))
            {
                GameObjectHelper.UpdateSpriteColor(characterObject, "HighlightSprite", Color.yellow);
            }
        }
        else
        {
            GameObjectHelper.UpdateSpriteColor(characterObject, "HighlightSprite", Color.red);
        }

        var characterPos = getWorldPosFromTilePoint(new Point(character.x, -character.y));
        characterObject.transform.position = characterPos;

        return characterObject;
    }


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

        List<Point> pathFind;

        if (uiState == UIStateType.PlayerDecide)
        {
            switch (playerDecideState)
            {
                case PlayerDecideState.MovePendingClick:
                    
                    pathFind = PathFind.Pathfind(battleGame.board, battleGame.ActiveCharacter.x, battleGame.ActiveCharacter.y, hoverPoint.x, -hoverPoint.y);
                    if (pathFind.Count > battleGame.ActiveCharacter.ap) {
                        AddHighlightTiles(pathFind, Color.red);
                    }
                    else
                    {
                        AddHighlightTiles(pathFind, Color.green);
                    }
                    
                    break;
                case PlayerDecideState.AttackPendingClick:
                    pathFind = PathFind.Pathfind(battleGame.board, battleGame.ActiveCharacter.x, battleGame.ActiveCharacter.y, hoverPoint.x, -hoverPoint.y);
                    if (pathFind.Count + battleGame.ActiveCharacter.weapon.actionPoints > battleGame.ActiveCharacter.ap) {
                        AddHighlightTiles(pathFind, Color.red);
                    }
                    else
                    {
                        AddHighlightTiles(pathFind, Color.green);
                    }
                    
                    break;
                case PlayerDecideState.AbilityPendingClick:
                    DisplayHighlightTilesForAbility();
                    break;
                default:
                    break;
            }
        }
    }

    private void DisplayHighlightTilesForAbility()
    {
        Point fixedHoverPoint = new Point(hoverPoint.x, -hoverPoint.y);
        Tile origin;
        Tile dest;
        List<Point> pointList = new List<Point>();

        switch (selectedAbility.targetType)
        {
            case AbilityTargetType.AllFoes:
                break;
            case AbilityTargetType.AllFriends:
                break;
            case AbilityTargetType.LOSEmpty:
                origin = battleGame.board.getTileFromLocation(battleGame.ActiveCharacter.x, battleGame.ActiveCharacter.y);
                dest = battleGame.board.getTileFromPoint(fixedHoverPoint);
                pointList = battleGame.board.getBoardLOSPointList(origin, dest);
                if (battleGame.board.checkLOS(origin, dest) && pointList.Count <= selectedAbility.range && selectedAbility.ap <= battleGame.ActiveCharacter.ap)
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
                if (battleGame.board.checkLOS(origin, dest) && pointList.Count <= selectedAbility.range && selectedAbility.ap <= battleGame.ActiveCharacter.ap)
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

    private float getUpdateBattleTimer()
    {
        if(battleGame.ActiveCharacter.type == CharacterType.Enemy)
        {
            return GameConfig.enemyUpdateBattleTimer;
        }
        else
        {
            return GameConfig.playerUpdateBattleTimer; 

        }

    }

    public void LoseBattle()
    {
        //Go back to Start Scene / Reload
        //Or show a game over popup from this screen, not a new scene.
        battleStatus = BattleStatusType.GameOver;
        Application.LoadLevel("GameOverScene");
    }

    void UpdateDebug()
    {
        Vector3 viewPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var canvas = GameObject.FindGameObjectWithTag("FrontCanvas");

        Vector2 canvasPos = Input.mousePosition - canvas.transform.localPosition; 

        string mousePos = string.Format("Mouse: {0},{1} | World: {2},{3} | view {4},{5} | canvas {6},{7}",
            Input.mousePosition.x, Input.mousePosition.y, viewPos.x, viewPos.y, worldPos.x, worldPos.y,canvasPos.x,canvasPos.y);
        DebugText.text = mousePos;

    }

    void UpdateBattle()
    {

        battleGame.board.ClearTempTiles();

        //Update Map
        LoadCharacters();

        UpdateBattleLogText();

        switch(uiState)
        {
            case UIStateType.NewTurn:
                UIHelper.SetAllButtons(false);
                UpdateNewTurn();
                break;
            case UIStateType.PlayerDecide:
                
                UpdatePlayerDecide();
                break;
            case UIStateType.PlayerExecute:
                UIHelper.SetAllButtons(false);
                UpdateBattleActions();
                break;
            case UIStateType.EnemyDecide:
                UIHelper.SetAllButtons(false);
                UpdateEnemyDecide();
                break;
            case UIStateType.EnemyExecute:
                UIHelper.SetAllButtons(false);
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

        //Update UI
        UpdateInitiativePanel();

        //Update Abilities
        LoadAbilityList();

        //update Items
        LoadItemList();

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
        switch(playerDecideState)
        {
            case PlayerDecideState.Waiting:
                UIHelper.SetAllButtons(true);
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
                    battleGame.NextTurn();
                    uiState = UIStateType.NewTurn;
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

    //Create battle actions here, or lower level?
    public void PlayerEndTurn()
    {
        HidePanels();
        if (uiState == UIStateType.PlayerDecide)
        {
            battleGame.battleLog.AddEntry(string.Format("{0} ended turn.", battleGame.ActiveCharacter.name));

            battleGame.NextTurn();
            uiState = UIStateType.NewTurn;
        }
    }

    public void PlayerMoveStart()
    {
        HidePanels();
        if (uiState == UIStateType.PlayerDecide)
        {
            if (playerDecideState == PlayerDecideState.Waiting)
            {
                UIHelper.SetAllButtons(false);
                UIHelper.SetButton("MoveButton", true);

                DebugText.text = string.Format("Select Destination");
                clickPoint = null;
                playerDecideState = PlayerDecideState.MovePendingClick;

                DisplayPendingActionHover();
            }
            else
            {
                UIHelper.SetAllButtons(true);
                playerDecideState = PlayerDecideState.Waiting;
            }
        }
    }

    public void PlayerMoveSelect()
    {
        if(clickPoint != null)
        {
            playerDecideState = PlayerDecideState.Waiting;
            battleGame.actionQueue.AddRange(battleGame.GetMoveActionList(clickPoint.x, clickPoint.y));
            uiState = UIStateType.PlayerExecute;

            RemovePendingAbilityHover();
        }
    }

    //Determine if enemy was clicked on to attack, otherwise move to spot
    public void PlayerSmartAttackOrMove()
    {
        if (clickPoint != null)
        {
            Tile clickTile = battleGame.board.getTileFromPoint(clickPoint);
            GameCharacter enemyChar =  battleGame.getCharacterFromTile(clickTile);
            if (enemyChar != null)
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

    public void PlayerAttackStart()
    {

        if (battleGame.ActiveCharacter.weapon != null)
        {
            if (battleGame.ActiveCharacter.weapon.weaponType == WeaponType.OneHandRanged || battleGame.ActiveCharacter.weapon.weaponType == WeaponType.TwoHandRanged)
            {
                PlayerRangedAttackStart();
            }
            else
            {
                PlayerMeleeAttackStart();
            }

        }
    }

    public void PlayerMeleeAttackStart()
    {
        HidePanels();

        if (uiState == UIStateType.PlayerDecide)
        {
            if (playerDecideState == PlayerDecideState.Waiting)
            {
                UIHelper.SetAllButtons(false);
                UIHelper.SetButton("AttackButton", true);

                DebugText.text = string.Format("Select Target");
                clickPoint = null;
                playerDecideState = PlayerDecideState.AttackPendingClick;

                DisplayPendingActionHover();

            }
            else
            {
                UIHelper.SetAllButtons(true);
                playerDecideState = PlayerDecideState.Waiting;
            }
           
        }
    }

    public void PlayerAttackSelect()
    {
        if(clickPoint != null)
        {
            playerDecideState = PlayerDecideState.Waiting;
            battleGame.actionQueue.AddRange(battleGame.getAttackActionList(clickPoint.x, clickPoint.y));
            uiState = UIStateType.PlayerExecute;
            RemovePendingAbilityHover();
        }
    }

    public void PlayerRangedAttackStart()
    {
        HidePanels();

        if (uiState == UIStateType.PlayerDecide)
        {
            if (playerDecideState == PlayerDecideState.Waiting)
            {
                UIHelper.SetAllButtons(false);
                UIHelper.SetButton("AttackButton", true);

                DebugText.text = string.Format("Select Target");
                clickPoint = null;
                playerDecideState = PlayerDecideState.RangedAttackPendingClick;
                DisplayPendingActionHover();
            }

            else
            {
                UIHelper.SetAllButtons(true);
                playerDecideState = PlayerDecideState.Waiting;
            }
        }
    }
    public void PlayerRangedAttackSelect()
    {
        if (clickPoint != null)
        {
            playerDecideState = PlayerDecideState.Waiting;
            battleGame.actionQueue.AddRange(battleGame.getRangedAttackActionList(clickPoint.x, clickPoint.y));
            uiState = UIStateType.PlayerExecute;
            RemovePendingAbilityHover();
        }
    }

    public void PlayerItemStart(UsableItem selectedItem)
    {
        if (uiState == UIStateType.PlayerDecide)
        {
                clickPoint = null;
                this.selectedItem = selectedItem;
                playerDecideState = PlayerDecideState.ItemPendingClick;

                DisplayPendingActionHover();
        }
    }

    public void PlayerItemSelect()
    {
        if (clickPoint != null)
        {
            playerDecideState = PlayerDecideState.Waiting;
            battleGame.actionQueue.AddRange(battleGame.getItemActionList(selectedItem, clickPoint.x, clickPoint.y));
            uiState = UIStateType.PlayerExecute;
            HidePanels();
            LoadItemList();

            RemovePendingAbilityHover();
        }
    }


    public void PlayerAbilityStart(Ability selectedAbility)
    {
        if (uiState == UIStateType.PlayerDecide)
        {
            clickPoint = null;
            this.selectedAbility = selectedAbility;
            playerDecideState = PlayerDecideState.AbilityPendingClick;
            DisplayPendingActionHover();
        }
    }

    public void PlayerAbilitySelect()
    { 
        if(clickPoint != null)
        {
            playerDecideState = PlayerDecideState.Waiting;
            battleGame.actionQueue.AddRange(battleGame.getAbilityActionList(selectedAbility, clickPoint.x, clickPoint.y));
            uiState = UIStateType.PlayerExecute;
            HidePanels();
            LoadAbilityList();

            RemovePendingAbilityHover();
        }
    }

    public void DisplayPendingActionHover()
    {
        Sprite actionSprite;
        switch (playerDecideState)
        {
            case PlayerDecideState.AbilityPendingClick:
                PendingActionHover = Instantiate(PendingActionPrefab);
                 actionSprite = gameDataObject.assetLibrary.getSprite(selectedAbility.sheetname,selectedAbility.spriteindex);
                UpdatePendingActionHover(PendingActionHover, actionSprite, selectedAbility.name);
                break;
            case PlayerDecideState.AttackPendingClick:
                 PendingActionHover = Instantiate(PendingActionPrefab);
                 actionSprite = gameDataObject.assetLibrary.getSprite("Blank",0);
                UpdatePendingActionHover(PendingActionHover, actionSprite, "Attack");
                break;
            case PlayerDecideState.ItemPendingClick:
                 PendingActionHover = Instantiate(PendingActionPrefab);
                 actionSprite = gameDataObject.assetLibrary.getSprite(selectedItem.sheetname, selectedItem.spriteindex);
                UpdatePendingActionHover(PendingActionHover, actionSprite, selectedItem.name);
                break;
            case PlayerDecideState.MovePendingClick:
                 PendingActionHover = Instantiate(PendingActionPrefab);
                 actionSprite = gameDataObject.assetLibrary.getSprite("Blank",0);
                UpdatePendingActionHover(PendingActionHover, actionSprite, "Move");
                break;
            case PlayerDecideState.RangedAttackPendingClick:
                 PendingActionHover = Instantiate(PendingActionPrefab);
                 actionSprite = gameDataObject.assetLibrary.getSprite("Blank",0);
                UpdatePendingActionHover(PendingActionHover, actionSprite, "Ranged Attack");
                break;
            default:
                break;
        }
    }

    private void UpdatePendingActionHover(GameObject hoverObject, Sprite image, string text)
    {
        UIHelper.UpdateSpriteComponent(hoverObject, "Image", image);
        UIHelper.UpdateTextComponent(hoverObject, "PendingActionText", text);
        Button b = hoverObject.GetComponentInChildren<Button>();
        b.onClick.AddListener(CancelPendingAction);
        hoverObject.transform.SetParent(canvas.transform, true);
        hoverObject.transform.localPosition = new Vector3(0, 320, 0);
    }

    public void CancelPendingAction()
    {
        uiState = UIStateType.PlayerDecide;
        playerDecideState = PlayerDecideState.Waiting;
        RemovePendingAbilityHover();
    }

    public void RemovePendingAbilityHover()
    {
        Destroy(PendingActionHover);
    }

    private void ClickTile()
    {

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        DebugText.text = mouseWorldPosition.ToString();

        if (Input.GetMouseButtonDown(0)) //left click
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            //if (!checkPointOnUI(Input.mousePosition))
            {
                var mouseTilePt = getTileLocationFromVectorPos(mouseWorldPosition);
                if (mouseTilePt != null)
                {
                    this.clickPoint = new Point(mouseTilePt.x, -mouseTilePt.y);

                }
                if (clickPoint != null)
                {
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

   

    //return true if point on top of UI
    private bool checkPointOnUI(Vector3 screenPos)
    {
        Rect uiScreenRect = new Rect(0, 0, 1280, 210);
        if(uiScreenRect.Contains(screenPos))
        {
            return true;
        }
        return false;
    }

    private Bounds getTileBounds(int x, int y)
    {
        Vector3 center = new UnityEngine.Vector3(x, y, 0);
        Vector3 size = new UnityEngine.Vector3(Tile.TILE_SIZE, Tile.TILE_SIZE);
        Bounds b = new UnityEngine.Bounds(center, size);
        return b;
    }

    private Point getTileLocationFromVectorPos(Vector3 pos)
    {
        int x = Mathf.RoundToInt(pos.x-.5f );
        int y = Mathf.RoundToInt(pos.y+.5f );

        Point retval = null;

        if (x >= 0 && x <= tileMapData.battleTileArray.GetLength(0) && y <= 0 && y >= -tileMapData.battleTileArray.GetLength(1))
        {
            retval = new Point() { x = (int)x, y = (int)y };
        }
        return retval;
    }

    private Vector3 getWorldPosFromTilePoint(Point p)
    {
        return new Vector3(p.x+0.5f, p.y-0.5f, 0);
    }

    private Vector3 getWorldPosOffset(Vector3 pos)
    {
        return new Vector3(pos.x+0.5f, pos.y-0.5f, pos.z);
    }

    private Point getBoardPointFromLocation(float x, float y)
    {
        x = x + 0.5f;
        y = y + 0.5f;

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

    private void SelectTile()
    {
        Destroy(SelectedTile);
        SelectedTile = new GameObject("SelectedTile");
        SpriteRenderer renderer = SelectedTile.AddComponent<SpriteRenderer>();
        renderer.sortingOrder = 1;

        SelectedTile.transform.position = getWorldPosFromTilePoint(new Point(clickPoint.x, -clickPoint.y));
       
        renderer.sprite = Resources.Load<Sprite>("Sprites/highlightTile 1");
    }

    private void DeselectTile()
    {
        Destroy(SelectedTile);
        SelectedTile = null;
    }


    public void MoveCameraToCharacter(System.Object characterObject)
    {
        GameCharacter gc = (GameCharacter)characterObject;
        FocusCamera(gc.x, -gc.y);
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
        var cameraPos = getWorldPosFromTilePoint(new Point(x,y));
        cameraData.SetCamera(cameraPos.x,cameraPos.y);
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


    #region UI

    public void HidePanels()
    {
        HideItemPanel();
        HideAbilityPanel();
    }

    public void ShowAbilityPanel()
    {
        if(playerDecideState == PlayerDecideState.Waiting)
        {
            UIHelper.SetAllButtons(false);
            UIHelper.SetButton("AbilitiesButton", true);

            HidePanels();
            var abilityPanel = GameObject.FindGameObjectWithTag("AbilitiesPanel");
            UIHelper.MoveUIObject(abilityPanel, GameConfig.AbilityPanelDisplayLocation);

            playerDecideState = PlayerDecideState.AbilityPanelShow;
        }
        else
        {
            HideAbilityPanel();
            playerDecideState = PlayerDecideState.Waiting;
            UIHelper.SetAllButtons(true);
        }

    }

    public void HideAbilityPanel()
    {
        var abilityPanel = GameObject.FindGameObjectWithTag("AbilitiesPanel");
        UIHelper.MoveUIObject(abilityPanel, GameConfig.AbilityPanelHideLocation);
     
    }

    public void ShowItemPanel()
    {
        LoadItemList();
        if (uiState == UIStateType.PlayerDecide)
        {
            if (playerDecideState == PlayerDecideState.Waiting)
            {
                UIHelper.SetAllButtons(false);
                UIHelper.SetButton("ItemButton", true);

                HidePanels();
                var itemPanel = GameObject.FindGameObjectWithTag("ItemPanel");
                UIHelper.MoveUIObject(itemPanel, GameConfig.ItemPanelDisplayLocation);

                playerDecideState = PlayerDecideState.AbilityPanelShow;
            }
            else
            {
                HideItemPanel();
                playerDecideState = PlayerDecideState.Waiting;
                UIHelper.SetAllButtons(true);
            }
        }
    }

    public void HideItemPanel()
    {
        var itemPanel = GameObject.FindGameObjectWithTag("ItemPanel");
        UIHelper.MoveUIObject(itemPanel, GameConfig.ItemPanelHideLocation);
    }

    public void ShowEquipPanel()
    {
        
        if (uiState == UIStateType.PlayerDecide)
        {
           
            var equipPanel = GameObject.FindGameObjectWithTag("EquipPanel");
            
            var equipControllerObject = equipPanel.GetComponent<EquipmentControllerScript>();
            equipControllerObject.RefreshEquipment();

            UIHelper.MoveUIObject(equipPanel, GameConfig.EquipPanelDisplayLocation);
        }
    }

    public void HideEquipPanel()
    {
        var equipPanel = GameObject.FindGameObjectWithTag("EquipPanel");
        UIHelper.MoveUIObject(equipPanel, GameConfig.EquipPanelHideLocation);
    }

   

    public void UpdateInitiativePanel()
    {
        //clear current Panel
        for (int i = InitiativePanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(InitiativePanel.transform.GetChild(i).gameObject);
        }

        LoadInitiative();
    }


    private void UpdateBattleLogText()
    {
        var battleLogContent = GameObject.FindGameObjectWithTag("BattleLogContent");
        UIHelper.UpdateTextComponent(battleLogContent, "BattleLogText", battleGame.battleLog.ToString());

        var battleLogScrollRect = GameObject.FindGameObjectWithTag("BattleLogScrollRect").GetComponent<ScrollRect>();
        battleLogScrollRect.verticalNormalizedPosition = 0;

    }

    private void LoadInitiative()
    {
      
        if (battleGame != null)
        {
            foreach (var character in battleGame.characterList)
            {
                GameObject initGameObject = (GameObject)Instantiate(InitPrefab);
                initGameObject = updateInitPortrait(initGameObject, character);

                initGameObject.transform.SetParent(InitiativePanel.transform, true);
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

            panelImg.overrideSprite = gameDataObject.assetLibrary.getSprite("InitBG2", 0);
        }


        UIHelper.UpdateSpriteComponent(charPortrait, "PortraitImage", gameDataObject.assetLibrary.getSprite(character.portraitSpritesheetName, character.portraitSpriteIndex));

        UIHelper.UpdateTextComponent(charPortrait, "CharacterName", character.name.ToString());

        string stats = string.Format("HP: {0}/{1} AP: {2}", character.hp, character.totalHP, character.ap);
 

        UIHelper.AddClickToGameObject(charPortrait, UpdateInitiativeHover, EventTriggerType.PointerEnter, character);
        UIHelper.AddClickToGameObject(charPortrait, ClearCharacterHover, EventTriggerType.PointerExit);
        UIHelper.AddClickToGameObject(charPortrait, MoveCameraToCharacter, EventTriggerType.PointerClick, character);

        return charPortrait;
    }

   
    private void LoadAbilityList()
    {
        var usableAbilityList = (from data in battleGame.ActiveCharacter.abilityList
                                 where data.uses > 0
                                 select data).ToList();

        Transform AbilityPanel = GameObject.FindGameObjectWithTag("AbilityContentPanel").transform;

        //Clear existing abilities
        UIHelper.DestroyAllChildren(AbilityPanel);
       
        foreach (var ab in usableAbilityList)
        {
            GameObject abilityItem = (GameObject)Instantiate(AbilityItemPrefab);
            updateAbilityButton(abilityItem, gameDataObject.assetLibrary.getSprite(ab.sheetname, ab.spriteindex), ab);
            abilityItem = updateAbilityItem(abilityItem, ab);
            abilityItem.transform.SetParent(AbilityPanel, true);
        }
    }

    private void LoadItemList()
    {
           
        Transform ItemPanel = GameObject.FindGameObjectWithTag("ItemContentPanel").transform;

        //Clear existing abilities
        UIHelper.DestroyAllChildren(ItemPanel);

        var usableItemList = battleGame.ActiveCharacter.usableItemList;
        foreach (var item in usableItemList)
        {
            var usableItem = (UsableItem)item;

            GameObject itemObject = (GameObject)Instantiate(ItemPrefab);
            UIHelper.UpdateTextComponent(itemObject, "ItemText", item.ToString());
            UIHelper.UpdateSpriteComponent(itemObject, "ItemButtonImage", gameDataObject.assetLibrary.getSprite(item.sheetname, item.spriteindex));

            Button buttonClick = itemObject.GetComponentInChildren<Button>();
            buttonClick.onClick.AddListener(() => PlayerItemStart(usableItem));

            itemObject.transform.SetParent(ItemPanel, true);

        }
    }

    private void updateAbilityButton(GameObject parent, Sprite sprite, Ability selectedAbility)
    {
        UIHelper.UpdateSpriteComponent(parent, "AbilityButtonImage", sprite);

        Button buttonClick = parent.GetComponentInChildren<Button>();
        buttonClick.onClick.AddListener(() => PlayerAbilityStart(selectedAbility));
    }

    private GameObject updateAbilityItem(GameObject abilityItem, Ability a)
    {
        string abilityText = string.Format("{0} - {1}. AP: {2} Uses: {3}",a.name,a.description,a.ap,a.uses);
       
        //set click on icon
        UIHelper.UpdateTextComponent(abilityItem, "AbilityText", abilityText);

        return abilityItem;
    }


    #endregion

    private void GetCharacterHover()
    {
        if (CharacterHover == null)
        {
            CharacterHover = GameObject.FindGameObjectWithTag("CharacterHover");
        }
    }

    public void UpdateInitiativeHover(System.Object characterObject)
    {

        GameCharacter character = (GameCharacter)characterObject;

        GetCharacterHover();

        if (character != null)
        {
            UIHelper.UpdateSliderValue(CharacterHover, "HPSlider", (float)character.hp / (float)character.totalHP);
            UIHelper.UpdateTextComponent(CharacterHover, "CharacterName", character.name);
            UIHelper.UpdateTextComponent(CharacterHover, "CharacterStats", character.ToString());
        }
    }

    public void UpdateCharacterHover()
    {
        GetCharacterHover();

        //Determine character being highlighted
        Vector3 boardPos = getBoardPositionFromScreenPosition(Input.mousePosition);
        Point boardPoint = getBoardPointFromLocation(boardPos.x, -boardPos.y);
        if (boardPoint != null)
        {

            GameCharacter hoverCharacter = battleGame.getCharacterFromTile(battleGame.board.getTileFromLocation(boardPoint.x, boardPoint.y));

            if (hoverCharacter != null)
            {
                UIHelper.UpdateSliderValue(CharacterHover, "HPSlider", (float)hoverCharacter.hp / (float)hoverCharacter.totalHP);
                UIHelper.UpdateTextComponent(CharacterHover, "CharacterName", hoverCharacter.name);
                UIHelper.UpdateTextComponent(CharacterHover, "CharacterStats", hoverCharacter.ToString());
            }
        }
    }

    public void ClearCharacterHover()
    {
        GetCharacterHover();
        UIHelper.UpdateSliderValue(CharacterHover, "HPSlider",0);
        UIHelper.UpdateTextComponent(CharacterHover, "CharacterName", "");
        UIHelper.UpdateTextComponent(CharacterHover, "CharacterStats", "");
    }

    public void DisplayDebugText()
    {
        var debugText = GameObject.FindGameObjectWithTag("DebugText");
        var text = debugText.GetComponentInChildren<Text>();

        text.text = "Hovering " + DateTime.Now.ToString();

    }

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
        if(isDamage)
        {
            c = Color.red;
        }

        Vector3 charPos = new Vector3(gameCharacter.x, -gameCharacter.y+0.5f, 0);
        battleGame.battleLog.AddEntry(charPos.ToString());

        StartTempText(charPos, c, amount.ToString());
    }

    public void StartTempSpriteOnChar(GameCharacter gameCharacter, string spritesheet, int spriteindex)
    {
         Vector3 charPos = new Vector3(gameCharacter.x,-gameCharacter.y,0);
         StartTempSprite(charPos, spritesheet, spriteindex);
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

        
        Vector3 endPos = new Vector3(pos.x,pos.y+.2f,pos.z);
       
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

    public void StartTempSprite(Vector3 pos, string spritesheetName, int spriteIndex)
    {
        pos = getWorldPosOffset(pos);

        var sprite = gameDataObject.assetLibrary.getSprite(spritesheetName, spriteIndex);
        var spriteObj = gameDataObject.assetLibrary.getPrefabGameObject("Sprite");
        spriteObj.transform.position = pos;
        var spriteRenderer = spriteObj.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 3;
        spriteRenderer.sprite = sprite;
        //GameObjectHelper.UpdateSprite(spriteObj, "Sprite", sprite);

        AddEffect(TempEffectType.Sprite, 1, pos, pos, spriteObj);
    }

    #endregion


    public void WinBattle()
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

    public void TestButton()
    {
        StartTempSpriteOnChar(battleGame.ActiveCharacter, "Tile64", 0);
        //StartTempSprite(new Vector3(0, 0, 0), "HighlightTile", 0);
        
        TestParticle();
        //spawn text at the player loc
        //var textPrefab = Resources.Load("PrefabGame/TextPopupPrefab");
        //StartTempTextOnChar(battleGame.ActiveCharacter, 5000, true);

        StartTempParticleOnChar(battleGame.ActiveCharacter, "Fire_01");

        //StartTempSpriteOnChar(battleGame.ActiveCharacter, "DamageEffects", 2);

        //StartTempSprite(new Vector3(4, 0, 0), "DamageEffects", 2);
        //StartTempText(new Vector3(4,-8, 0), Color.green, "HELLO WORLD!");
        //StartTempText(new Vector3(4, -9, 0), Color.blue, "HELLO WORLD BLUE!");
        
    }

    public void TestParticle()
    {
        StartTempParticles("Fire_01", new Vector3(0, 0, 0));
    }


}

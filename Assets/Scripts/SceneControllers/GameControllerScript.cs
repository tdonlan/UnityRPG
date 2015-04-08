using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

using UnityRPG;

using UnityEngine.EventSystems;

public class GameControllerScript : MonoBehaviour
{
    public int battleIndex { get; set; }

    public GameData gameData { get; set; }

    public BattleGame battleGame { get; set; }
    public BattleStatusType battleStatus { get; set; }

    public List<GameObject> tileCharacterList { get; set; }
    public List<TempEffect> tempEffectList { get; set; }

    Text DebugText = null;

    public System.Random r { get; set; }

    public Point clickPoint { get; set; }
    public List<GameObject> highlightTiles { get; set; }
    GameObject SelectedTile { get; set; }

    public GameObject HoverStatsObject { get; set; }
    public GameObject CharacterHover { get; set; }
    public bool isStatsDisplay { get; set; }

    private float UITimer { get; set; }
    private float TempEffectTimer { get; set; }

    private Ability selectedAbility { get; set; }
    private UsableItem selectedItem { get; set; }
    public UIStateType uiState { get; set; }

    public PlayerDecideState playerDecideState { get; set; }

    //Camera Pan / Zoom
    BattleSceneCameraData cameraData { get; set; }

    //UI Prefabs
    public GameObject ItemPrefab { get; set; }
    public GameObject AbilityItemPrefab { get; set; }
    public GameObject HoverPrefab { get; set; }
    public GameObject InitiativePanel { get; set; }
    private GameObject InitPrefab { get; set; }

    private GameObject CharacterPrefab { get; set; }
    private GameObject CharacterPrefab2 { get; set; }

    void Awake()
    {
        
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
           
            tileCharacterList = new List<GameObject>();
            tempEffectList = new List<TempEffect>();

            this.r = new System.Random();

            this.clickPoint = null;

            this.uiState = UIStateType.NewTurn;

            getBattleIndex();
           
            this.gameData = BattleFactory.getGameData(this.battleIndex, this.r);

            this.battleGame = new BattleGame(gameData, r,this);

            LoadPrefabs();

            LoadBoard();
            LoadCharacters();

            LoadUI();

            SetCamera();

            DontDestroyOnLoad(this);
        }
    }

    // Use this for initialization
    void Start()
    {

    
    }

    private void getBattleIndex()
    {
        var startScript = GameObject.FindObjectOfType<StartGameScript>();
        if (startScript == null)
        {
            this.battleIndex = 1;
        }
        else
        {
            this.battleIndex = startScript.battleIndex;
        }

        Destroy(startScript);
    }

    private void LoadUI()
    {
     
        LoadInitiative();

    }

    private void LoadPrefabs()
    {
         ItemPrefab = Resources.Load<GameObject>("Prefab/ItemPrefab");
        AbilityItemPrefab = Resources.Load<GameObject>("Prefab/AbilityPrefab");
        InitPrefab = Resources.Load<GameObject>("Prefab/BattleInitiativePanelPrefab");
        HoverPrefab = Resources.Load<GameObject>("Prefab/HoverPrefab");

        CharacterPrefab = Resources.Load<GameObject>("Prefab/CharacterPrefab");
        CharacterPrefab2 = Resources.Load<GameObject>("Prefab/CharacterPrefab2");

        InitiativePanel = GameObject.FindGameObjectWithTag("InitiativePanel");

        DebugText = GameObject.FindGameObjectWithTag("DebugText").GetComponent<Text>();

    }


    private void LoadBoard()
    {
        for (int i = 0; i < this.battleGame.board.board.GetLength(0); i++)
        {
            for (int j = 0; j < this.battleGame.board.board.GetLength(1); j++)
            {

                LoadTile(battleGame.board.board[i, j]);

            }
        }
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
            UIHelper.UpdateSpriteComponent(ActiveCharacterPanel, "ActiveCharacterPortrait", gameData.assetLibrary.getSprite(ac.portraitSpritesheetName,ac.portraitSpriteIndex));
            UIHelper.UpdateTextComponent(ActiveCharacterPanel, "ActiveCharacterName", ac.name);
            UIHelper.UpdateTextComponent(ActiveCharacterPanel,"ActiveCharacterAPText",string.Format("AP:{0}/{1}",ac.ap,ac.totalAP));

            //hardcoded to 10
            UIHelper.UpdateSliderValue(ActiveCharacterPanel,"ActiveCharacterAPSlider",ac.ap);

            UIHelper.UpdateSliderValue(ActiveCharacterPanel, "ActiveCharacterHPSlider", (float)ac.hp / (float)ac.totalHP);

        }
    }


    private GameObject LoadCharacter(GameCharacter character)
    {
        GameObject characterObject = (GameObject)Instantiate(CharacterPrefab2);
        GameObjectHelper.UpdateSprite(characterObject, "CharacterSprite", gameData.assetLibrary.getSprite(character.characterSpritesheetName, character.characterSpriteIndex));

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


        characterObject.transform.position = new Vector3(character.x, character.y, -1);

        return characterObject;
    }

    //Deprecated
    /*
    private void LoadTempEffects()
    {
        foreach (var c in tempEffectList)
        {
            //Destroy(c);
        }

        tempEffectList.Clear();

        for(int i =0;i<battleGame.board.board.GetLength(0);i++)
        {
            for(int j=0;j<battleGame.board.board.GetLength(1);j++)
            {
                var tile = battleGame.board.board[i, j];

                if(tile.tempSheetName != string.Empty)
                {
                     //tempEffectList.Add(LoadTempEffect(tile));
                }
               
            }
        }
    }

    private GameObject LoadTempEffect(Tile t)
    {
        GameObject effectSprite = new GameObject("Effect");

        SpriteRenderer renderer = effectSprite.AddComponent<SpriteRenderer>();
        effectSprite.transform.position = new Vector3(t.x, t.y, -2);

        renderer.sprite = gameData.assetLibrary.getSprite(t.tempSheetName, t.tempSpriteIndex);


        return effectSprite;
    }
     * */

    private void LoadTile(Tile t)
    {
        GameObject spriteObject = new GameObject("Tile " + name);
        SpriteRenderer renderer = spriteObject.AddComponent<SpriteRenderer>();
        spriteObject.transform.position = new Vector3(t.x, t.y, 0);

        renderer.sprite = gameData.assetLibrary.getSprite(t.tileSheetName, t.tileSpriteIndex);

    }


    //Init the camera to middle of game board
    private void SetCamera()
    {
        int x = (int)Mathf.Round(battleGame.board.board.GetLength(0) / 2f);
        int y = (int)Mathf.Round(battleGame.board.board.GetLength(1) / 2f);

        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.transform.position = new Vector3(x, y, -10);

        var cam = mainCamera.GetComponent<Camera>();

        cameraData = new BattleSceneCameraData(x, y, cam.orthographicSize, GameConfig.PanLerp, GameConfig.ZoomLerp);

    }


    // Update is called once per frame
    void Update()
    {

        float delta = Time.deltaTime;

        if (battleStatus != BattleStatusType.GameOver)
        {
            battleStatus = battleGame.getBattleStatus();
        }

        if (battleStatus == BattleStatusType.Running)
        {

            //UpdateDebug();

            UpdateTempEffects(delta);

            UITimer -= Time.deltaTime;
            if (UITimer <= 0)
            {
                UpdateBattle();
                UITimer = getUpdateBattleTimer();
                //LoadTempEffects();
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
        battleStatus = BattleStatusType.GameOver;
        Application.LoadLevel("GameOverScene");
    }

    private void WinBattle()
    {
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
        //DebugText.text = string.Format("UIState: {0} BattleState {1}", uiState, battleGame.getBattleStatus());
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
        FocusCamera(battleGame.ActiveCharacter.x, battleGame.ActiveCharacter.y);

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
        FocusCamera(battleGame.ActiveCharacter.x, battleGame.ActiveCharacter.y);

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
        }
    }

    public void PlayerItemStart(UsableItem selectedItem)
    {
        if (uiState == UIStateType.PlayerDecide)
        {
                clickPoint = null;
                this.selectedItem = selectedItem;
                playerDecideState = PlayerDecideState.ItemPendingClick;
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
        }
    }


    public void PlayerAbilityStart(Ability selectedAbility)
    {
        if (uiState == UIStateType.PlayerDecide)
        {
            clickPoint = null;
            this.selectedAbility = selectedAbility;
            playerDecideState = PlayerDecideState.AbilityPendingClick;
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
        }
    }

  

    private void ClickTile()
    {
        
        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        var camera = mainCamera.GetComponent<Camera>();

        Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)) //left click
        {
            if (!checkPointOnUI(Input.mousePosition))
            {
                this.clickPoint = getBoardPointFromLocation(mouseWorldPosition.x, mouseWorldPosition.y);

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
        SelectedTile.transform.position = new Vector3(clickPoint.x, clickPoint.y, 0);
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
        FocusCamera(gc.x, gc.y);
    }

    private void UpdateCamera()
    {
        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        float vert = Input.GetAxis("Vertical") * GameConfig.PanSpeed;
        float hor = Input.GetAxis("Horizontal") * GameConfig.PanSpeed;

        var destination = new Vector2(cameraData.CameraX + hor, cameraData.CameraY + vert);
        var fixedDestination = MoveCamera(mainCamera, mainCamera.transform.position, destination);

        cameraData.SetCamera(fixedDestination.x, fixedDestination.y);

        mainCamera.transform.position = cameraData.UpdateCamera(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);

        UpdateCameraScroll(mainCamera);
      

    }

    private void UpdateCameraScroll(GameObject mainCameraObject)
    {
        var camera = mainCameraObject.GetComponent<Camera>();

        var newZoom = Input.GetAxis("Mouse ScrollWheel");

        cameraData.SetZoom(Mathf.Clamp(cameraData.Zoom + newZoom * GameConfig.ZoomFactor, GameConfig.MinZoom, GameConfig.MaxZoom));

        camera.orthographicSize = cameraData.UpdateZoom(camera.orthographicSize);

    }

    private void FocusCamera(int x, int y)
    {
        cameraData.SetCamera(x, y);
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

            var equipControllerObject = GameObject.FindGameObjectWithTag("EquipmentController");
            var equipScript = equipControllerObject.GetComponent<EquipmentTestScript>();
            equipScript.RefreshEquipment();


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

            panelImg.overrideSprite = gameData.assetLibrary.getSprite("InitBG2", 0);
        }


        UIHelper.UpdateSpriteComponent(charPortrait, "PortraitImage", gameData.assetLibrary.getSprite(character.portraitSpritesheetName, character.portraitSpriteIndex));

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
            updateAbilityButton(abilityItem, gameData.assetLibrary.getSprite(ab.sheetname, ab.spriteindex), ab);
            abilityItem = updateAbilityItem(abilityItem, ab);
            abilityItem.transform.SetParent(AbilityPanel, true);
        }
    }

    private void LoadItemList()
    {
        
        List<ItemSet> itemSetList = ItemHelper.getItemSetList(battleGame.ActiveCharacter.inventory);

        var usableItemList = (from data in itemSetList
                         where data.count > 0
                         select data).ToList();
                        
 

        Transform ItemPanel = GameObject.FindGameObjectWithTag("ItemContentPanel").transform;

        //Clear existing abilities
        UIHelper.DestroyAllChildren(ItemPanel);

        foreach (var item in usableItemList)
        {

            var usableItem = (UsableItem)ItemHelper.getFirstItemWithID(battleGame.ActiveCharacter.inventory, item.itemID);

            GameObject itemObject = (GameObject)Instantiate(ItemPrefab);
            UIHelper.UpdateTextComponent(itemObject, "ItemText", item.ToString());
            UIHelper.UpdateSpriteComponent(itemObject, "ItemButtonImage", gameData.assetLibrary.getSprite(item.sheetname, item.spriteindex));

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
        Point boardPoint = getBoardPointFromLocation(boardPos.x, boardPos.y);
        GameCharacter hoverCharacter = battleGame.getCharacterFromTile(battleGame.board.getTileFromLocation(boardPoint.x, boardPoint.y));

        if (hoverCharacter != null)
        {
            UIHelper.UpdateSliderValue(CharacterHover, "HPSlider", (float)hoverCharacter.hp / (float)hoverCharacter.totalHP);
            UIHelper.UpdateTextComponent(CharacterHover, "CharacterName", hoverCharacter.name);
            UIHelper.UpdateTextComponent(CharacterHover, "CharacterStats", hoverCharacter.ToString());
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
        
        var particleObj = gameData.assetLibrary.getPrefabGameObject(particleName);

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

        Vector3 charPos = new Vector3(gameCharacter.x,gameCharacter.y,-2);

        StartTempText(charPos, c, amount.ToString());
    }

    public void StartTempText(Vector3 pos, Color c, string text)
    {
        var textObj = gameData.assetLibrary.getPrefabGameObject("TextPopup");
        UpdateTextPopup(textObj, text, c);



        Vector3 endPos = new Vector3(pos.x,pos.y+.2f,pos.z);

       
        AddEffect(TempEffectType.Text, 10, pos, endPos, textObj);
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
        var sprite = gameData.assetLibrary.getSprite(spritesheetName, spriteIndex);
        var spriteObj = gameData.assetLibrary.getPrefabGameObject("Sprite");
        GameObjectHelper.UpdateSprite(spriteObj, "Sprite", sprite);

        AddEffect(TempEffectType.Sprite, 1, pos, pos, spriteObj);
    }

    #endregion




}

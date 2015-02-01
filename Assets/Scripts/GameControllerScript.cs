using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

using Assets;

using SimpleRPG2;

public class GameControllerScript : MonoBehaviour
{

    public AssetLibrary assetLibrary { get; set; } 
    public BattleGame battleGame { get; set; }

    public List<GameObject> tileCharacterList { get; set; }
    public List<GameObject> tempEffectList { get; set; }

    Text DebugText =null;

    public int TileSize = 32;
    //public Board board { get; set; }
    public System.Random r { get; set; }

    public Point clickPoint { get; set; }
    GameObject SelectedTile { get; set; }

    private float UITimer { get; set; }
    private float TempEffectTimer { get; set; }


    private Ability selectedAbility { get; set; }
    private Item selectedItem { get; set; }
    public UIStateType uiState { get; set; }

    public PlayerDecideState playerDecideState { get; set; }

    //UI Prefabs
    public GameObject InitiativePanel { get; set; }
    private GameObject InitPrefab { get; set; }

    // Use this for initialization
    void Start()
    {
        this.assetLibrary = new AssetLibrary();
        this.battleGame = new BattleGame();

        tileCharacterList = new List<GameObject>();
        tempEffectList = new List<GameObject>();

        this.r = new System.Random();
   
        this.clickPoint = null;
        
        this.uiState = UIStateType.NewTurn;

        

        LoadBoard();
        LoadCharacters();
       
        LoadUI();

        SetCamera();
     

    }

    private void LoadUI()
    {
        LoadPrefabs();
        LoadInitiative();
    }

    private void LoadPrefabs()
    {
        InitPrefab = Resources.Load<GameObject>("BattleInitiativePanelPrefab");
        InitiativePanel = GameObject.FindGameObjectWithTag("InitiativePanel");

        DebugText = GameObject.FindGameObjectWithTag("DebugText").GetComponent<Text>();

    }

    private void LoadBoard()
    {
        for (int i = 0; i < this.battleGame.board.board.GetLength(0); i++)
        {
            for (int j = 0; j < this.battleGame.board.board.GetLength(1); j++)
            {
                 if (battleGame.board.board[i,j].TileChar == '#')
                {
                    LoadTile(true, i, j);
                }
                else 
                {
                    LoadTile(false, i, j);
                }
            }
        }
    }

    private void LoadCharacters()
    {

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

    private GameObject LoadCharacter(GameCharacter character)
    {
        int playerIndex = 5;
        int monsterIndex = 83;

        GameObject characterSprite = new GameObject("Character");

        SpriteRenderer renderer = characterSprite.AddComponent<SpriteRenderer>();
        characterSprite.transform.position = new Vector3(character.x, character.y, -1);

        if (character.type == CharacterType.Player)
        {
            renderer.sprite = assetLibrary.getSprite(SpritesheetType.Characters, playerIndex);
        }
        else
        {
            renderer.sprite = assetLibrary.getSprite(SpritesheetType.Characters, monsterIndex);
         
        }

        return characterSprite;

    }

    private void LoadTempEffects()
    {
        foreach (var c in tempEffectList)
        {
            Destroy(c);
        }

        tempEffectList.Clear();

        for(int i =0;i<battleGame.board.board.GetLength(0);i++)
        {
            for(int j=0;j<battleGame.board.board.GetLength(1);j++)
            {
                var tile = battleGame.board.board[i, j];
                /*
                if(tile.TempChar == '*')
                {
                    tempEffectList.Add(LoadTempEffect(tile));
                }
                 * */
                
                if(tile.TempChar != ' ')
                {
                     tempEffectList.Add(LoadTempEffect(tile));
                }
               
            }
        }
    }

    private GameObject LoadTempEffect(Tile t)
    {
        int effectIndex = 102;
        GameObject effectSprite = new GameObject("Effect");

        SpriteRenderer renderer = effectSprite.AddComponent<SpriteRenderer>();
        effectSprite.transform.position = new Vector3(t.x, t.y, -2);

        renderer.sprite = assetLibrary.getSprite(SpritesheetType.Particles, effectIndex);


        return effectSprite;
    }
    

    //load a sprite from resources and add as Game Object
    public void LoadTile(bool isWall, int x, int y)
    {

        GameObject spriteObject = new GameObject("Tile " + name);
        SpriteRenderer renderer = spriteObject.AddComponent<SpriteRenderer>();
        spriteObject.transform.position = new Vector3(x, y, 0);
        if(isWall)
        {
            renderer.sprite = assetLibrary.getSprite(SpritesheetType.Tiles, 27);
        
        }
        else
        {
            renderer.sprite = assetLibrary.getSprite(SpritesheetType.Tiles, 2);
           
        }
       
    }

    //Init the camera to middle of game board
    private void SetCamera()
    {
        int x = (int)Mathf.Round(battleGame.board.board.GetLength(0) / 2f);
        int y = (int)Mathf.Round(battleGame.board.board.GetLength(1) / 2f);

        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.transform.position = new Vector3(x, y, -10);

    }


    // Update is called once per frame
    void Update()
    {
        UpdateDebug();
        UITimer -= Time.deltaTime; 
        if(UITimer <= 0)
        {
            UpdateBattle();
            UITimer = .5f;
            LoadTempEffects();
        }

        UpdateCamera();
        ClickTile();

    }

    void UpdateDebug()
    {
        DebugText.text = string.Format("UIState: {0} BattleState {1}", uiState, battleGame.getBattleStatus());
    }

    void UpdateBattle()
    {
        battleGame.board.ClearTempTiles();

        //Update Map
        LoadCharacters();

        //Update UI
        UpdateInitiativePanel();

        switch(uiState)
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
        FocusCamera(battleGame.ActiveCharacter.x, battleGame.ActiveCharacter.y);

        //Update Map
        LoadCharacters();

        //Update UI
        UpdateInitiativePanel();

        //Update Abilities
        LoadAbilityList();

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
        if (uiState == UIStateType.PlayerDecide)
        {
            battleGame.NextTurn();
            uiState = UIStateType.NewTurn;

            /*
            battleGame.actionQueue.Add(new BattleAction() { character = battleGame.ActiveCharacter, actionType = BattleActionType.EndTurn });
            uiState = UIStateType.PlayerExecute;
             */
        }
    }

    public void PlayerMoveStart()
    {
        HideAbilityPanel();

        if (uiState == UIStateType.PlayerDecide)
        {
            DebugText.text = string.Format("Select Destination");
            clickPoint = null;
            playerDecideState = PlayerDecideState.MovePendingClick;
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
        HideAbilityPanel();

        if(uiState == UIStateType.PlayerDecide)
        {
            DebugText.text = string.Format("Select Target");
            clickPoint = null;
            playerDecideState = PlayerDecideState.AttackPendingClick;
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
        HideAbilityPanel();

        if (uiState == UIStateType.PlayerDecide)
        {
            DebugText.text = string.Format("Select Target");
            clickPoint = null;
            playerDecideState = PlayerDecideState.RangedAttackPendingClick;
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

    public void PlayerAbilityStart(Ability selectedAbility)
    {
        if(uiState == UIStateType.PlayerDecide)
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
            HideAbilityPanel();
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

    private void SelectTile()
    {
        Destroy(SelectedTile);
        SelectedTile = new GameObject("SelectedTile");
        SpriteRenderer renderer = SelectedTile.AddComponent<SpriteRenderer>();
        SelectedTile.transform.position = new Vector3(clickPoint.x, clickPoint.y, 0);
        renderer.sprite = Resources.Load<Sprite>("highlightTile");
    }

    private void DeselectTile()
    {
        Destroy(SelectedTile);
        SelectedTile = null;
    }

    private void UpdateCamera()
    {
        float speed = 100;
        float vert = Input.GetAxis("Vertical") * speed;
        float hor = Input.GetAxis("Horizontal") * speed;

        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        var destination = mainCamera.transform.position + new Vector3(hor, vert, 0);
        var velocity = Vector3.zero;
        float dampTime = 0.3f;

        var tempPosition = Vector3.SmoothDamp(mainCamera.transform.position, destination, ref velocity, dampTime);

        //mainCamera.transform.position = tempPosition;
        mainCamera.transform.position = MoveCamera(mainCamera, mainCamera.transform.position, tempPosition);

    }

    private void FocusCamera(int x, int y)
    {
        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.transform.position = new Vector3(x, y,-10);

    }


    //http://answers.unity3d.com/questions/501893/calculating-2d-camera-bounds.html
    private Vector3 MoveCamera(GameObject camera, Vector3 oldPos, Vector3 newPos)
    {


        float vertExtent = Camera.main.camera.orthographicSize;
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

    public void ShowAbilityPanel()
    {
        var abilityPanel = GameObject.FindGameObjectWithTag("AbilitiesPanel");
        MoveUIObject(abilityPanel, new Vector3(-251, -268, -1));
       
    }

    public void HideAbilityPanel()
    {
        var abilityPanel = GameObject.FindGameObjectWithTag("AbilitiesPanel");
        MoveUIObject(abilityPanel, new Vector3(265, -532, 0));
     
    }

    private void MoveUIObject(GameObject uiObject, Vector3 newPos)
    {
        var uiRectTransform = uiObject.GetComponent<RectTransform>();
        uiRectTransform.localPosition = newPos;
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

    private void LoadInitiative()
    {
        if (battleGame != null)
        {
            foreach (var character in battleGame.characterList)
            {
                GameObject charPortrait = (GameObject)Instantiate(InitPrefab);
                charPortrait = updateCharPortrait(charPortrait, character);

                charPortrait.transform.SetParent(InitiativePanel.transform, true);
            }
        }

    }

    private GameObject updateCharPortrait(GameObject charPortrait, GameCharacter character)
    {

        if (character == battleGame.ActiveCharacter)
        {
            var panelImg = charPortrait.GetComponent<Image>();
            panelImg.color = new Color(.8f, .8f, 0, .5f);

        }

        UpdateSpriteComponent(charPortrait, "PortraitImage", assetLibrary.getSprite(SpritesheetType.Portraits, 0));

        UpdateTextComponent(charPortrait, "CharacterName", character.name.ToString());

        string stats = string.Format("HP: {0}/{1} AP: {2}", character.hp, character.totalHP, character.ap);
        UpdateTextComponent(charPortrait, "CharacterStats", stats);

        return charPortrait;
    }

    private void UpdateTextComponent(GameObject parent, string componentName, string text)
    {
        foreach (var comp in parent.GetComponentsInChildren<Text>())
        {
            if (comp.name == componentName)
            {
                comp.text = text;
            }
        }
    }

    private void UpdateSpriteComponent(GameObject parent, string componentName, Sprite sprite)
    {
        foreach (var comp in parent.GetComponentsInChildren<Image>())
        {
            if (comp.name == componentName)
            {
                comp.sprite = sprite;
            }
        }
    }

    private void LoadAbilityList()
    {
        var usableAbilityList = (from data in battleGame.ActiveCharacter.abilityList
                                 where data.uses > 0
                                 select data).ToList();

        var AbilityItemPrefab = Resources.Load<GameObject>("AbilityPrefab");

        Transform AbilityPanel = GameObject.FindGameObjectWithTag("AbilityContentPanel").transform;

        //Clear existing abilities
        DestroyAllChildren(AbilityPanel);
       
        foreach (var ab in usableAbilityList)
        {
            GameObject abilityItem = (GameObject)Instantiate(AbilityItemPrefab);
            updateAbilityButton(abilityItem, assetLibrary.getSprite(SpritesheetType.Particles, 0), ab);
            abilityItem = updateAbilityItem(abilityItem, ab);
            abilityItem.transform.SetParent(AbilityPanel, true);
        }
    }

    private void DestroyAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    private void updateAbilityButton(GameObject parent, Sprite sprite, Ability selectedAbility)
    {
        Image buttonImage = parent.GetComponentInChildren<Image>();
        buttonImage.overrideSprite = sprite;

        Button buttonClick = parent.GetComponentInChildren<Button>();
        buttonClick.onClick.AddListener(() => PlayerAbilityStart(selectedAbility));
    }

    private GameObject updateAbilityItem(GameObject abilityItem, Ability a)
    {
        string abilityText = string.Format("{0} - {1}. AP: {2} Uses: {3}",a.name,a.description,a.ap,a.uses);
       
        //set click on icon
        UpdateTextComponent(abilityItem, "AbilityText", abilityText);

        return abilityItem;
    }


    #endregion


}

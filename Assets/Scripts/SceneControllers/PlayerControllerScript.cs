using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour {

    private float speed = .05f;
    private float moveAmt = 0.5f;

    private bool canMove = true;

    public Bounds playerBounds;

    private TileSceneControllerScript tileSceneScript;

    private Camera mainCamera;
    
	// Use this for initialization
	void Start () {
        setRefs();
        setPlayerRect();
	}

    private void setRefs()
    {
        this.tileSceneScript = GameObject.FindObjectOfType<TileSceneControllerScript>().GetComponent<TileSceneControllerScript>();
        this.mainCamera = GameObject.FindObjectOfType<Camera>();

       
    }

    private void setPlayerRect()
    {
        var box2d = this.gameObject.GetComponent<BoxCollider2D>();
        playerBounds = box2d.bounds;

    }


	// Update is called once per frame
	void Update () {
        UpdateCamera();
        setPlayerRect();
      

        tileSceneScript.checkPlayerObjectCollision(playerBounds);

        float moveY=0.0f;
        float moveX=0.0f;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            canMove = false;
          
            moveX = -moveAmt;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            canMove = false;
   
            moveX = moveAmt;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            canMove = false;
         
            moveY = moveAmt;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            canMove = false;
            moveY = -moveAmt;
        }

        if(Input.GetKeyUp(KeyCode.LeftArrow)){
            canMove = true;
        }
        if(Input.GetKeyUp(KeyCode.RightArrow)){
             canMove = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            canMove = true;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            canMove = true;
        }

            UpdateCamera();
        
    
        var pos = this.gameObject.transform.position;

        var newPos = new Vector3(pos.x + moveX, pos.y + moveY);

        Move(newPos);
       
	}

    public void Move(Vector3 newPos)
    {
        if (!checkCollision(getNewBounds(newPos)))
        {
            this.gameObject.transform.position = newPos;
        }
    }

    private void UpdateCamera()
    {
        var cameraPos = this.mainCamera.transform.position;
        var newPos = this.transform.position;
        this.mainCamera.transform.position = new Vector3(Mathf.Lerp(cameraPos.x,newPos.x,.1f),Mathf.Lerp(cameraPos.y,newPos.y,.1f),cameraPos.z);
    }

    private Bounds getNewBounds(Vector3 newPos)
    {
        Bounds newBounds = playerBounds;
        newBounds.center = newPos;
        return newBounds;

    }

    private bool checkCollision(Bounds bounds)
    {
        if (tileSceneScript.tileMapData != null)
        {
            return tileSceneScript.tileMapData.checkCollision(bounds);

        }
        return false;
       
    }
}

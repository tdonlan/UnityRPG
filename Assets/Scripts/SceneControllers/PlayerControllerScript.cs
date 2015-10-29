using UnityEngine;
using System.Collections.Generic;

public class PlayerControllerScript : MonoBehaviour {

    private float speed = 5;
    private float moveAmt = 0.5f;

    private bool canMove = true;

    public Vector3 moveDestination;

    public Vector3 velocity;

    public Bounds playerBounds;

    private ZoneControllerScript zoneControllerScript;

    private Camera mainCamera;
    
	// Use this for initialization
	void Start () {
        setRefs();
        setPlayerRect();
	}

    private void setRefs()
    {
        this.zoneControllerScript = GameObject.FindObjectOfType<ZoneControllerScript>().GetComponent<ZoneControllerScript>();
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
      
        zoneControllerScript.checkPlayerObjectCollision(playerBounds);

        if (Vector3.Distance(gameObject.transform.position, moveDestination) > .1f)
        {
            UpdateMove();
        }
     

       
	}

    private void UpdateMove()
    {
        var dir = moveDestination - this.gameObject.transform.position;
       
        dir.Normalize();

        velocity = dir * Time.deltaTime * speed;

        this.gameObject.transform.position += velocity;

    }

    public void Move(Vector3 newPos)
    {
        if (!checkCollision(getNewBounds(newPos)))
        {
        
            moveDestination = newPos;
        }
    }

    //Used when spawning character
    public void SetPosition(Vector3 newPos)
    {
        gameObject.transform.position = newPos;
        moveDestination = newPos;
            
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
        if (zoneControllerScript.tileMapData != null)
        {
            return zoneControllerScript.tileMapData.checkCollision(bounds);

        }
        return false;
       
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerScript : MonoBehaviour
{

    BoxCollider2D playerCol;
    Rigidbody2D rb;
    public GraveScript lastGrave;

	public RespawnScript spawnManager;
    public ScoreScript scoreManager;
	public CollisionScript collManager;

    private string LeftTrigger;
	private string RightTrigger;
    private string LeftBumper;
    private string RightBumper;
	private string LeftX;
	private string LeftY;
	private string RightX;
	private string RightY;
	private string AButton;
	private string RightStickClick;

	public int playerNum;
	public int enemyPlayerNum;
	private int scoreInt = 1;

    public GameObject flowerPrefab;
	public int flowerScoreInt = 3;
	public float flowerForce = 1000;

    public float moveSpeed;
    public float maxSpeed;
    public float friction;

	public float hitForce;
	public float hitstunTimer;
	public float hitstunMaxTimer;

    public Vector2 acceleration;
    public Vector2 speed;
    public Vector2 force;
    public Vector2 position;

    public int dashCount;
    public float dashSpeed;
    public int dashMax;
    public float dashResetTimer;
    public float dashResetMaxTime;

    public bool canDig;
    public bool inHitstun;

	bool prevUp, prevDown;

    public bool isAnchoredDig;
	public bool isAnchoredBury;

	public bool shovelDownDig;
	public bool shovelDownBury;


    public ShovelScript playerShovel;

    // Use this for initialization
    void Start()
    {
        LeftTrigger = "p" + playerNum + "LeftTrigger";
		RightTrigger = "p" + playerNum + "RightTrigger";
        LeftBumper = "p" + playerNum + "LeftBumper";
        RightBumper = "p" + playerNum + "RightBumper";
        LeftX = "p" + playerNum + "LeftX";
        LeftY = "p" + playerNum + "LeftY";
        RightX = "p" + playerNum + "RightX";
        RightY = "p" + playerNum + "RightY";
        AButton = "p" + playerNum + "A";
		RightStickClick = "p" + playerNum + "RightStickClick";


        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<BoxCollider2D>();
        lastGrave = null;
        inHitstun = false;

		//get enemy player num once lol this took me years
		enemyPlayerNum = (playerNum == 1) ? 2 : 1;

        isAnchoredDig = false;
		isAnchoredBury = false;

        dashCount = dashMax;

		playerShovel = GetComponentInChildren<ShovelScript> ();

        acceleration = Vector2.zero;
        force = Vector2.zero;
        speed = Vector2.zero;

        spawnManager = GameObject.Find ("RespawnManager").GetComponent<RespawnScript>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreScript>();
		collManager = GameObject.Find("CollisionManager").GetComponent<CollisionScript>();


        transform.position = spawnManager.getSpawnpoint(playerNum).position;

		position = transform.position;

	}

    public void addForce(Vector2 newForce)
    {
        force += newForce;
    }

    // Update is called once per frame
    void Update()
    {
        //check for dig state
		if (Input.GetAxisRaw(LeftTrigger) == 0)
        {
            isAnchoredDig = false;
        }

		if (Input.GetAxisRaw(RightTrigger) == 0)
		{
			isAnchoredBury = false;
		}


		//movement
		//if (!isAnchoredDig && !isAnchoredBury) {

			Vector2 moveVec = new Vector2 (Input.GetAxis (LeftX) * moveSpeed, -Input.GetAxis (LeftY) * moveSpeed);

			addForce (moveVec);


			//dash
			if (Input.GetButtonDown (LeftBumper) && dashCount > 0) {
				addForce (moveVec * 15);
				--dashCount;

				if (dashCount < dashMax) {
					dashResetTimer = 0f;
				}
			}

			if (dashCount < dashMax && dashResetTimer < dashResetMaxTime) {
				dashResetTimer += Time.deltaTime;

				if (dashResetTimer >= dashResetMaxTime) {
					dashCount++;
					dashResetTimer = 0f;
				}
			}
		
			addForce (-speed * friction);

			speed = speed + force * Time.deltaTime;

			float magnitude = Mathf.Min (speed.magnitude, maxSpeed);
			speed = speed.normalized * magnitude;

			position = position + speed * Time.deltaTime;

			force = Vector2.zero;

			transform.position = position;
		//}


		//rotating player
        if (Input.GetAxis(RightX) != 0 || Input.GetAxis(RightY) != 0)
        {

			//godLIKE
			float angle = Mathf.Atan2(Input.GetAxis(RightX), Input.GetAxis(RightY)) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, angle), Time.deltaTime * 1000);

        }

		//shovel spinning
		if (Input.GetButtonDown(RightBumper)) {

			playerShovel.spinShovel ();
		}

		//flower throw
		if (Input.GetButtonDown(RightStickClick)) {

			throwFlowers ();
		}


		bool isDown = Input.GetAxisRaw(LeftTrigger) == 1;
		bool isUp = Input.GetAxisRaw(LeftTrigger) == 0;
		bool neither = !isUp && !isDown;


		if (isUp && prevDown) {
			
			//dig
			--lastGrave.currentState;
			
			lastGrave.updateGraveState();
			
			if (lastGrave.currentState == GraveScript.DigState.DUG)
			{
				//scoremanager
				scoreManager.incrementPlayerScore (playerNum, scoreInt, transform);
			}
		}
		
		//if trigger is up & was previously down
		//do digging

		if (isDown) {
			
			prevDown = true;

		}

		if (isUp) {
			prevDown = false;
		}








		//checking states for digging or burying
		/*if (canDig && Input.GetAxisRaw(LeftTrigger) > 0)//Input.GetAxis(RightTrigger) > 0)
		{


			shovelDownDig = true;

			Debug.Log ("shoveldown");
			
			if (lastGrave != null && shovelDownDig && lastGrave.currentState != GraveScript.DigState.DUG) {

				if (Input.GetAxisRaw(LeftTrigger) == 0) {
					
					
					
				}
			}
		}*/
		
		//prevUp = isUp;
		//prevDown = isDown;
		
		if (canDig && Input.GetAxisRaw(RightTrigger) > 0)//Input.GetAxis(RightTrigger) > 0)
		{
			++lastGrave.currentState;
			
			lastGrave.updateGraveState();
			
			
			
			if (lastGrave.currentState == GraveScript.DigState.UNDUG)
			{
				//scoremanager
				if (lastGrave.scoreValue > 0) 
				{
					scoreManager.incrementPlayerScore(playerNum, lastGrave.scoreValue, transform);
				}
				
				lastGrave.cashout();
				
			}
			
		}

		//digging
		if (isAnchoredDig && !isAnchoredBury) {
	//		Debug.Log(Input.GetAxis(RightX));

			if (Input.GetAxis(RightX) < 0 && !inHitstun) {


			}

		}

		//burying
		if (isAnchoredBury && !isAnchoredDig) {
			//Debug.Log(Input.GetAxis(RightX));

			if ( Input.GetAxis(RightX) > 0 && !inHitstun) {

				shovelDownBury = true;

			}

			if (shovelDownBury && Input.GetAxis(RightX) < 0 && !inHitstun && lastGrave != null && lastGrave.currentState != GraveScript.DigState.UNDUG) {

				shovelDownBury = false;

			}

		}

    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        //make sure your objects are tagged, dipshit
		if (collision.CompareTag("shovel") && collision.GetComponent<ShovelScript>().shovelNum != playerNum)
		{			 
			collManager.sendCollisionData (collision.gameObject, this.gameObject, CollisionScript.CollisionType.SHOVEL_PLAYER);
		}

		if (collision.CompareTag("undead") && playerShovel.spinning == false)
		{
			collManager.sendCollisionData (collision.gameObject.transform.parent.gameObject, this.gameObject, CollisionScript.CollisionType.UNDEAD_PLAYER);
		}

		if (collision.CompareTag("grave"))
        {
			collManager.sendCollisionData (this.gameObject, collision.gameObject, CollisionScript.CollisionType.PLAYER_GRAVE);
        }

		if (collision.CompareTag("pit"))
        {
			collManager.sendCollisionData (this.gameObject, collision.gameObject, CollisionScript.CollisionType.PLAYER_PIT);
        }
			
		if (collision.CompareTag("pit"))
		{
			collManager.sendCollisionData (this.gameObject, collision.gameObject, CollisionScript.CollisionType.PLAYER_PIT);
		}
		if (collision.CompareTag ("flowers")) {
			collManager.sendCollisionData (collision.gameObject, this.gameObject, CollisionScript.CollisionType.FLOWER_PLAYER);
		}
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
//        Debug.Log(collision.GetComponentInParent<GraveScript>().currentState);

		if (collision.CompareTag("digRange") )
		{
			lastGrave = collision.GetComponentInParent<GraveScript> ();

			canDig = true;
		}
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
		if (collision.CompareTag("digRange"))
        {
            lastGrave = null;

            canDig = false;
        }
    }

    public void triggerDeath()
    {
		Destroy(this.gameObject);

        //trigger respawn
		spawnManager.respawnPlayer(playerNum);
		scoreManager.incrementPlayerScore (enemyPlayerNum, scoreInt, this.transform);

    }

	public void pickupFlowers()
	{
		scoreManager.incrementPlayerScore (playerNum, flowerScoreInt, this.transform);
	}
		
	public void throwFlowers()
	{
		if (scoreManager.getPlayerScore(playerNum) - flowerScoreInt >= 0) {
			
			scoreManager.incrementPlayerScore (playerNum, -flowerScoreInt, this.transform);

			GameObject tmp = Instantiate (flowerPrefab, transform.position, Quaternion.identity);

			tmp.GetComponent<Rigidbody2D> ().AddForce (transform.up * flowerForce);
		}
	}

}

using System.Collections;
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
    
    private string LeftTrigger;
	private string RightTrigger;
    private string LeftBumper;
    private string RightBumper;
	private string LeftX;
	private string LeftY;
	private string RightX;
	private string RightY;
	private string AButton;

	public int playerNum;

    public GameObject dirtPrefab;
    public int dirtCount;

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

        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<BoxCollider2D>();
        lastGrave = null;
        inHitstun = false;

        isAnchoredDig = false;
		isAnchoredBury = false;

        dirtCount = 0;
        dashCount = dashMax;

		playerShovel = GetComponentInChildren<ShovelScript> ();

        acceleration = Vector2.zero;
        force = Vector2.zero;
        speed = Vector2.zero;

        spawnManager = GameObject.Find ("RespawnManager").GetComponent<RespawnScript>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreScript>();

        transform.position = spawnManager.getSpawnpoint(playerNum).position;

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
		if (!isAnchoredDig && !isAnchoredBury) {

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
		}


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

		
		//checking states for digging or burying
		if (canDig && Input.GetAxisRaw(LeftTrigger) > 0)//Input.GetAxis(RightTrigger) > 0)
		{
			isAnchoredDig = true;
		}
		
		if (canDig && Input.GetAxisRaw(RightTrigger) > 0)//Input.GetAxis(RightTrigger) > 0)
		{
			isAnchoredBury = true;
		}

		//digging
		if (isAnchoredDig && !isAnchoredBury) {
			Debug.Log(Input.GetAxis(RightX));

			if (Input.GetAxis(RightX) < 0 && !inHitstun) {

				shovelDownDig = true;

			}

			if (shovelDownDig && Input.GetAxis(RightX) > 0 && !inHitstun && lastGrave.currentState != GraveScript.DigState.DUG) {

                ++dirtCount;

				shovelDownDig = false;

				--lastGrave.currentState;
				
				lastGrave.updateGraveState();
			}

		}

		//burying
		if (isAnchoredBury && !isAnchoredDig) {
			Debug.Log(Input.GetAxis(RightX));

			if (dirtCount > 0 && Input.GetAxis(RightX) > 0 && !inHitstun) {

				shovelDownBury = true;

			}

			if (dirtCount > 0 && shovelDownBury && Input.GetAxis(RightX) < 0 && !inHitstun && lastGrave.currentState != GraveScript.DigState.UNDUG) {

				shovelDownBury = false;

                --dirtCount;

				++lastGrave.currentState;

				lastGrave.updateGraveState();

                if (lastGrave.currentState == GraveScript.DigState.UNDUG)
                {
                    //scoremanager
                    scoreManager.incrementPlayerScore(playerNum, lastGrave.scoreValue);

                    lastGrave.cashout();

                }
			}

		}

    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        //make sure your objects are tagged, dipshit
		if (collision.tag == "shovel" && collision.GetComponent<ShovelScript>().shovelNum != playerNum)
		{

			//Debug.Log ("adding force?");
			 
			Vector2 hitVec = new Vector2(transform.position.x - collision.GetComponentInParent<Transform>().position.x, transform.position.y - collision.GetComponentInParent<Transform>().position.y);

			addForce (hitVec.normalized * hitForce);

            //drop dirt
            if (dirtCount > 0)
            {
                Debug.Log("dropping dirt");
                dropDirt();
            }
		}


        if (collision.tag == "grave")
        {
            dropDirt();

            collision.GetComponentInParent<GraveScript>().incrementHoleScore();

            triggerDeath();

        }

        if (collision.tag == "pit")
        {

            dropDirt();

            triggerDeath();
        }

        if (collision.tag == "dirt")
        {
            ++dirtCount;
            Destroy(collision.gameObject);
        }
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
//        Debug.Log(collision.GetComponentInParent<GraveScript>().currentState);

		if (collision.tag == "digRange") 
		{
			lastGrave = collision.GetComponentInParent<GraveScript> ();

			canDig = true;
		}
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "digRange")
        {
            lastGrave = null;

            canDig = false;
        }
    }

    public void triggerDeath()
    {

        //trigger respawn
		spawnManager.respawnPlayer(playerNum);

		Destroy(this.gameObject);
    }

    public void dropDirt()
    {
        if (dirtCount > 0)
        {
         --dirtCount;
        }

         Instantiate(dirtPrefab, transform.position, Quaternion.identity);
    }

}

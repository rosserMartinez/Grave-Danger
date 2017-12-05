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
    public CollisionScript collManager;


    public GameObject dashParticles;


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
    public Vector2 acceleration;
    public Vector2 speed;
    public Vector2 force;
    public Vector2 position;
    public Vector2 moveVec;

    public float rotSpeed;

    public int dashCount;
    public float dashSpeed;
    public int dashMax;
    public float dashResetTimer;
    public float dashResetMaxTime;

    public bool canDig;

    bool prevUpRight, prevUpLeft, prevDownLeft, prevDownRight;

    public ShovelScript playerShovel;


    //HP
    public int health;
    public int maxHealth;
    SpriteRenderer rend;

    //hitstun bools?
    public bool markedForDeath;
    public bool inHitstun;

    public float hitForce;
    public float hitstunTimer;
    public float hitstunMaxTimer;

    public Color baseColor;
    public Color hitstunColor;


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

        rend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<BoxCollider2D>();
        lastGrave = null;
        inHitstun = false;

        //get enemy player num once lol this took me years
        enemyPlayerNum = (playerNum == 1) ? 2 : 1;

        dashCount = dashMax;

        playerShovel = GetComponentInChildren<ShovelScript>();

        //   acceleration = Vector2.zero;
        force = Vector2.zero;
        speed = Vector2.zero;

        spawnManager = GameObject.Find("RespawnManager").GetComponent<RespawnScript>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreScript>();
        collManager = GameObject.Find("CollisionManager").GetComponent<CollisionScript>();

        transform.position = spawnManager.getSpawnpoint(playerNum).position;

        health = maxHealth;
        hitstunTimer = hitstunMaxTimer;

        //position = transform.position;

    }

    public void addForce(Vector2 newForce)
    {
        //force += newForce;
        rb.AddForce(newForce);

    }

    // Update is called once per frame
    void Update()
    {

        moveVec = new Vector2(Input.GetAxis(LeftX) * moveSpeed, -Input.GetAxis(LeftY) * moveSpeed);

        if (!inHitstun)
        {
             addForce(moveVec);

             addForce(-speed * friction);
        }

        //dash
        if (Input.GetButtonDown(LeftBumper) && dashCount > 0)
        {
            addForce(moveVec * 15);
            --dashCount;

            GameObject puff = Instantiate(dashParticles, transform.position + new Vector3(0, 0, 1), Quaternion.identity);
            // puff.transform.parent = gameObject.transform;   

            if (dashCount < dashMax)
            {
                dashResetTimer = 0f;
            }
        }

        if (dashCount < dashMax && dashResetTimer < dashResetMaxTime)
        {
            dashResetTimer += Time.deltaTime;

            if (dashResetTimer >= dashResetMaxTime)
            {
                dashCount++;
                dashResetTimer = 0f;
            }
        }

        //force = Vector2.zero;
        /*
			speed = speed + force * Time.deltaTime;

			float magnitude = Mathf.Min (speed.magnitude, maxSpeed);
			speed = speed.normalized * magnitude;

			position = position + speed * Time.deltaTime;


			transform.position = position; */
        //



        if (inHitstun)
        {

            hitstunTimer -= Time.deltaTime;

            if (hitstunTimer <= 0)
            {
                inHitstun = false;
                hitstunTimer = hitstunMaxTimer;
                rend.color = baseColor;

                if (markedForDeath)
                {
                    triggerDeath();
                }

            }
        }


        //rotating player
        if (Input.GetAxis(RightX) != 0 || Input.GetAxis(RightY) != 0)
        {

            //godLIKE
            float angle = Mathf.Atan2(Input.GetAxis(RightX), Input.GetAxis(RightY)) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * rotSpeed);

        }

        //shovel spinning
        if (Input.GetButtonDown(RightBumper))
        {

            playerShovel.spinShovel();
        }

        //flower throw
        if (Input.GetButtonDown(RightStickClick))
        {

            throwFlowers();
        }


        bool isDownLeft = Input.GetAxisRaw(LeftTrigger) == 1;
        bool isUpLeft = Input.GetAxisRaw(LeftTrigger) == 0;
        //bool neither = !isUpLeft && !isDownLeft;

        if (isUpLeft && prevDownLeft)
        {

            if (lastGrave != null && lastGrave.currentState != GraveScript.DigState.DUG)
            {
                //dig
                --lastGrave.currentState;

                lastGrave.updateGraveState();

            }
        }

        //if trigger is up & was previously down
        //do digging
        if (isDownLeft)
        {

            prevDownLeft = true;

        }

        if (isUpLeft)
        {
            prevDownLeft = false;
        }

        bool isDownRight = Input.GetAxisRaw(RightTrigger) == 1;
        bool isUpRight = Input.GetAxisRaw(RightTrigger) == 0;

        if (isUpRight && prevDownRight)
        {

            if (lastGrave != null && lastGrave.currentState != GraveScript.DigState.UNDUG)
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
        }

        //if trigger is up & was previously down
        //do digging
        if (isDownRight)
        {
            prevDownRight = true;
        }

        if (isUpRight)
        {
            prevDownRight = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //make sure your objects are tagged, dipshit
        if (collision.CompareTag("shovel") && collision.GetComponent<ShovelScript>().shovelNum != playerNum)
        {
            collManager.sendCollisionData(collision.gameObject, this.gameObject, CollisionScript.CollisionType.SHOVEL_PLAYER);
        }

        if (collision.CompareTag("undead") && playerShovel.spinning == false)
        {
            collManager.sendCollisionData(collision.gameObject.transform.parent.gameObject, this.gameObject, CollisionScript.CollisionType.UNDEAD_PLAYER);
        }

        if (collision.CompareTag("grave"))
        {
            collManager.sendCollisionData(this.gameObject, collision.gameObject, CollisionScript.CollisionType.PLAYER_GRAVE);
        }

        if (collision.CompareTag("pit"))
        {
            collManager.sendCollisionData(this.gameObject, collision.gameObject, CollisionScript.CollisionType.PLAYER_PIT);
        }

        if (collision.CompareTag("flowers"))
        {
            collManager.sendCollisionData(collision.gameObject, this.gameObject, CollisionScript.CollisionType.FLOWER_PLAYER);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("digRange"))
        {
            lastGrave = collision.GetComponentInParent<GraveScript>();

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
        //Debug.Log("Triggering death");

        spawnManager.respawnPlayer(playerNum);
        scoreManager.incrementPlayerScore(enemyPlayerNum, scoreInt, this.transform);
    }

    public void pickupFlowers()
    {
        scoreManager.incrementPlayerScore(playerNum, flowerScoreInt, this.transform);
    }

    public void throwFlowers()
    {
        if (scoreManager.getPlayerScore(playerNum) - flowerScoreInt >= 0)
        {

            scoreManager.incrementPlayerScore(playerNum, -flowerScoreInt, this.transform);

            GameObject tmp = Instantiate(flowerPrefab, transform.position, Quaternion.identity);

            //tmp.GetComponent<Rigidbody2D> ().AddForce (transform.up * flowerForce);
        }
    }

    public void damageHealth()
    {
        --health;

        inHitstun = true;
        rend.color = hitstunColor;

        if (health == 0)
        {
            markedForDeath = true;
        }
    }

}

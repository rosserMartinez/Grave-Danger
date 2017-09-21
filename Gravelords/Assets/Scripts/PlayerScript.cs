using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerScript : MonoBehaviour
{

    BoxCollider2D playerCol;
    Rigidbody2D rb;
    public GraveScript lastGrave;

    private string RightTrigger;
    private string LeftBumper;
    private string RightBumper;
	private string LeftX;
	private string LeftY;
	private string RightX;
	private string RightY;
	private string AButton;

	public int playerNum;

    public float moveSpeed;
    public float maxSpeed;
    public float friction;

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
    public bool isAnchored;


    public ShovelScript playerShovel;

    // Use this for initialization
    void Start()
    {
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
        isAnchored = false;

        dashCount = dashMax;

		playerShovel = GetComponentInChildren<ShovelScript> ();

        
    }

    public void addForce(Vector2 newForce)
    {
        force += newForce;
    }

    // Update is called once per frame
    void Update()
    {
        //movement

        if (Input.GetAxis(RightTrigger) > 0)
        {
            isAnchored = false;
        }

        if (!isAnchored)
        {

            Vector2 moveVec = new Vector2(Input.GetAxis(LeftX) * moveSpeed, -Input.GetAxis(LeftY) * moveSpeed);

            //rb.velocity = moveVec;
            addForce(moveVec);

            //Debug.Log(Input.GetButtonDown("p1A"));

            //dash
            if (Input.GetButtonDown(LeftBumper) && dashCount > 0)
            {
                //Debug.Log(Input.GetButtonDown("p1A"));
                addForce(moveVec * 15);
                --dashCount;

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

            addForce(-speed * friction);


            speed = speed + force * Time.deltaTime;



            float magnitude = Mathf.Min(speed.magnitude, maxSpeed);
            speed = speed.normalized * magnitude;



            position = position + speed * Time.deltaTime;

            force = Vector2.zero;

            transform.position = position;
        }

        if (Input.GetAxis(RightX) != 0 || Input.GetAxis(RightY) != 0)
        {

			//godLIKE
			float angle = Mathf.Atan2(Input.GetAxis(RightX), Input.GetAxis(RightY)) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, angle), Time.deltaTime * 1000);

        }


		if (Input.GetButtonDown(RightBumper)) {

			playerShovel.spinShovel ();
		}

        Debug.Log(Input.GetAxis(RightTrigger));

        if (canDig && Input.GetAxis(RightTrigger) < 0)
        {
            //Debug.Log(Input.GetButtonUp("p1A"));

            isAnchored = true;


            //if (Input.GetButtonUp("p1X") && !inHitstun && lastGrave.currentState != GraveScript.DigState.DUG)
            //{
            //    --lastGrave.currentState;

            //    lastGrave.updateGraveState();

            //}
            //else if (Input.GetButtonUp("p1B") && !inHitstun && lastGrave.currentState != GraveScript.DigState.UNDUG)
            //{
            //    ++lastGrave.currentState;

            //    lastGrave.updateGraveState();
            //}
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
//        Debug.Log(collision.GetComponentInParent<GraveScript>().currentState);

		if (collision.tag == "digRange") {
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

}

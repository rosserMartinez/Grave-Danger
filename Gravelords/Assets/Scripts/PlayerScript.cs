using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerScript : MonoBehaviour
{

    BoxCollider2D playerCol;
    Rigidbody2D rb;
    public GraveScript lastGrave;

    public string LeftX;
    public string LeftY;

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
    //public bool dashCooldown;

    public bool canDig;
    public bool inHitstun;

	public ShovelScript playerShovel;

    // Use this for initialization
    void Start()
    {
        LeftX = "p1LeftX";
        LeftY = "p1LeftY";

        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<BoxCollider2D>();
        lastGrave = null;
        inHitstun = false;

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


        Vector2 moveVec = new Vector2(Input.GetAxis(LeftX) * moveSpeed, -Input.GetAxis(LeftY) * moveSpeed);

        //rb.velocity = moveVec;
        addForce(moveVec);

        //Debug.Log(Input.GetButtonDown("p1A"));

        //dash
        if (Input.GetButtonDown("p1A") && dashCount > 0)
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



        var magnitude = Mathf.Min(speed.magnitude, maxSpeed);
        speed = speed.normalized * magnitude;



        position = position + speed * Time.deltaTime;

        force = Vector2.zero;

        transform.position = position;


        if (Input.GetAxis("p1RightX") != 0 || Input.GetAxis("p1RightY") != 0)
        {

			//godLIKE
			float angle = Mathf.Atan2(Input.GetAxis("p1RightX"), Input.GetAxis("p1RightY")) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, angle), Time.deltaTime * 1000);

        }


		if (Input.GetButtonDown("p1RightBumper")) {

			playerShovel.spinShovel ();
		}

        if (canDig)
        {
            //Debug.Log(Input.GetButtonUp("p1A"));

            if (Input.GetButtonUp("p1X") && !inHitstun && lastGrave.currentState != GraveScript.DigState.DUG)
            {
                //GamePad.SetVibration(PlayerIndex.One, .3f, .3f);
                --lastGrave.currentState;

                lastGrave.updateGraveState();

                //GamePad.SetVibration(PlayerIndex.One, .0f, .0f);

            }
            else if (Input.GetButtonUp("p1B") && !inHitstun && lastGrave.currentState != GraveScript.DigState.UNDUG)
            {
                ++lastGrave.currentState;

                lastGrave.updateGraveState();
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
//        Debug.Log(collision.GetComponentInParent<GraveScript>().currentState);

        if (collision.tag == "digRange")
        {
            lastGrave = collision.GetComponentInParent<GraveScript>();

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

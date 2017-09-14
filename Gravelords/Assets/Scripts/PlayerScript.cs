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

    public int moveSpeed;

    public bool canDig;

    // Use this for initialization
    void Start()
    {
        LeftX = "p1LeftX";
        LeftY = "p1LeftY";

        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<BoxCollider2D>();
        lastGrave = null;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 moveVec = new Vector2(Input.GetAxis(LeftX) * moveSpeed, -Input.GetAxis(LeftY) * moveSpeed);

        rb.velocity = moveVec;

        // Debug.Log(Input.GetButton("p1A"));

        if (canDig)
        {
            Debug.Log(Input.GetButtonUp("p1A"));

            if (Input.GetButtonUp("p1A") && lastGrave.currentState != GraveScript.DigState.DUG)
            {

                --lastGrave.currentState;

                lastGrave.updateGraveState();
            }
            else if (Input.GetButtonUp("p1B") && lastGrave.currentState != GraveScript.DigState.UNDUG)
            {
                ++lastGrave.currentState;

                lastGrave.updateGraveState();
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.GetComponentInParent<GraveScript>().currentState);

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

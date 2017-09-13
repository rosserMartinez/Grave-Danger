using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerScript : MonoBehaviour {


    Rigidbody2D rb;
    public string LeftX;
    public string LeftY;

    public int moveSpeed;

    // Use this for initialization
    void Start () {
        LeftX = "p1LeftX";
        LeftY = "p1LeftY";

        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        Vector2 moveVec = new Vector2(Input.GetAxis(LeftX) * moveSpeed, -Input.GetAxis(LeftY) * moveSpeed);

        rb.velocity = moveVec;

        Debug.Log(Input.GetButton("p1A"));
    }
}

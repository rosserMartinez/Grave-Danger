using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtScript : MonoBehaviour {

    private CircleCollider2D dirtBubble;

    float bubbleTimer;


	// Use this for initialization
	void Start () {

        dirtBubble = GetComponent<CircleCollider2D>();

        dirtBubble.enabled = false;
        bubbleTimer = 2;
	}
	
	// Update is called once per frame
	void Update () {

        if (dirtBubble.enabled == false)
        {
            bubbleTimer -= Time.deltaTime;

            bubbleTimer = Mathf.Max(bubbleTimer, 0);


            if (bubbleTimer == 0)
            {
                dirtBubble.enabled = true;
            }
        }
    }
}

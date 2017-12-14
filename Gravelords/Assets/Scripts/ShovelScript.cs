using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelScript : MonoBehaviour {


	public bool spinning;

	public float spinTimer;
	public float spinMaxTimer;

    public bool onCooldown;

    public float cooldown;
    public float cooldownMax;

	public float spinSpeed;

	public int shovelNum;

	CapsuleCollider2D hitbox;

	// Use this for initialization
	void Start () {
		spinning = false;

		hitbox = GetComponent<CapsuleCollider2D>();
		shovelNum = GetComponentInParent<PlayerScript> ().playerNum;

		hitbox.enabled = false;


        onCooldown = false;
        cooldown = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

        if (onCooldown)
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0)
            {
                onCooldown = false;
            }

        }

		if (spinTimer < spinMaxTimer && spinning)
		{
			transform.Rotate (0, 0, spinSpeed * Time.deltaTime);
			spinTimer += Time.deltaTime;

			if (spinTimer > spinMaxTimer)
			{
				spinning = false;
				hitbox.enabled = false;
				transform.localRotation = Quaternion.identity;

                spinTimer = 0.0f;
                cooldown = cooldownMax;
                onCooldown = true;
			}
		}

	}

	public void spinShovel()
	{
        //transform.localRotation = Quaternion.identity;
        if (!onCooldown)
        { 
		    spinning = true;
		    hitbox.enabled = true;
        }
	}


}

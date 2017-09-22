using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelScript : MonoBehaviour {


	public bool spinning;

	public float spinTimer;
	public float spinMaxTimer;
	public float spinSpeed;

	public int shovelNum;

	CapsuleCollider2D hitbox;

	// Use this for initialization
	void Start () {
		spinning = false;

		hitbox = GetComponent<CapsuleCollider2D>();
		shovelNum = GetComponentInParent<PlayerScript> ().playerNum;

		hitbox.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (spinTimer < spinMaxTimer && spinning)
		{
			transform.Rotate (0, 0, spinSpeed * Time.deltaTime);
			spinTimer += Time.deltaTime;

			if (spinTimer > spinMaxTimer)
			{
				spinning = false;
				hitbox.enabled = false;
				transform.localRotation = Quaternion.identity;
			}
		}
	}

	public void spinShovel()
	{
		//transform.localRotation = Quaternion.identity;
		spinning = true;
		hitbox.enabled = true;
		spinTimer = 0f;
	}


}

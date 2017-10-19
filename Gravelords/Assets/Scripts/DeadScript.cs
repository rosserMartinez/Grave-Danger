using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadScript : MonoBehaviour {

	bool tracking;

	public PlayerScript lastPlayerDetected;

	CircleCollider2D sightRange;

	public float moveSpeed;

	// Use this for initialization
	void Start () {
		tracking = false;
		sightRange = GetComponentInChildren<CircleCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {


		if (lastPlayerDetected != null) 
		{
			transform.position = Vector2.MoveTowards (transform.position, lastPlayerDetected.transform.position, moveSpeed * Time.deltaTime);	
		}
	}


	public void OnTriggerStay2D(Collider2D coll)
	{

		if (coll.gameObject.tag == "shovel" && coll.gameObject.GetComponentInParent<PlayerScript>() != null) 
		{
			//Debug.Log ("collision occuring");
			
			lastPlayerDetected = coll.gameObject.GetComponentInParent<PlayerScript> ();

		}


		if (coll.gameObject.tag == "pit") 
		{
			triggerDeathAgain ();
		}

		if (coll.gameObject.tag == "grave") 
		{
			
		}
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "shovel")
		{
		//	lastPlayerDetected = null;
		}
	}

	public void triggerDeathAgain()
	{

		//trigger respawn
		//spawnManager.respawnPlayer(playerNum);

		Destroy(this.gameObject);
	}
}

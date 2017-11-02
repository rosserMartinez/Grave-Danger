using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadScript : MonoBehaviour {

	Vector2 p1Position;
	Vector2 p2Position;
	public Vector2 lastPlayerDetected;
	public bool? pathToPlayer1;

	CircleCollider2D sightRange;

	public Vector2 speed;
	public Vector2 force;
	public Vector2 position;

	public Vector2 moveVec;

	public float hitForce;

	public float moveSpeed;
	public float maxSpeed;
	public float friction;

	// Use this for initialization
	void Start () 
	{

		force = Vector2.zero;
		speed = Vector2.zero;

		position = transform.position;

	}

	public void addForce(Vector2 newForce)
	{
		force += newForce;
	}


	// Update is called once per frame
	void Update () {

		if (RespawnScript.player1Instance != null) {	 
			
			p1Position = RespawnScript.player1Instance.transform.position;

		}

		if (RespawnScript.player2Instance != null) {	
			
			p2Position = RespawnScript.player2Instance.transform.position;
		}


			lastPlayerDetected = (Vector2.Distance(transform.position, p1Position) < Vector2.Distance(transform.position, p2Position)) ? p1Position : p2Position;

			//transform.position = Vector2.MoveTowards (transform.position, lastPlayerDetected, moveSpeed * Time.deltaTime);	


		moveVec = new Vector2 (lastPlayerDetected.x - transform.position.x, lastPlayerDetected.y - transform.position.y).normalized;

		addForce (moveVec * moveSpeed);

		addForce (-speed * friction);

		speed = speed + force * Time.deltaTime;

		float magnitude = Mathf.Min (speed.magnitude, maxSpeed);
		speed = speed.normalized * magnitude;

		position = position + speed * Time.deltaTime;

		force = Vector2.zero;

		transform.position = position;

	}


	public void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "shovel") 
		{
			Vector2 hitVec = new Vector2(transform.position.x - coll.transform.position.x, transform.position.y - coll.transform.position.y);

			addForce (hitVec.normalized * hitForce * 1.8f);

		}	
	}

	public void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "shovel") 
		{

		}

		if (coll.gameObject.tag == "pit") 
		{
			triggerDeathAgain ();
		}

		if (coll.gameObject.tag == "grave") 
		{
			triggerDeathAgain ();
			coll.GetComponentInParent<GraveScript>().incrementHoleScore();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadScript : MonoBehaviour {

	Vector2 p1Position;
	Vector2 p2Position;
	public Vector2 lastPlayerDetected;
	public bool? pathToPlayer1;

	Rigidbody2D rb;

	public CollisionScript collManager;

	public Vector2 speed;
	public Vector2 force;
	public Vector2 position;

	public Vector2 moveVec;

	public float hitForce;

	public float moveSpeed;
	public float maxSpeed;
    public float friction;

    public GameObject explosion;
    GameObject tmpExplosion;

    // Use this for initialization
    void Start () 
	{

		force = Vector2.zero;
		speed = Vector2.zero;

		position = transform.position;

		rb = GetComponent<Rigidbody2D> ();
		collManager = GameObject.Find("CollisionManager").GetComponent<CollisionScript>();
	}

	public void addForce(Vector2 newForce)
	{
		//force += newForce;
		rb.AddForce (newForce);
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

		addForce ((-speed * friction));

		//addForce (moveVec * moveSpeed);

		//addForce (-speed * friction);
		/*
		//DONT DO THESE 
		 speed = speed + force * Time.deltaTime;

		float magnitude = Mathf.Min (speed.magnitude, maxSpeed);
		speed = speed.normalized * magnitude;

		position = position + speed * Time.deltaTime;

		force = Vector2.zero;

		transform.position = position; */
		//DONT DO THESE 
	}


	public void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.CompareTag("shovel")) 
		{
			collManager.sendCollisionData(coll.gameObject, this.gameObject, CollisionScript.CollisionType.SHOVEL_UNDEAD);
		}	
	}

	public void OnTriggerStay2D(Collider2D coll)
	{

		if (coll.CompareTag ("pit")) 
		{
			collManager.sendCollisionData (this.gameObject, coll.gameObject, CollisionScript.CollisionType.UNDEAD_PIT);
		}

		if (coll.CompareTag("grave")) 
		{
			collManager.sendCollisionData(this.gameObject, coll.gameObject, CollisionScript.CollisionType.UNDEAD_GRAVE);
		}
			
	}

	public void triggerDeathAgain()
	{

		//trigger respawn
		//spawnManager.respawnPlayer(playerNum);

		Destroy(this.gameObject);

        tmpExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
    }
}

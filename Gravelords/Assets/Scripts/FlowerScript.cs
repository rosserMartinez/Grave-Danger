using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerScript : MonoBehaviour {

	CapsuleCollider2D cap;

	CollisionScript collManager;

	public float maxTimer;
	public float timer;

	public float maxSpeed;
	public float friction;
	public float smoothRot;
	public float startForce;
	public Vector2 speed;
	public Vector2 force;
	public Vector2 position;

	public void addForce(Vector2 newForce)
	{
		force += newForce;
	}

	// Use this for initialization
	void Start () {
		timer = maxTimer;
		cap = GetComponent<CapsuleCollider2D> ();
		cap.enabled = false;

		collManager = GameObject.Find("CollisionManager").GetComponent<CollisionScript>();

	//	force = Vector2.zero;
	//	speed = Vector2.zero;

	//	position = transform.position;

	//	addForce (Vector2.up *startForce);
	}
	
	// Update is called once per frame
	void Update () {
		
		timer -= Time.deltaTime;

		if (timer <= 0) {
			cap.enabled = true;
		} else {
			
			transform.Rotate (0, 0, smoothRot * Time.deltaTime);
		
			/*addForce (-speed * friction);
		
			speed = speed + force * Time.deltaTime;
		
			float magnitude = Mathf.Min (speed.magnitude, maxSpeed);
			speed = speed.normalized * magnitude;
		
			position = position + speed * Time.deltaTime;
		
			force = Vector2.zero;
		
			transform.position = position;	*/	


		}

	}

	void OnTriggerEnter2D( Collider2D collision)
	{
		if (collision.CompareTag("pit"))
		{
			collManager.sendCollisionData (this.gameObject, collision.gameObject, CollisionScript.CollisionType.FLOWER_PIT);
		}

		if (collision.CompareTag("grave"))
		{
			collManager.sendCollisionData (this.gameObject, collision.gameObject, CollisionScript.CollisionType.FLOWER_GRAVE);
		}
	}
}

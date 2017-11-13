using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour {

	public float playerHitForce;
	public float undeadHitForce;

	public int dirtIncrement;

	PlayerScript player;
	DeadScript undead;
	GraveScript grave;
	GameObject pit;

	public enum CollisionType
	{
		//naming convention, gameobj a will be first, b will be after underscore
		SHOVEL_PLAYER,
		PLAYER_GRAVE,
		PLAYER_PIT,
		SHOVEL_UNDEAD,
		UNDEAD_GRAVE,
		UNDEAD_PIT,
		UNDEAD_PLAYER,
		FLOWER_PLAYER,
		FLOWER_GRAVE,
		FLOWER_PIT

	}

	// Use this for initialization
	void Start () {

		//Time.timeScale = .25f;

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void sendCollisionData(GameObject a, GameObject b, CollisionType c)
	{

		switch (c) {
		case CollisionType.SHOVEL_PLAYER:
			player = b.GetComponent<PlayerScript> ();
			
			Vector2 hitVec = new Vector2 (b.transform.position.x - a.transform.position.x, b.transform.position.y - a.transform.position.y);
			
			player.addForce (hitVec.normalized * playerHitForce);
			break;
		case CollisionType.PLAYER_GRAVE:
			player = a.GetComponent<PlayerScript> ();
			grave = b.GetComponentInParent<GraveScript> ();
			
			grave.incrementHoleScore ();
			
			player.triggerDeath ();
			break;
		case CollisionType.PLAYER_PIT:
			player = a.GetComponent<PlayerScript> ();
			
			player.triggerDeath ();
			break;

		case CollisionType.SHOVEL_UNDEAD:
			undead = b.GetComponent<DeadScript> ();
			
			Vector2 hitVecShovel = new Vector2 (b.transform.position.x - a.transform.position.x, b.transform.position.y - a.transform.position.y);
			
			undead.addForce (hitVecShovel.normalized * undeadHitForce);
			break;

		case CollisionType.UNDEAD_GRAVE:
			undead = a.GetComponent<DeadScript> ();
			grave = b.GetComponentInParent<GraveScript> ();
			
			undead.triggerDeathAgain ();
			grave.incrementHoleScore ();
			break;

		case CollisionType.UNDEAD_PIT:
			undead = a.GetComponent<DeadScript> ();
			
			undead.triggerDeathAgain ();
			break;

		case CollisionType.UNDEAD_PLAYER:
			player = b.GetComponent<PlayerScript> ();
			
			Vector2 hitVecUndead = new Vector2 (b.transform.position.x - a.transform.position.x, b.transform.position.y - a.transform.position.y);
			
			player.addForce (hitVecUndead.normalized * undeadHitForce);
			break;

		case CollisionType.FLOWER_PLAYER:
			player = b.GetComponent<PlayerScript> ();
			player.pickupFlowers ();
			Destroy (a.gameObject);
			break;

		case CollisionType.FLOWER_GRAVE:
			Destroy (a.gameObject);
			break;

		case CollisionType.FLOWER_PIT:
			Destroy (a.gameObject);
			break;
		default:
			break;
		}


	}



}

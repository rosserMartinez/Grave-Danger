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
		UNDEAD_PLAYER

	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void sendCollisionData(GameObject a, GameObject b, CollisionType c)
	{
		if (c == CollisionType.SHOVEL_PLAYER) {
			
			player = b.GetComponent<PlayerScript> ();

			Vector2 hitVec = new Vector2 (b.transform.position.x - a.transform.position.x, b.transform.position.y - a.transform.position.y);

			player.addForce (hitVec.normalized * playerHitForce);

		} 
		else if (c == CollisionType.PLAYER_GRAVE) 
		{

			player = a.GetComponent<PlayerScript> ();
			grave = b.GetComponentInParent<GraveScript> ();

			grave.incrementHoleScore ();

			player.triggerDeath ();

		}
		else if (c == CollisionType.PLAYER_PIT) 
		{
			player = a.GetComponent<PlayerScript> ();

			player.triggerDeath ();
		} 
		else if (c == CollisionType.SHOVEL_UNDEAD)
		{
			undead = b.GetComponent<DeadScript> ();

			Vector2 hitVec = new Vector2 (b.transform.position.x - a.transform.position.x, b.transform.position.y - a.transform.position.y);

			undead.addForce (hitVec.normalized * undeadHitForce);

		}
		else if (c == CollisionType.UNDEAD_GRAVE)
		{
			undead = a.GetComponent<DeadScript> ();
			grave = b.GetComponentInParent<GraveScript> ();

			undead.triggerDeathAgain ();
			grave.incrementHoleScore();
		}
		else if (c == CollisionType.UNDEAD_PIT)
		{
			undead = a.GetComponent<DeadScript> ();

			undead.triggerDeathAgain ();
		}
		else if (c == CollisionType.UNDEAD_PLAYER)
		{
			player = b.GetComponent<PlayerScript>();

			Vector2 hitVec = new Vector2(b.transform.position.x - a.transform.position.x, b.transform.position.y - a.transform.position.y);

			player.addForce(hitVec.normalized * undeadHitForce);

		}

	}



}

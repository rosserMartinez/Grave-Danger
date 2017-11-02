using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour {

	public enum CollisionType
	{
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
		
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour {


    public GameObject player1;
    public GameObject player2;

    public Transform p1Respawn;
    public Transform p2Respawn;

    public float respawnTimerP1;
    public float respawnTimerP2;

    public bool p1Respawning;
    public bool p2Respawning;

    public float respawnTimerMax;

	public int player1Num;
	public int player2Num;

	// Use this for initialization
	void Start () {

        p1Respawning = false;
        p2Respawning = false;

        respawnTimerP1 = respawnTimerMax;
        respawnTimerP2 = respawnTimerMax;

		player1Num = player1.GetComponent<PlayerScript>().playerNum;
		player2Num = player2.GetComponent<PlayerScript>().playerNum;
    }
	
	// Update is called once per frame
	void Update () {

        if (p1Respawning)
        {
            respawnTimerP1 -= Time.deltaTime;

            respawnTimerP1 = Mathf.Max(respawnTimerP1, 0);

			//Debug.Log(respawnTimerP1);

            if (respawnTimerP1 == 0)
            {
                p1Respawning = false;
				Instantiate (player1, p1Respawn);
            }
        }

        if (p2Respawning)
        {
			respawnTimerP2 -= Time.deltaTime;

			respawnTimerP2 = Mathf.Max(respawnTimerP2, 0);


			if (respawnTimerP2 == 0)
			{
				p2Respawning = false;
				Instantiate (player2, p2Respawn);
			}        
		}

	}


	public void respawnPlayer(int num)
    {

		if (num == player1Num) {

			respawnTimerP1 = respawnTimerMax;
			//Debug.Log (respawnTimerP1);
			p1Respawning = true;

		}
		if (num == player2Num) {
			
			respawnTimerP2 = respawnTimerMax;
			//Debug.Log (respawnTimerP1);
			p2Respawning = true;

		}
    }



	public Transform getSpawnpoint(int num)
	{
		if (num == player1Num) {
			return p1Respawn;
		}
		if (num == player2Num) {
			return p2Respawn;
		} 
		else
			return this.transform;
	}

}

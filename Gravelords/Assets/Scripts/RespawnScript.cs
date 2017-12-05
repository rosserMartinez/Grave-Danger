using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour {

	public static GameObject player1Instance;
	public static GameObject player2Instance;

    public GameObject player1;
    public GameObject player2;

	public GraveScript grave1;
	public GraveScript grave2;
	public GraveScript grave3;

    public GraveScript[] graves;

    public Transform p1Respawn;
    public Transform p2Respawn;

    public float respawnTimerP1;
    public float respawnTimerP2;
	public float respawnTimerUndead;

    public bool p1Respawning;
    public bool p2Respawning;
	public bool undeadSpawning;

    public float respawnTimerMax;
	public float undeadSpawnMax;

	public int player1Num;
	public int player2Num;

	// Use this for initialization
	void Start () {

		p1Respawning = true;
		p2Respawning = true;
		undeadSpawning = true;

		respawnTimerP1 = respawnTimerMax;
		respawnTimerP2 = respawnTimerMax;
		respawnTimerUndead = 2f;

		player1Num = player1.GetComponent<PlayerScript>().playerNum;
		player2Num = player2.GetComponent<PlayerScript>().playerNum;

        //graves = FindObjectsOfType(GraveScript);

        GameObject graveList = GameObject.Find("Graves");

        graves = graveList.GetComponentsInChildren<GraveScript>();

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
				player1Instance = Instantiate (player1, p1Respawn);

            }
        }

        if (p2Respawning)
        {
			respawnTimerP2 -= Time.deltaTime;

			respawnTimerP2 = Mathf.Max(respawnTimerP2, 0);


			if (respawnTimerP2 == 0)
			{
				p2Respawning = false;
				player2Instance = Instantiate (player2, p2Respawn);

			}        
		}

		if (undeadSpawning) {

			respawnTimerUndead -= Time.deltaTime;

			respawnTimerUndead = Mathf.Max (respawnTimerUndead, 0);

			if (respawnTimerUndead == 0) {
                //p2Respawning = false;

                //grave1.spawnUndead ();
                //grave2.spawnUndead ();
                //grave3.spawnUndead ();

                for (int i = 0; i < graves.Length; ++i)
                {
                    graves[i].spawnUndead();
                }


				respawnTimerUndead = undeadSpawnMax;
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

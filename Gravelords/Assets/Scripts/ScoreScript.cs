using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour {

    public int p1Score;
    public int p2Score;

    // Use this for initialization
    void Start() {

        p1Score = 0;
        p2Score = 0;

    }

    // Update is called once per frame
    void Update() {

    }

    public void incrementPlayerScore (int playerNum, int scoreToAdd)
    {
        if (playerNum == 1)
        {
            p1Score += scoreToAdd;
        }
        else if (playerNum == 2)
        {
            p2Score += scoreToAdd;
        }
    }

}

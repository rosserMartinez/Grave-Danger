using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startWait : MonoBehaviour {

    public int playerInControl;
    public string p1Start;

    public string mainScreen;

    // Use this for initialization
    void Start () {

        p1Start = "p" + playerInControl + "Start";
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown(p1Start))
        {
            SceneManager.LoadScene(mainScreen);
        }
    }
}

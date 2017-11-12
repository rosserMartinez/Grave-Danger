using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsMenu : MonoBehaviour {

	public string splashScreen;

	private string RightBumper;
	public int playerInControl;

	// Use this for initialization
	void Start () {

		RightBumper = "p" + playerInControl + "RightBumper";
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetButtonDown(RightBumper)) 
		{
			SceneManager.LoadScene (splashScreen);
		}

	}
}

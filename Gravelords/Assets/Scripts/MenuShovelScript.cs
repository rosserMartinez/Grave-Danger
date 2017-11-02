using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuShovelScript : MonoBehaviour {

	public Transform startPos;
	public Transform controlsPos;
	public Transform quitPos;

	public enum Option { START, CONTROLS, QUIT };

	public int playerInControl;
	public int frames;
	public int frameBuffer;
	public Option currentState;

	private string LeftY;
	private string RightBumper;

	public string controlScreen;
	public string game;

	//borrowed from normal shovel
	public bool spinning;

	public float spinTimer;
	public float spinMaxTimer;
	public float spinSpeed;

	// Use this for initialization
	void Start () {
		spinning = false;
		spinTimer = spinMaxTimer;
		this.transform.position = startPos.position;

		LeftY = "p" + playerInControl + "LeftY";
		RightBumper = "p" + playerInControl + "RightBumper";

	}
	
	// Update is called once per frame
	void Update () {

		++frames;

		if (!spinning && frames % frameBuffer == 0 && Input.GetAxis(LeftY) < 0)
		{
			//Debug.Log (Input.GetAxisRaw (LeftY));
			--currentState;
			updateMenuState();
		}

		if (!spinning && frames % frameBuffer == 0 && Input.GetAxis (LeftY) > 0)
		{
			//Debug.Log (Input.GetAxisRaw (LeftY));
			++currentState;
			updateMenuState();
		}


		if (currentState > Option.QUIT) 
		{
			currentState = Option.QUIT;
		}

		if (currentState < Option.START)
		{
			currentState = Option.START;
		}

		if (Input.GetButtonDown(RightBumper)) {

			spinShovel ();
		}



		if (spinTimer > 0 && spinning)
		{
			transform.Rotate (0, 0, spinSpeed * Time.deltaTime);
			spinTimer -= Time.deltaTime;

			if (spinTimer <= 0)
			{
				spinning = false;
				transform.localRotation = Quaternion.identity;

				selectMenu ();
			}
		}

	}

	void updateMenuState()
	{
		if (currentState == Option.START)
		{
			this.transform.position = startPos.position;
		}
		if (currentState == Option.CONTROLS)
		{
			this.transform.position = controlsPos.position;
		}
		if (currentState == Option.QUIT)
		{
			this.transform.position = quitPos.position;
		}
	}

	void selectMenu()
	{

		if (currentState == Option.START)
		{
			SceneManager.LoadScene (game);
		}
		if (currentState == Option.CONTROLS)
		{
			SceneManager.LoadScene (controlScreen);
		}
		if (currentState == Option.QUIT)
		{
			Application.Quit ();
		}
	}

	public void spinShovel()
	{
		spinning = true;
		spinTimer = spinMaxTimer;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreScript : MonoBehaviour {

    public int p1Score;
    public int p2Score;

	public int startMessage;

	public Text p1Text;
	public Text p2Text;
	public Text goalText;
	public Text startText;

	public Text graveText1;
	public Text graveText2;
	public Text graveText3;

	public RespawnScript respawn;

	private GameObject tmp;
	public GameObject spawnTextObject;
	public Color p1TextColor;
	public Color p2TextColor;

	public bool gameStart;
	public bool gameOver;
	public float startTimer;
	public float fightTime;
	public bool? p1Won;

	//messages
	private string baseString;
	private string winString;
	private string startString;
	private string endString;

	public string gameLevel;
	public string startLevel;


	//menu controls
	private string p1Back;
	private string p1Start;
	private string p2Back;
	private string p2Start;


	private int fightSize = 160;
	private int startSize = 70;
	private int endSize = 50;

	public int goalPoints;

    // Use this for initialization
    void Start() {

		Time.timeScale = 1.0f;

		p1Won = null;

		startText.enabled = true;


		graveText1.enabled = false;
		graveText2.enabled = false;
		graveText3.enabled = false;

		gameStart = false;
		gameOver = false;
        p1Score = 0;
        p2Score = 0;

		p1Back = "p1Back";
     	p1Start = "p1Start";
    	p2Back = "p2Back";
     	p2Start = "p2Start";

		baseString = "FIRST TO ";
		endString = "LOSER PICKS! \n <START> TO RUN IT BACK \n <BACK> TO HOLD THE L";

		goalText.text = baseString + goalPoints;

		startMessage = Random.Range (0, 6); //num options EXCLUSIVE

		switch (startMessage) {
		case 5:
			startString = "PUT 'EM IN A COFFIN!";
			break;
		case 4:
			startString = "BETTER THEM THAN YOU!";
			break;
		case 3:
			startString = "SHOW 'EM WHO'S BOSS!";
			break;
		case 2:
			startString = "IT'S THE BATTLE OF THE CENTURY!";
			break;
		case 1:
			startString = "DARE TO BELIEVE YOU CAN SURVIVE!";
			break;
		default:
			startString = "DIG OR BE DUG!";
			break;
		}

		startText.text = startString;
		startText.fontSize = startSize;


		respawn.respawnPlayer (1);
		respawn.respawnPlayer (2);


    }

    // Update is called once per frame
    void Update() {
		
		startTimer -= Time.deltaTime;

		if (!gameStart && startTimer <= fightTime) 
		{
			startString = "FIGHT!";
			startText.text = startString;
			startText.fontSize = fightSize;
		}
			
		if (!gameOver && startTimer <= 0) 
		{
			//start game
			gameStart = true;
			startText.enabled = false;
			goalText.enabled = true;
			p1Text.enabled = true;
			p2Text.enabled = true;
			graveText1.enabled = true;
			graveText2.enabled = true;
			graveText3.enabled = true;
		}


		if (p1Won != null)
		{

			//p2 runback
			if (p1Won == true && Input.GetButtonDown(p2Start))
			{
				//Debug.Log ("p2 runback");
				SceneManager.LoadScene (gameLevel);
			}
			
			//p1 runback
			if (p1Won == false && Input.GetButtonDown(p1Start))
			{
				//Debug.Log ("p1 runback");
				SceneManager.LoadScene (gameLevel);
			}
			
			

			if (p1Won == true && Input.GetButtonDown(p2Back))
			{
				//Debug.Log ("p2 quit");
				SceneManager.LoadScene (startLevel);
			}
			
			//p1 quit
			if (p1Won == false && Input.GetButtonDown(p1Back))
			{
				//Debug.Log ("p1 quit");
				SceneManager.LoadScene (startLevel);
			}


		}
    }

	public void incrementPlayerScore (int playerNum, int scoreToAdd, Transform textPos)
    {
		if (!gameOver) {
			
        if (playerNum == 1)
        {

		   	p1Score += scoreToAdd;
			p1Text.text = p1Score.ToString();

//				p1Text.gameObject.transform.DOPunchScale (Vector3.one * 1.3f,0.3f);
		   	
				tmp = Instantiate (spawnTextObject, textPos.position, Quaternion.identity);
		   	
				if (tmp.GetComponent<FloatScript>() != null)
				{
					FloatScript pointsText = tmp.GetComponent<FloatScript> ();
		   	
					//alter values for the heads up
					pointsText.points = scoreToAdd;
					//pointsText.textObj.color = p1TextColor;
		   	
				}
		   	
		   	
			if 	(p1Score >= goalPoints) 
			{  	
				winString = "PLAYER 1 WINS!";
				goalText.text = winString;
				gameOver = true;
				startText.enabled = true;
		   	
				graveText1.enabled = false;
				graveText2.enabled = false;
				graveText3.enabled = false;
				
				//slowmo?
				Time.timeScale = .5f;
		   	
				startText.text = endString;
				startText.fontSize = endSize;
				
				p1Won = true;
			}  	
        }
        else if (playerNum == 2)
        {
            p2Score += scoreToAdd;
			p2Text.text = p2Score.ToString();

				tmp = Instantiate (spawnTextObject, textPos.position, Quaternion.identity);

				if (tmp.GetComponent<FloatScript>() != null)
				{
					FloatScript pointsText = tmp.GetComponent<FloatScript> ();

					//alter values for the heads up
					pointsText.points = scoreToAdd;
					//pointsText.textObj.color = p2TextColor;

				}

			if (p2Score >= goalPoints) 
			{
				winString = "PLAYER 2 WINS!";
				goalText.text = winString;
				gameOver = true;
				startText.enabled = true;

				Time.timeScale = .5f;
				
				graveText1.enabled = false;
				graveText2.enabled = false;
				graveText3.enabled = false;

				startText.text = endString;
				startText.fontSize = endSize;

				p1Won = false;
			}
        }
	

		}
    }

	public int getPlayerScore(int playerNum)
	{
		if (playerNum == 1)
			return p1Score;
		else
			return p2Score;
	}

}

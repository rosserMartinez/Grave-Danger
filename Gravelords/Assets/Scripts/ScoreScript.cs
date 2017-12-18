using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ScoreScript : MonoBehaviour {

    public int p1Score;
    public int p2Score;

	public int startMessage;

	public TextMeshProUGUI p1Text;
	public TextMeshProUGUI p2Text;
	public TextMeshProUGUI goalText;
	public TextMeshProUGUI startText;

	//public Text graveText1;
	//public Text graveText2;
	//public Text graveText3;

    public TextMeshPro[] inscriptions;

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

    int p1num = 1;
    int p2num = 2;

	//messages
	private string baseString;
	private string winString;
	private string startString;
	private string endString;

	public string gameLevel;
	public string startLevel;

    Vector3 baseScale;
    float punchScale = 1.3f;
    float punchDuration = 0.3f;
    float half = .5f;
    float full = 1f;

	//menu controls
	private string p1Back;
	private string p1Start;
	private string p2Back;
	private string p2Start;


	private int fightSize = 166;
	private int startSize = 90;
	private int endSize = 60;

	public int goalPoints;

    // Use this for initialization
    void Start() {



        p1Text = GameObject.Find("p1TextPro").GetComponent<TextMeshProUGUI>();
        p2Text = GameObject.Find("p2TextPro").GetComponent<TextMeshProUGUI>();
        goalText = GameObject.Find("GoalTextPro").GetComponent<TextMeshProUGUI>();
        startText = GameObject.Find("READYTextPro").GetComponent<TextMeshProUGUI>();
          
        //p2Text = GetComponent<TextMeshProUGUI>();
        //goalText = GetComponent<TextMeshProUGUI>();
        //startText = GetComponent<TextMeshProUGUI>();



        baseScale = new Vector3(1f, 1f, 1f);

		Time.timeScale = full;

		p1Won = null;


        startText.enabled = true;
        //startText.text.fontSize = 0;

        GameObject graveTextList = GameObject.Find("graveTextList");

        inscriptions = graveTextList.GetComponentsInChildren<TextMeshPro>();

        for (int i = 0; i < inscriptions.Length; ++i)
        {
            inscriptions[i].enabled = false;
        }

        //graveText1.enabled = false;
        //graveText2.enabled = false;
        //graveText3.enabled = false;

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
			startString = "DIGTOWN? AND YOU'RE THE MAYOR?!";
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
			//startText.enabled = false;
            startText.fontSize = 0; //pseudo disable
            goalText.enabled = true;
			p1Text.enabled = true;
			p2Text.enabled = true;

            for (int i = 0; i < inscriptions.Length; ++i)
            {
                inscriptions[i].enabled = true;
            }
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

                p1Text.rectTransform.localScale = baseScale;
                p1Text.gameObject.transform.DOPunchScale (Vector3.one * punchScale, punchDuration);

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
                //startText.enabled = true;
                startText.fontSize = startSize; //pseudo enable

                for (int i = 0; i < inscriptions.Length; ++i)
                {
                    inscriptions[i].enabled = false;
                }

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

                p1Text.rectTransform.localScale = baseScale;
                p1Text.gameObject.transform.DOPunchScale(Vector3.one * punchScale, punchDuration);

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
                    //startText.enabled = true;
                    startText.fontSize = startSize; //pseudo enable


                    for (int i = 0; i < inscriptions.Length; ++i)
                   {
                        inscriptions[i].enabled = false;
                   }

				Time.timeScale = .5f;
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

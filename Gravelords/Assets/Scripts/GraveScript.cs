using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraveScript : MonoBehaviour {

    SpriteRenderer rend;

    CapsuleCollider2D cap;

	//apublic GameObject ;

    public int scoreValue;

    public Sprite dugSprite;
    public Sprite halfSprite;
    public Sprite undugSprite;

    public enum DigState { DUG, HALFDUG, UNDUG };

    public DigState currentState;

	public Text graveText;
//	public float transparency = .6f;

	public GameObject undead;

    // Use this for initialization
    void Start()
    {
        scoreValue = 0;

        rend = GetComponent<SpriteRenderer>();
        cap = GetComponentInChildren<CapsuleCollider2D>();

        currentState = DigState.UNDUG;
        updateGraveState();

		graveText.text = scoreValue.ToString();
		//graveText.material.color = new Color(graveText.material.color.r, graveText.material.color.g, graveText.material.color.b, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateGraveState()
    {
        if (currentState == DigState.DUG)
        {
            rend.sprite = dugSprite;
            cap.enabled = true;
        }
        if (currentState == DigState.HALFDUG)
        {
            rend.sprite = halfSprite;
            cap.enabled = false;
        }
        if (currentState == DigState.UNDUG)
        {
            rend.sprite = undugSprite;
            cap.enabled = false;
        }
    }

    public void incrementHoleScore()
    {
        ++scoreValue;

		graveText.text = scoreValue.ToString();

    }

    public void cashout()
    {
        scoreValue = 0;

		graveText.text = scoreValue.ToString();
    }

	public void spawnUndead()
	{
		if (currentState == DigState.UNDUG) {

			Instantiate (undead, transform.position, Quaternion.identity);

		}
	}


}

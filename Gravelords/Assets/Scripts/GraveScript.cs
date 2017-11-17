using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GraveScript : MonoBehaviour {

    SpriteRenderer rend;

    CapsuleCollider2D cap;

	BoxCollider2D playerBox;

	//apublic GameObject ;

    public int scoreValue;

	public float hazeKill;

    public Sprite dugSprite;
    public Sprite halfSprite;
    public Sprite undugSprite;

	public GameObject haze;

	public GameObject playerColl;

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
		playerBox = playerColl.GetComponent<BoxCollider2D>();

		playerBox.enabled = true;

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
        if (currentState <= DigState.DUG)
        {
			currentState = DigState.DUG;
            rend.sprite = dugSprite;
            cap.enabled = true;
			playerBox.enabled = false;

        }
        if (currentState == DigState.HALFDUG)
        {
            rend.sprite = halfSprite;
            cap.enabled = false;
			playerBox.enabled = true;

        }
        if (currentState >= DigState.UNDUG)
        {
			currentState = DigState.UNDUG;
            rend.sprite = undugSprite;
            cap.enabled = false;
			playerBox.enabled = true;

        }
    }

    public void incrementHoleScore()
    {
        ++scoreValue;

		graveText.text = scoreValue.ToString();
		graveText.gameObject.transform.DOPunchScale (Vector3.one * 1.3f,0.2f);

	//	graveText.gameObject.transform.DOPunchScale
    }

    public void cashout()
    {
        scoreValue = 0;

		graveText.text = scoreValue.ToString();
		graveText.gameObject.transform.DOPunchScale (Vector3.one * 1.3f,0.2f);

    }

	public void spawnUndead()
	{
		if (currentState == DigState.UNDUG) {

			GameObject tmp = Instantiate (haze, transform.position + new Vector3(0, 0, 1), Quaternion.identity);

			Instantiate (undead, transform.position, Quaternion.identity);

			Destroy (tmp, hazeKill);

		}
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatScript : MonoBehaviour {

	public string textToDisplay;
	public float moveSpeed;
	public float lifeSpan;
	public float timer;
	public Text textObj;
	public Color textColor;
	public int points;

    public int textSize;

	// Use this for initialization
	void Start () {

		//points = 0;

		timer = lifeSpan;
		textObj = GetComponentInChildren<Text> ();

	}
	
	// Update is called once per frame
	void Update () {

		timer -= Time.deltaTime;

		if (timer < 0)
		{
			Destroy (gameObject);
		}

		if (moveSpeed > 0) 
		{
			transform.Translate (Vector3.up * moveSpeed * Time.deltaTime, Space.World);
		}

		if (points > 0)
		{
		//	textObj.color = textColor;
			textObj.text = "+" + points.ToString ();

		}


	}
}

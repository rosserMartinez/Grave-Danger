using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatScript : MonoBehaviour {

	public string textToDisplay;
	public float moveSpeed;
	public float lifeSpan;
	public float timer;
	public TextMeshPro textObj;
	public int points;

    MeshRenderer mesh;

	// Use this for initialization
	void Start () {

		//points = 0;

		timer = lifeSpan;
		textObj = GetComponentInChildren<TextMeshPro> ();


        //just in case
        mesh = GetComponentInChildren<MeshRenderer>();
        mesh.sortingOrder = 30;
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
        else
        {
            textObj.text = points.ToString();
        }


    }
}

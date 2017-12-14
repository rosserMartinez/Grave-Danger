using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPScript : MonoBehaviour {

    SpriteRenderer rend;

    public Sprite hearts5;
    public Sprite hearts4;
    public Sprite hearts3;
    public Sprite hearts2;
    public Sprite hearts1;
    public Sprite hearts0;

    const int hp5 = 5;
    const int hp4 = 4;
    const int hp3 = 3;
    const int hp2 = 2;
    const int hp1 = 1;
    const int hp0 = 0;


    Quaternion fixedRotation;

    Vector3 fixedLocalPosition;
    public Transform player;

    // Use this for initialization
    void Start () {

        rend = GetComponent<SpriteRenderer>();

        fixedLocalPosition = transform.localPosition;

        //fixedRotation = transform.rotation;

        //player.transform.rotation;

        rend.sprite = hearts5;

	}
	
	// Update is called once per frame
	void Update () {

      //  transform.rotation = fixedRotation;

       // transform.position = player.position + fixedLocalPosition;

    }

    public void updateVisual(int currentHP)
    {
        switch (currentHP)
        {
            case hp5:
                rend.sprite = hearts5;
                break;
            case hp4:
                rend.sprite = hearts4;
                break;
            case hp3:
                rend.sprite = hearts3;
                break;
            case hp2:
                rend.sprite = hearts2;
                break;
            case hp1:
                rend.sprite = hearts1;
                break;
            case hp0:
                rend.sprite = hearts0;
                break;
        }
    }

}

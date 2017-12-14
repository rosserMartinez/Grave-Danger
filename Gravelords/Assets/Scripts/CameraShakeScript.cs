using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeScript : MonoBehaviour {


    Vector3 startPos;

    public float shakeStrength;
    public float shakeStrengthMax;
    public float shakeDecay;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (shakeStrength > 0)
        {
            transform.position = startPos + Random.insideUnitSphere * shakeStrength;
            shakeStrength -= shakeDecay;
        }

	}

    public void initShake()
    {
        shakeStrength = shakeStrengthMax;
    }

}

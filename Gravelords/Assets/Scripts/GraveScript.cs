using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveScript : MonoBehaviour {

    SpriteRenderer rend;

    CapsuleCollider2D cap;

    public Sprite dugSprite;
    public Sprite halfSprite;
    public Sprite undugSprite;

    public enum DigState { DUG, HALFDUG, UNDUG };

    public DigState currentState;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        cap = GetComponentInChildren<CapsuleCollider2D>();

        currentState = DigState.UNDUG;
        updateGraveState();
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
}

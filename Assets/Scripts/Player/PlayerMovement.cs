using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    float PUSH_FORCE = 15.0f;
    float MAX_VELOCITY = 7.0f;
    Rigidbody2D cRigidbody;

    void Awake()
    {
        TouchManager.instance.OnStartTouch += OnTouch;
        GameManager.instance.OnStartGame += StartGame;
    }

    // Use this for initialization
    void Start () {
        cRigidbody = GetComponent<Rigidbody2D>();
        cRigidbody.isKinematic = true;
    }
	
	// Update is called once per frame
	void Update () {
	}


    // Use this for initialization
    void StartGame()
    {
        cRigidbody.isKinematic = false;
    }

    void OnTouch(TouchStruct touchStruct)
    {
        float currVelocity = cRigidbody.velocity.y;
        Vector2 newVelocity = new Vector2(0, Mathf.Clamp(currVelocity + PUSH_FORCE, -Mathf.Infinity, MAX_VELOCITY));
        cRigidbody.velocity = newVelocity;
    }

    private void OnDestroy()
    {
        TouchManager.instance.OnStartTouch -= OnTouch;
        GameManager.instance.OnStartGame -= StartGame;
    }
}

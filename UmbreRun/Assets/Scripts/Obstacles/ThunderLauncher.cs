using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderLauncher : MonoBehaviour {

    Thunder thunder;

    float TIME_MIN = 6;
    float TIME_MAX = 12;
    float timer;
    float targetTimer;
    // Use this for initialization
    void Start () {
        thunder = GetComponent<Thunder>();
        timer = 0;
    }
	
    void nextBolt()
    {
        targetTimer = Random.Range(TIME_MIN, TIME_MAX);
        timer = 0;
    }

	// Update is called once per frame
	void Update () {
        Debug.Log(timer);

        timer += Time.deltaTime;
        if(timer > targetTimer)
        {
            thunder.Play();
            nextBolt();
        }
    }
}

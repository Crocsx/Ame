using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour {

    public Text[] score;

	// Use this for initialization
	public void Setup () {
		for(int i = 0; i < score.Length; i++)
        {
            score[i].text = ScoreManager.Instance.Score.ToString();
        }

        ScoreManager.Instance.SaveScore();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

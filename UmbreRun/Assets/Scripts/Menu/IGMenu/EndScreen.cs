using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour {

    public Text[] score;

    public BestScore[] BestScore;

    [SerializeField]
    HighScore hScore;
    // Use this for initialization
    public void Setup () {
		for(int i = 0; i < score.Length; i++)
        {
            score[i].text = ((int)ScoreManager.Instance.Score).ToString();
        }

        ScoreManager.Instance.SaveScore();

        DisplayScore();
    }
	
	// Update is called once per frame
	void DisplayScore() {
        for (int i = 0; i < BestScore.Length; i++)
        {
            BestScore[i].SetScore(hScore.scores[i]);
        }
    }
}

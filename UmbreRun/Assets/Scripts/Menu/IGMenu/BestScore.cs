using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestScore : MonoBehaviour {

    public Text[] score;

    // Use this for initialization
    public void SetScore(int value) {
        for (int i = 0; i < score.Length; i++)
        {
            score[i].text = value.ToString();
        }
    }
}

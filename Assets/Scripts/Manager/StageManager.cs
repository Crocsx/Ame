using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

    public static StageManager instance = null;

    #region Singleton Initialization 
    void Awake()
    {

        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }
    #endregion

    // Use this for initialization
    void Start () {
        GameManager.instance.InitGame();
    }

    public void StartStage()
    {
        GameManager.instance.StartGame();
    }

    public void PauseStage()
    {
        GameManager.instance.PauseGame();
    }

    public void ResumeStage()
    {
        GameManager.instance.ResumeGame();
    }

    void OnDestroy ()
    {
    }
}

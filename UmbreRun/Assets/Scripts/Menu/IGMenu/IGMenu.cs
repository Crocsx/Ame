﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IGMenu : MonoBehaviour
{

    public GameObject pStart;
    public GameObject pPause;
    public GameObject pUI;
    public GameObject pEnd;

    GameObject currentPanel;

    void Start()
    {
        ShowPanel("UI");
    }

    public void ShowPanel(string name)
    {
        DeactivateCurrent();
        Activate(GetPanel(name));
    }

    // Update is called once per frame
    void DeactivateCurrent()
    {
        if (currentPanel == null)
            return;

        currentPanel.SetActive(false);
        currentPanel = null;
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void StartStage()
    {
        GameManager.Instance.SetNewState(GameManager.GameState.InGame);
    }

    public void PauseStage()
    {
        GameManager.Instance.SetNewState(GameManager.GameState.Pause);
    }

    public void UnPause()
    {
        GameManager.Instance.SetNewState(GameManager.GameState.InGame);
    }

    void Activate(GameObject panel)
    {
        currentPanel = panel;
        currentPanel.SetActive(true);
    }

    GameObject GetPanel(string name)
    {
        GameObject panel;
        switch (name)
        {
            case "Start":
                panel = pStart;
                break;
            case "End":
                panel = pEnd;
                break;
            case "UI":
                panel = pUI;
                break;
            case "Pause":
            default:
                panel = pPause;
                break;
        }
        return panel;
    }
}

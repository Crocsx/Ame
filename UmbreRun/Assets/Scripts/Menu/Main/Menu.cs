using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public GameObject Home;
    public GameObject Option;
    public GameObject Credits;

    public Thunder thunderEffect;

    GameObject currentPanel;

    private void Start()
    {
        ShowPanel("Home");
    }

    public void ShowPanel (string name) {
        thunderEffect.Play();
        DeactivateCurrent();
        currentPanel = GetPanel(name);
        Invoke("Activate", 0.5f);
    }

    public void LoadGame()
    {
        thunderEffect.Play();
        DeactivateCurrent();
        Invoke("StartGame", 0.5f);
    }

    void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Update is called once per frame
    void DeactivateCurrent() {
        if(currentPanel == null)
            return;

        currentPanel.SetActive(false);
        currentPanel = null;
    }

    void Activate()
    {
        currentPanel.SetActive(true);
    }

    GameObject GetPanel(string name)
    {
        GameObject panel;
        switch (name)
        {
            case "Option":
                panel = Option;
                break;
            case "Credits":
                panel = Credits;
                break;
            case "Home":
            default:
                panel = Home;
                break;
        }
        return panel;
    }
}

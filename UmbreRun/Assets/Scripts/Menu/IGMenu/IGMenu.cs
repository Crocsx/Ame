using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IGMenu : MonoBehaviour
{
    private static IGMenu m_instance = null;
    public static IGMenu Instance
    {
        get
        {
            if (m_instance)
                return m_instance;
            return null;
        }
    }

    public GameObject pStart;
    public GameObject pPause;
    public GameObject pUI;
    public GameObject pEnd;

    public AudioSource gameSound;
    GameObject currentPanel;

    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else if (m_instance != this)
        {
            Debug.LogWarning("IGMenu.Awake() - instance already exists!");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameSound.pitch = 0.3f;
        ShowPanel("Start");
    }

    private void OnDestroy()
    {
        m_instance = null;
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
        gameSound.pitch = 1f;
        GameManager.Instance.SetNewState(GameManager.GameState.StartGame);
        gameSound.Play();
    }

    public void ReloadStage(string name)
    {
        DeactivateCurrent();
        GameManager.Instance.SetNewState(GameManager.GameState.Menu);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseStage()
    {
        gameSound.pitch = 0.3f;
        GameManager.Instance.SetNewState(GameManager.GameState.Pause);
    }

    public void SetupEnd()
    {
        GameManager.Instance.SetNewState(GameManager.GameState.Menu);
        pEnd.GetComponent<EndScreen>().Setup();
    }

    public void UnPause()
    {
        gameSound.pitch = 1f;
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

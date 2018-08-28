using UnityEngine;
using UnityEngine.SceneManagement;

public class BootMgr : MonoBehaviour
{
    [SerializeField]
    GameObject GameMgr;

    [SerializeField]
    GameObject SoundMgr;

    void Start ()
    {
        Instantiate(GameMgr);
        Instantiate(SoundMgr);

        SceneManager.LoadScene("Menu");
    }
}
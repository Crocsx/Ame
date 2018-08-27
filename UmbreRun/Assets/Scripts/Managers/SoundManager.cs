using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    private static SoundManager s_instance = null;
    public static SoundManager Instance
    {
        get
        {
            if (s_instance)
                return s_instance;
            return null;
        }
    }

    public float volume { set { AudioListener.volume = value; } get { return AudioListener.volume; } }

    private void Awake()
    {
        if (s_instance == null)
            s_instance = this;
        else if (s_instance != this)
        {
            Debug.LogWarning("SoundManager.Awake() - instance already exists!");
            Destroy(gameObject);
        }
    }

    public void Unmute(float value)
    {
        AudioListener.volume = value;
    }

    public void Unmute()
    {
        AudioListener.volume = 1;
    }
    public void Mute()
    {
        AudioListener.volume = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOption : MonoBehaviour {

    public Slider soundSlider;
    public Button SoundButton;

    public Sprite UnMuteSprite;
    public Sprite MuteSprite;
    // Use this for initialization
    void Start () {
        soundSlider.onValueChanged.AddListener(delegate { OnSoundValueChange(); });
        soundSlider.value = SoundManager.Instance.volume;
        SetButtonSprite();
    }
	
    void SetButtonSprite()
    {
        if (SoundManager.Instance.volume > 0)
            SoundButton.GetComponent<Image>().sprite = UnMuteSprite;
        else
            SoundButton.GetComponent<Image>().sprite = MuteSprite;
    }

    void OnSoundValueChange()
    {
        SoundManager.Instance.volume = soundSlider.value;
        SetButtonSprite();
    }

    void Unmute()
    {
        SoundManager.Instance.Unmute(soundSlider.value);
        SetButtonSprite();
    }

	void Mute () {
        SoundManager.Instance.Mute();
        SetButtonSprite();
    }

    public void ToggleSound()
    {
        if (SoundManager.Instance.volume > 0)
            Mute();
        else
            Unmute();

        soundSlider.value = SoundManager.Instance.volume;
    }
}

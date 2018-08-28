using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

    public AudioClip[] StepSound;
    public AudioClip JumpSound;
    public AudioClip JumpBack;
    public AudioClip CrouchSound;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayJumpSound()
    {
        audioSource.clip = JumpSound;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayCrouchSound()
    {
        audioSource.clip = CrouchSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayJumpBackSound()
    {
        audioSource.clip = JumpBack;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayStepSound()
    {
        audioSource.clip = StepSound[Random.Range(0, StepSound.Length - 1)];
        audioSource.loop = false;
        audioSource.Play();
    }
}

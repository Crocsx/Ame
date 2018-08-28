using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour {

    public AudioClip[] thunderSound;
    public Material[] thunderMaterial;

	public void Play () {
        GetComponent<ParticleSystemRenderer>().material = thunderMaterial[Random.Range(0, thunderMaterial.Length - 1)];
        GetComponent<AudioSource>().clip = thunderSound[Random.Range(0, thunderSound.Length - 1)];
        GetComponent<AudioSource>().Play();
        transform.GetComponent<ParticleSystem>().Play();
    }
}

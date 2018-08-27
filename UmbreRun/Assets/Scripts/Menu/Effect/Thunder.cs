using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour {

    public Material[] thunderMaterial;

	public void Play () {
        GetComponent<ParticleSystemRenderer>().material = thunderMaterial[Random.Range(0, thunderMaterial.Length - 1)];
        transform.GetComponent<ParticleSystem>().Play();
    }
}

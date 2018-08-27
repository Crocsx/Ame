using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour {

    public Material[] thunderMaterial;
    ParticleSystemRenderer pSystem;
	// Use this for initialization
	void Start () {
        pSystem = GetComponent<ParticleSystemRenderer>();
    }
	
	// Update is called once per frame
	void PlayThunder () {
        pSystem.material = thunderMaterial[Random.Range(0, thunderMaterial.Length - 1)];
    }
}

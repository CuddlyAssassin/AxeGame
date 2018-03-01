using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<AudioSource>().Play();
        Destroy(gameObject, 4f);
	}
	
}

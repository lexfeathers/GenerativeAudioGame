﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public Hv_Enemies_AudioLib HeavyScript;
	public float highpassFreq;
	public float lowpassFreq;
	public float distlevel = 0f;
	public GameObject parent;

	// Use this for initialization
	void Start () {

		//HeavyScript2.distortionCrush = distlevel;

	}
	
	// Update is called once per frame
	void Update () {
	
	}





	void OnTriggerEnter( Collider other){
		Destroy (this.gameObject);
		//distlevel += 0.1f;
	}


}


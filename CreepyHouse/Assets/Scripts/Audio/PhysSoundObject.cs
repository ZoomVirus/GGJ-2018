﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysSoundObject : MonoBehaviour {
	Rigidbody ownRigidbody;
	float timeSinceEmit = 0f;
	public float timeBetweenEmits;


	// Use this for initialization
	void Start () {
		ownRigidbody = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceEmit += Time.deltaTime;
	}

	void OnCollisionEnter(Collision colision){
		float velocityMagnitude = colision.relativeVelocity.magnitude;
		if (velocityMagnitude > 0.5 && timeSinceEmit > timeBetweenEmits) {
			this.timeSinceEmit = 0f;
			float speed = Mathf.Clamp (velocityMagnitude * 2f, 4, 12);
			float falloff = Mathf.Clamp (velocityMagnitude / 3, 0.5f, 5) ;
			EmitManager.Instance.Emit (colision.contacts [0].point, speed, falloff, 1f);
		}
	}

	void OnCollisionStay(Collision colision){
		float velocityMagnitude = colision.relativeVelocity.magnitude;
		if (velocityMagnitude > 0.5 && timeSinceEmit > timeBetweenEmits) {
			this.timeSinceEmit = 0f;
			float speed = Mathf.Clamp (velocityMagnitude * 1.5f, 4, 12);
			float falloff = Mathf.Clamp (velocityMagnitude / 3, 1.5f, 5) ;
			EmitManager.Instance.Emit (colision.contacts [0].point, speed, falloff, 1f);
		}
	}
}
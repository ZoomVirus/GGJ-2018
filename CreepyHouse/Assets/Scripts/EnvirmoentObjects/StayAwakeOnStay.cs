using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAwakeOnStay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionStay(Collision collision){
		collision.collider.attachedRigidbody.WakeUp ();
	}
}

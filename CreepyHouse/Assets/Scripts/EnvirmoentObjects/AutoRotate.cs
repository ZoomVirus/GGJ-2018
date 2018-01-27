using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour {
	public Vector3 rotationPerSecond;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.rotation = Quaternion.Euler (rotationPerSecond * Time.deltaTime) * this.transform.rotation;
	}
}

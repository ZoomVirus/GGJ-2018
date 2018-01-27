using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMovable : MonoBehaviour {
	Vector3 initalPos;
	Vector3 initalRot;
	public enum interpolationFunc {linear, sin, sinSquared};

	public Vector3 secondPos;
	public Vector3 secondRot;
	public interpolationFunc function;
	public bool state;
	public float time;

	float key = 0f;


	// Use this for initialization
	void Start () {
		initalPos = this.transform.position;
		initalRot = this.transform.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		if (state) {
			key = Mathf.Clamp01 (key + Time.deltaTime / time);
		} else {
			key = Mathf.Clamp01 (key - Time.deltaTime / time);
		}


		float keyPostFunc = key;
		switch (function) {
		case interpolationFunc.linear:
			keyPostFunc = key;
			break;
		case interpolationFunc.sin:
			keyPostFunc = Mathf.Sin (key * Mathf.PI / 2);
			break;
		case interpolationFunc.sinSquared:
			keyPostFunc = Mathf.Pow (Mathf.Sin (key * Mathf.PI / 2), 2);
			break;
		}

		this.transform.position = Vector3.Lerp (initalPos, initalPos + secondPos, keyPostFunc);
		this.transform.rotation = Quaternion.Euler(Vector3.Lerp(initalRot, initalRot + secondRot, keyPostFunc));
		
	}
}

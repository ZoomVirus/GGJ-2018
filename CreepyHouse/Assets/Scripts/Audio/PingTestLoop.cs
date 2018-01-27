using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingTestLoop : MonoBehaviour {
	public float speed;
	public float falloff;
	public float width;
	public float delay;
	float timeSinceEmit = 0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceEmit += Time.deltaTime;
		if (timeSinceEmit > delay) {
			timeSinceEmit -= delay;
			EmitManager.Instance.Emit (this.transform.position, speed, falloff, width);
		}
	}
}

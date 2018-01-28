using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSetLoop : MonoBehaviour {
	public float delay;
	float timeSinceSound = 0f;

	public List<AudioClip> sounds;
	AudioSource source;

	public float speed;
	public float falloff;
	public float width;
	int lastPlayed = 1;

	// Use this for initialization
	void Start () {
		source = this.GetComponent<AudioSource> ();
		source.spatialize = true;
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceSound += Time.deltaTime;
		if (timeSinceSound >= delay && sounds.Count >= 1) {
			timeSinceSound -= delay;
			//avoid repeating the same sound imedietely
			int toPlay = Random.Range (0, sounds.Count - 2);
			if (toPlay >= lastPlayed) {// less than or equal to maintain randomness
				toPlay++;
			}
			lastPlayed = toPlay;
			source.clip = sounds [toPlay];
			source.Play ();
			EmitManager.Instance.Emit (this.transform.position, speed, falloff, width);
		}
	}
}

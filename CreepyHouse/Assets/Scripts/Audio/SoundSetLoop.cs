using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSetLoop : SoundObject {
	public float delay;
	float timeSinceSound = 0f;

	public List<AudioClip> sounds;
	AudioSource source;

    public bool on = true;
	int lastPlayed = 1;

	// Use this for initialization
	override protected void Start ()
    {
        m_source = GetComponent<AudioSource>();
        source = this.GetComponent<AudioSource> ();
		source.spatialize = true;
        m_loudness = source.volume;
	}
	
	// Update is called once per frame
	void Update () {
        if (on)
        {
            timeSinceSound += Time.deltaTime;
            if (timeSinceSound >= delay && sounds.Count >= 1)
            {
                timeSinceSound -= delay;
                //avoid repeating the same sound imedietely
                int toPlay = Random.Range(0, sounds.Count - 2);
                if (toPlay >= lastPlayed)
                {// less than or equal to maintain randomness
                    toPlay++;
                }
                lastPlayed = toPlay;
                source.clip = sounds[toPlay];
                source.Play();
                PingRequest();
            }
        }
	}
}

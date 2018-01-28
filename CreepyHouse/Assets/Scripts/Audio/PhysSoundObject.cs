using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This automatically adds a rigidbody when you add this component - massive timesaver.
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PhysSoundObject : SoundObject
{
	Rigidbody ownRigidbody;
	AudioSource audioSource;
    float timeSinceEmit = 0f;
	public float timeBetweenEmits;


	// Use this for initialization
	protected override void Start()
    {
        m_source = GetComponent<AudioSource>();
        ownRigidbody = this.GetComponent<Rigidbody> ();
        audioSource = GetComponent<AudioSource>();
        m_loudness = audioSource.volume;
	}
	
	// Update is called once per frame
	void Update ()
    {
		timeSinceEmit += Time.deltaTime;
	}

	void OnCollisionEnter(Collision colision){
		float velocityMagnitude = colision.relativeVelocity.magnitude;
		if (velocityMagnitude > 0.5 && timeSinceEmit > timeBetweenEmits) {
			this.timeSinceEmit = 0f;
			m_pulseSpeed = Mathf.Clamp (velocityMagnitude * 2f, 4, 12);
			m_pulseDistance = Mathf.Clamp (velocityMagnitude / 3, 0.5f, 5) ;
            PingRequest();
            audioSource.Play();
        }
	}

	void OnCollisionStay(Collision colision){
		float velocityMagnitude = colision.relativeVelocity.magnitude;
		if (velocityMagnitude > 0.5 && timeSinceEmit > timeBetweenEmits) {
			this.timeSinceEmit = 0f;
            m_pulseSpeed = Mathf.Clamp(velocityMagnitude * 2f, 4, 12);
            m_pulseDistance = Mathf.Clamp(velocityMagnitude / 3, 0.5f, 5);
            PingRequest();
            audioSource.Play();
        }
	}
}

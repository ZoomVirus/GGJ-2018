using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This automatically adds a rigidbody when you add this component - massive timesaver.
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PhysSoundObject : SoundObject
{
	AudioSource audioSource;
    float timeSinceEmit = 0f;
	public float timeBetweenEmits;

    public float minSpeed = 2;
    public float maxSpeed = 4;
    public float minDistance = 1;
    public float maxDistance = 3;


    // Use this for initialization
    protected override void Start()
    {
        m_source = GetComponent<AudioSource>();
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
            float speed = Mathf.InverseLerp(0f, 10f, Mathf.Clamp(velocityMagnitude, 0, 10));
            m_pulseSpeed = Mathf.Lerp(minSpeed, maxSpeed, speed);
            m_pulseDistance = Mathf.Lerp(minDistance, maxDistance, speed);

            PingRequest();
            audioSource.Play();
        }
	}

	void OnCollisionStay(Collision colision){
		float velocityMagnitude = colision.relativeVelocity.magnitude;
		if (velocityMagnitude > 0.5 && timeSinceEmit > timeBetweenEmits) {
			this.timeSinceEmit = 0f;
            float speed = Mathf.InverseLerp(0f, 10f, Mathf.Clamp(velocityMagnitude, 0, 10));
            m_pulseSpeed = Mathf.Lerp(minSpeed, maxSpeed, speed);
            m_pulseDistance = Mathf.Lerp(minDistance, maxDistance, speed);

            PingRequest();
            audioSource.Play();
        }
	}
}

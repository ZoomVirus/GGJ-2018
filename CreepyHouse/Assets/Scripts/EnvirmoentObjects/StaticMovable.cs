using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] 
public class StaticMovable : Interactable {
	Vector3 initalPos;
	Vector3 initalRot;
	public enum interpolationFunc {linear, sin, sinSquared};

	public Vector3 secondPos;
	public Vector3 secondRot;
	public interpolationFunc function;
	public bool state;
	public float time;

    //Should use this for editor variables you don't want to access anywhere else in code :)
    [SerializeField] private AudioClip m_clip;

    bool m_soundPlayed = false;
    bool m_moving = false;

	float key = 0f;


	// Use this for initialization
	void Start () {
		initalPos = this.transform.position;
		initalRot = this.transform.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		if (state && key < 1)
        {
			key = Mathf.Clamp01 (key + Time.deltaTime / time);
            m_moving = true;
		}
        else if (!state && key > 0)
        {
			key = Mathf.Clamp01 (key - Time.deltaTime / time);
            m_moving = true;
        }
        else
        {
            m_moving = false;
            m_soundPlayed = false;
        }

        if (m_moving && !m_soundPlayed)
        {
            GetComponent<AudioSource>().PlayOneShot(m_clip);
            m_soundPlayed = true;
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

    public override void Interact()
    {
        state = !state;
    }
}

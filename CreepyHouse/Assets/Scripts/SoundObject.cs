using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundObject : MonoBehaviour {

    private AudioSource m_source;
    private float m_loudness;

	// Use this for initialization
	void Start () {
        StartCoroutine(UpdateLoop());		
	}
	
    private IEnumerator UpdateLoop()
    {
        while (enabled)
        {
            if (m_source.isPlaying)
            {
                m_loudness = m_source.volume;
            }
            else
            {
                m_loudness = 0;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    public float GetVolume() { return m_loudness; }
}

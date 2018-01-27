using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundObject : MonoBehaviour {

    [SerializeField] private float m_pingsPerSecond;

    private AudioSource m_source;
    protected float m_loudness;

	// Use this for initialization
	virtual protected void Start () {
        m_source = GetComponent<AudioSource>();
        StartCoroutine(UpdateLoop());		
	}
	
    protected virtual IEnumerator UpdateLoop()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(0.05f);
            if (m_source != null)
            {
                while (m_source.isPlaying)
                {
                    m_loudness = m_source.volume;
                    PingRequest();
                    yield return new WaitForSeconds(m_pingsPerSecond);
                }
                m_loudness = 0;
            }
            else
            {
                m_source = GetComponent<AudioSource>();
            }
        }
    }

    void PingRequest()
    {
        // Position and Loudness provided. Need to convert loudness into range somehow. 
        // TODO: Get nick to confirm how he wants to set that info.
    }

    public float GetVolume() { return m_loudness; }
    public Vector3 GetPosition() { return transform.position; }
}

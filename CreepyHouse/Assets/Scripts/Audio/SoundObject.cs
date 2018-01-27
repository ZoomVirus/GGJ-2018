using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundObject : MonoBehaviour {

    [SerializeField] protected float m_SecondsPerPing = 1f;
    [SerializeField] protected float m_pulseSpeed = 0.3f;
    [SerializeField] protected float m_pulseDistance = 0.5f;
    [SerializeField] protected float m_pulseWidth = 0.7f;
    [SerializeField] private   bool  m_destroyOnAttacked = true;

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
                    yield return new WaitForSeconds(m_SecondsPerPing);
                }
                if (!m_source.loop)
                {
                    m_loudness = 0;
                }
            }
            else
            {
                m_source = GetComponent<AudioSource>();
            }
        }
    }

    protected void PingRequest()
    {
        // Position and Loudness provided. Need to convert loudness into range somehow. 
        // TODO: Get nick to confirm how he wants to set that info.
        Vector3 location = transform.position;
        float speed = m_pulseSpeed;
        float falloff = m_pulseDistance * m_loudness;
        float width = m_pulseWidth;

        EmitManager.Instance.Emit(location, speed, falloff, width);
    }

    public void Attacked()
    {
        if (m_destroyOnAttacked)
        {

            GameObject.Destroy(gameObject);
        }
    }

    public float GetVolume() { return m_loudness; }
    public Vector3 GetPosition() { return transform.position; }
}

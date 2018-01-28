using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundObject : MonoBehaviour {

    [SerializeField] protected float m_SecondsPerPing = 1f;
    [SerializeField] protected float m_pulseSpeed = 2f;
    [SerializeField] protected float m_pulseDistance = 2f;
    [SerializeField] protected float m_pulseWidth = 0.7f;
    [SerializeField] private   bool  m_destroyOnAttacked = true;
    [SerializeField] private AudioClip m_DestroyedSound;


    protected AudioSource m_source;
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

    protected void PingRequest(bool NotifyMonster = true)
    {
        // Position and Loudness provided. Need to convert loudness into range somehow. 
        // TODO: Get nick to confirm how he wants to set that info.
        Vector3 location = transform.position;
        float speed = m_pulseSpeed;
        float falloff = m_pulseDistance * GetLoudness();
        float width = m_pulseWidth;

        EmitManager.Instance.Emit(location, speed, falloff, width);

        if (NotifyMonster && MonsterAI.Get() != null)
        {
            MonsterAI.Get().SoundEmitted(this);
        }
    }

    public virtual void Attacked()
    {
        if (m_destroyOnAttacked)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            for(int i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = false;
            }
            if (m_DestroyedSound != null)
            {
                m_source.clip = m_DestroyedSound;
                m_source.Play();

                PingRequest(false);

                GameObject.Destroy(gameObject, m_DestroyedSound.length);
            }
            else
                GameObject.Destroy(gameObject);

        }
            
        else
        {
            m_source.Stop();
        }
    }

    public float GetVolume() { return m_loudness; }
    public Vector3 GetPosition() { return transform.position; }
    public virtual float GetLoudness() { return m_loudness; }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundObject : SoundObject {

    [SerializeField] private MicIndicator m_micIndicator;
    [SerializeField] private float m_tolerance = 0.1f;

	// Use this for initialization
	protected override void Start () {
		m_micIndicator = Object.FindObjectOfType<MicIndicator>() as MicIndicator;
        //StartCoroutine(UpdateLoop());
        StartCoroutine(UpdateLoopTest());
    }

    IEnumerator PlayerPulseCoroutine()
    {
        yield return new WaitForSeconds(m_pulseSpeed);
        if (m_loudness > m_tolerance)
        {
            PingRequest();
        }
    }

    protected virtual IEnumerator UpdateLoopTest()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(0.05f);
            while (true)
            {
                while (m_loudness > m_tolerance)
                {
                    m_loudness = m_micIndicator.SoundLevel;
                    PingRequest();
                    yield return new WaitForSeconds(m_SecondsPerPing);
                }
                yield return null;
            }
            /*if (m_source != null)
            {
                while (m_source.isPlaying)
                {
                    m_loudness = m_source.volume;
                    PingRequest();
                    yield return new WaitForSeconds(m_SecondsPerPing);
                }
                m_loudness = 0;
            }
            else
            {
                m_source = GetComponent<AudioSource>();
            }*/
        }
    }

    // Update is called once per frame
    void Update () {

        m_loudness = m_micIndicator.SoundLevel;

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundObject : SoundObject {

    [SerializeField] private MicIndicator m_micIndicator;
    [SerializeField] private float m_tolerance = 0.1f;

	// Use this for initialization
	protected override void Start () {
		m_micIndicator = Object.FindObjectOfType<MicIndicator>() as MicIndicator;
    }

    IEnumerator PlayerPulseCoroutine()
    {
        yield return new WaitForSeconds(m_pulseSpeed);
        if (m_loudness > m_tolerance)
        {
            PingRequest();
        }
    }
	
	// Update is called once per frame
	void Update () {

        m_loudness = m_micIndicator.SoundLevel;

	}
}

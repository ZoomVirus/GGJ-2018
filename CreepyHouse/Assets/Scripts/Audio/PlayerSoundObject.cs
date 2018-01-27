using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundObject : SoundObject {

    [SerializeField] private MicIndicator m_micIndicator;

	// Use this for initialization
	protected override void Start () {
		m_micIndicator = Object.FindObjectOfType<MicIndicator>() as MicIndicator;
    }
	
	// Update is called once per frame
	void Update () {

        m_loudness = m_micIndicator.SoundLevel;

	}
}

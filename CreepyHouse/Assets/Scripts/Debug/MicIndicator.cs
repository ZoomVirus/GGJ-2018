using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MicIndicator : MonoBehaviour {

    [SerializeField] [Range(0,1)] private float m_clamp;

    private Image m_decibelMeter;
    private float m_percent;

	// Use this for initialization
	void Start () {
        m_decibelMeter = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {

        m_percent = Mathf.Min(MicInput.MicAverage, m_clamp);

        m_percent = Mathf.Clamp01(m_percent/m_clamp);

        Vector3 scale = m_decibelMeter.transform.localScale;
        scale.x = m_percent;
        m_decibelMeter.transform.localScale = scale;

	}
}

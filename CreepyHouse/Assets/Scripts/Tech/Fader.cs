using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Fader : MonoBehaviour {

    [SerializeField] private float m_fadeTime = 0.3f;
    Image m_image;
    bool m_fading = false;

    private void Start()
    {
        m_image = GetComponent<Image>();
        FadeIn();
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(true));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(false));
    }

    public bool Fading() { return m_fading; }

    IEnumerator Fade(bool fadeIn)
    {
        if (m_fading)
        {
            yield break;
        }
        m_fading = true;

        float time = m_fadeTime;
        Color col = m_image.color;

        while((time -= Time.deltaTime) > 0)
        {
            float s = (time / m_fadeTime);

            s = fadeIn ? s : 1 - s; 

            col.a = s;
            m_image.color = col;

            yield return null;
        }

        col.a = fadeIn ? 0 : 1;
        m_image.color = col;
        m_fading = false;

    }

    //public void ShakeItAllAbout()
    //{
    //}
}

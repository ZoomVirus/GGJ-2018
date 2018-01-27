using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    bool m_levelLoadRequested = false;
    Fader m_fader;

    private void Start()
    {
        m_fader = Object.FindObjectOfType<Fader>();
    }

    public void LoadLevel(string level)
    {
        if (!m_levelLoadRequested)
        {
            StartCoroutine(LoadLevelCoroutine(level));
        }
    }

    IEnumerator LoadLevelCoroutine(string level)
    {
        m_fader.FadeOut();
        while (m_fader.Fading())
        {
            yield return null;
        }
        SceneManager.LoadScene(level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

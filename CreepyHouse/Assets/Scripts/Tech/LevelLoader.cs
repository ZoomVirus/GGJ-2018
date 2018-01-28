using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public Text mic;

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

    public void ToggleMic()
    {
        MicInput.On = !MicInput.On;

        mic.text = "Mic: " + (MicInput.On ? "On" : "Off");
    }

}

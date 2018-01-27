using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    bool m_levelLoadRequested = false;

    public void LoadLevel(string level)
    {
        if (!m_levelLoadRequested)
        {
            StartCoroutine(LoadLevelCoroutine(level));
        }
    }

    IEnumerator LoadLevelCoroutine(string level)
    {
        
        yield return null;
    }

}

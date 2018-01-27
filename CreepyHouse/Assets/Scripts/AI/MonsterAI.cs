using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour {

    SoundObject[] m_sounds;

    void Start()
    {


    }

    void PopulateList()
    {
        //Get every single sound producer in the scene.
        m_sounds = Object.FindObjectsOfType(typeof(SoundObject)) as SoundObject[];
    }
	
	// Update is called once per frame
	void Update () {

        bool shouldRepopulate = false;
        foreach(SoundObject soundObject in m_sounds)
        {
            if (soundObject != null)
            {
                float emitterVolume = soundObject.GetVolume();

                if (emitterVolume > 0.01f)
                {
                    Vector3 position = soundObject.transform.position;
                }
            }
            else
            {
                shouldRepopulate = true;
            }
        }

        if(shouldRepopulate)
        {
            // Shitty GC to keep list free of nulls.
            PopulateList();
        }

	}
}

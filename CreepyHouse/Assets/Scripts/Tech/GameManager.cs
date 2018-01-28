using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private Key[] m_keys;
    [SerializeField] private GameObject[] m_padalocks;
    [SerializeField] private StaticMovable m_exit;

    private int m_locksOpened = 0;

    static GameManager s_instance;

    static public GameManager Get() { return s_instance; }

    // Use this for initialization
    void Start () {

        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        // Set up so we have exactly the same number of keys as padalocks in the level
        int numKeys = Mathf.Min(m_keys.Length, 3);
        int count = 0;
        int bailcounter = 0;
        do
        {
            int key = Random.Range(0, m_keys.Length);
            if (m_keys[key] && !m_keys[key].gameObject.activeInHierarchy)
            {
                m_keys[key].gameObject.SetActive(true);
                ++count;
            }
            ++bailcounter;
        } while (count < numKeys && bailcounter < 500);

        for (int i = numKeys ; i < m_padalocks.Length; ++i)
        {
            m_padalocks[i].SetActive(false);
        }
	}

    public void TriggerLockRelease(Key key)
    {
        if (m_locksOpened < m_padalocks.Length)
        {
            m_padalocks[m_locksOpened++].SetActive(false);

            if (m_locksOpened >= m_padalocks.Length)
            {
                // true == go to second state, false return to 1st.
                m_exit.state = true;
            }

            key.gameObject.SetActive(false);
        }
    } 
	
	// Update is called once per frame
	void Update () {
		
	}
}

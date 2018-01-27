using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.parent == null)
        {
            Key key = null;
            if ((key = other.gameObject.GetComponent<Key>()) != null)
            {
                GameManager.Get().TriggerLockRelease(key);
            }
        }
    }
}

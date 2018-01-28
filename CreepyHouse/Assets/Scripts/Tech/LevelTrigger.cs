using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelLoader))]
public class LevelTrigger : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.parent == null)
        {
            Key key = null;
            if ((key = other.gameObject.GetComponent<Key>()) != null)
            {
                GetComponent<LevelLoader>().LoadLevel("EndGameScene");
            }
        }
    }
}

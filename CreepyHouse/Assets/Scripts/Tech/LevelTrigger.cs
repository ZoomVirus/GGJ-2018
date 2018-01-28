using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelLoader))]
public class LevelTrigger : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.parent == null)
        {
            if ( other.gameObject.GetComponent<PlayerSoundObject>() != null)
            {
                GetComponent<LevelLoader>().LoadLevel("EndGameScene");
            }
        }
    }
}

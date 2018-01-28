using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTowards : MonoBehaviour {

    [SerializeField]
    private GameObject[] m_lookObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach (GameObject obj in m_lookObject)
        {
            if (obj.activeInHierarchy)
            {
                Vector3 pos = obj.transform.position;
                pos.y = transform.position.y;
                transform.forward = (pos - transform.position).normalized;
                break;
            }
        } 
	}
}

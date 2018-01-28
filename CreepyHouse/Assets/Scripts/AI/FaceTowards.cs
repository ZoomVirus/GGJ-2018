using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTowards : MonoBehaviour {

    [SerializeField]
    private GameObject m_lookObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (m_lookObject != null)
        {
            Vector3 pos = m_lookObject.transform.position;
            pos.y = transform.position.y;
            transform.forward = (pos - transform.position).normalized;
        } 
	}
}

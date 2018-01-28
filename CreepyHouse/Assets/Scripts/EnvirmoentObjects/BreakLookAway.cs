using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakLookAway : MonoBehaviour
{
    public Renderer mesh;

    GameObject intact;
    GameObject broken;
    Camera cam;
    float timeSinceStart = 0f;
    public float minTime;
    public float minLookAwayTime;
    float lookingAwayTime;
    public float lookingAwayAngle;

    // Use this for initialization
    void Start()
    {
        intact = this.transform.Find("Intact").gameObject;
        broken = this.transform.Find("Broken").gameObject;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); ;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceStart += Time.deltaTime;
        //if (lookingAwayAngle < Vector3.Angle(cam.transform.forward, this.transform.position - cam.transform.position))
        //{
        //    lookingAwayTime += Time.deltaTime;
        //}
        if (!mesh.isVisible)
        {
            lookingAwayTime += Time.deltaTime;
        }
        else
        {
            lookingAwayTime = 0f;
        }

        if (timeSinceStart > minTime && lookingAwayTime > minLookAwayTime)
        {
            SetBroken();
        }
    }

    public void SetBroken(bool fix = false)
    {
        intact.SetActive(fix);
        broken.SetActive(!fix);
        GlobalSettings.AllowedToMove = true;
    }
}

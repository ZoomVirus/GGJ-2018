using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BreakLookAway : MonoBehaviour
{
    public Renderer mesh;

    GameObject intact;
    GameObject broken;

    float timeSinceStart = 0f;
    public float minTime;
    public float minLookAwayTime;
    float lookingAwayTime;
    public float lookingAwayAngle;

    public AudioClip m_breakingSound;

    // Use this for initialization
    void Start()
    {
        intact = this.transform.Find("Intact").gameObject;
        broken = this.transform.Find("Broken").gameObject;
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
        if (fix != intact.activeInHierarchy)
        {
            intact.SetActive(fix);
            broken.SetActive(!fix);
            GlobalSettings.AllowedToMove = true;
            GetComponent<AudioSource>().loop = false;
            GetComponent<AudioSource>().clip = m_breakingSound;
            GetComponent<AudioSource>().Play();
        }
    }
}

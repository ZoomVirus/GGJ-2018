using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    //with oculus still need all these inouts due to users hardware options
    // Use this for initialization
    public float rayCastSize;
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    private AudioSource m_AudioSource;
    public DateTime lastAudioPlay;
    public TimeSpan WalkingFoortStepsTiming = new TimeSpan(0, 0, 0, 0, 500);
    bool previousStepWas0 = false;
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float ratioMultiplyer = 1;
        if (GlobalSettings.RiftContoller)
        {
            ratioMultiplyer = GlobalSettings.riftControllerToKeyboardRatioTranslation;
        }
        else if (GlobalSettings.XboxContoller)
        {
            ratioMultiplyer = GlobalSettings.xboxControllerToKeyboardRatioTranslation;
        }
        Vector3 translate = new Vector3(ratioMultiplyer * Input.GetAxisRaw("Horizontal") * GlobalSettings.translateSpeed * Time.deltaTime, 0, ratioMultiplyer * Input.GetAxisRaw("Forward") * GlobalSettings.translateSpeed * Time.deltaTime);
        RaycastHit hit;
        if (translate != new Vector3(0, 0, 0))
        {
            previousStepWas0 = false;
            if (lastAudioPlay.Add(WalkingFoortStepsTiming) < DateTime.Now)
            {
                lastAudioPlay = DateTime.Now;
                EmitfootStep();
            }
        }
        else
        {
            if (!previousStepWas0)
            {
                previousStepWas0 = true;
                EmitfootStep();
            }
        }
        Vector3 wall = transform.forward;
        wall.y = 0;
        Debug.Log("X::" + Input.GetAxisRaw("Horizontal"));
        Debug.Log("Z::" + Input.GetAxisRaw("Forward"));
        bool noCollison = true;
        if (Input.GetAxisRaw("Forward") > 0)
        {
            if (Physics.Raycast(transform.position, wall, out hit, rayCastSize))
            {
                if (!hit.collider.gameObject.GetComponent("TouchControllerInteract"))
                {
                    noCollison = false;
                }
            }
        }
        if (Input.GetAxisRaw("Forward") < 0)
        {
            if (Physics.Raycast(transform.position, wall * -1, out hit, rayCastSize))
            {
                if (!hit.collider.gameObject.GetComponent("TouchControllerInteract"))
                {
                    noCollison = false;
                }
            }
        }
        wall = transform.right;
        wall.y = 0;
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if (Physics.Raycast(transform.position, wall, out hit, rayCastSize))
            {
                if (!hit.collider.gameObject.GetComponent("TouchControllerInteract"))
                {
                    noCollison = false;
                }
            }
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            if (Physics.Raycast(transform.position, wall * -1, out hit, rayCastSize))
            {
                if (!hit.collider.gameObject.GetComponent("TouchControllerInteract"))
                {
                    noCollison = false;
                }
            }
        }

        if (noCollison)
        {
            if (GlobalSettings.AllowedToMove)
            {
                this.transform.Translate(translate);
            }
        }


        if (GlobalSettings.RiftContoller)
        {
            ratioMultiplyer = GlobalSettings.riftControllerToMouseRatioTranslation;
            this.gameObject.transform.Rotate(Vector3.up, Input.GetAxisRaw("HorizontalRotation") * ratioMultiplyer * GlobalSettings.rotateHoriSpeed * Time.deltaTime);
        }
        else if (GlobalSettings.XboxContoller)
        {
            ratioMultiplyer = GlobalSettings.xboxControllerToMouseRatioRotation;
            this.gameObject.transform.Rotate(Vector3.up, Input.GetAxisRaw("HorizontalRotation") * ratioMultiplyer * GlobalSettings.rotateHoriSpeed * Time.deltaTime);
        }
        else
        {
            this.gameObject.transform.Rotate(Vector3.up, Input.GetAxisRaw("Mouse X") * ratioMultiplyer * GlobalSettings.rotateHoriSpeed * Time.deltaTime);
        }
        Quaternion q = gameObject.transform.rotation;
        q.eulerAngles = new Vector3(q.eulerAngles.x, q.eulerAngles.y, 0);
        gameObject.transform.rotation = q;
        Vector3 v = gameObject.transform.position;
        v = new Vector3(v.x, 0.8f, v.z);
        gameObject.transform.position = v;

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("wdhush");
    }


    void EmitfootStep()
    {

        int n = UnityEngine.Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;

        EmitManager.Instance.Emit(new Vector3(transform.position.x, 0, transform.position.z),
            1, 1.5f, 1.5f);
    }
}



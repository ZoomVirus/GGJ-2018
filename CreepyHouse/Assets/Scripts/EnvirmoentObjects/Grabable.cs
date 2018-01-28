using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : Interactable
{
    Vector3 m_initialPosition;
    float m_timer;

    bool heldInLeft = false;
    bool heldInRight = false;

    public bool HeldInLeft
    {
        get { return heldInLeft; }
        set { heldInLeft = value;
            SetGrabState();
        }
    }
    public bool HeldInRight
    {
        get { return heldInRight; }
        set { heldInRight = value;
            SetGrabState();
        }
    }
    Renderer m_Renderer;
    Rigidbody m_RigidBody;
    // Use this for initialization
    void Start()
    {
        m_initialPosition = transform.position;

        m_Renderer = GetComponent<Renderer>();
        if (m_Renderer == null)
        {
            m_Renderer = GetComponentInChildren<Renderer>(true);
        }
        m_RigidBody = GetComponent<Rigidbody>();
        m_Renderer.material = MaterialManager.Instance.m_GrabableDefault;
    }

    void Update()
    {

        if (GetXZDistanceBetween(m_initialPosition, transform.position) > 0.2f)
        {
            if (!heldInLeft && !heldInRight && (m_timer -= Time.deltaTime) <= 0)
            {
                transform.position = m_initialPosition;
                m_timer = 30f;
            }
        }
    }

    void SetGrabState()
    {
        if (heldInLeft || heldInRight)
        {
            m_Renderer.material = MaterialManager.Instance.m_GrabableGrabbed;
            m_RigidBody.isKinematic = true;
        }
        else
        {
            m_Renderer.material = MaterialManager.Instance.m_GrabableDefault;
            m_RigidBody.isKinematic = false;
        }
        m_timer = 30f;
    }

    public override void Interact()
    {
        //StartGrab();
    }

    public void ThrowObject()
    {
        this.transform.Translate(new Vector3(0, 0, GlobalSettings.ForceThrowObject));
        m_timer = 30f;
    }

    float GetXZDistanceBetween(Vector3 a, Vector3 b)
    {
        a.y = 0;
        b.y = 0;
        return (a - b).magnitude;
    }
}

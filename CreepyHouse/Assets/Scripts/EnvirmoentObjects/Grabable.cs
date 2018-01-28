using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : Interactable
{
    Vector3 m_initialPosition;
    float m_timer;

    bool held = false;

    public bool Held
    {
        get { return held; }
        set
        {
            held = value;
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
            if (!held&& (m_timer -= Time.deltaTime) <= 0)
            {
                transform.position = m_initialPosition;
                m_timer = 30f;
            }
        }
    }

    void SetGrabState()
    {
        if (held)
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            m_Renderer.material = MaterialManager.Instance.m_GrabableGrabbed;
            m_RigidBody.isKinematic = true;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            m_Renderer.material = MaterialManager.Instance.m_GrabableDefault;
            m_RigidBody.isKinematic = false;
        }
        m_timer = 30f;
    }

    public override void Interact()
    {
        //StartGrab();
    }

    public void ThrowObject(Vector3 Direction, float force = 350f)
    {
        m_RigidBody.AddForce(Direction * force);
        //this.transform.Translate(new Vector3(0, 0, GlobalSettings.ForceThrowObject));
        m_timer = 30f;
    }

    float GetXZDistanceBetween(Vector3 a, Vector3 b)
    {
        a.y = 0;
        b.y = 0;
        return (a - b).magnitude;
    }
}

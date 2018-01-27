using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : Interactable
{

    public bool heldInLeft = false;
    public bool heldInRight = false;

    Renderer m_Renderer;
    // Use this for initialization
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        m_Renderer.material = MaterialManager.Instance.m_GrabableDefault;
    }
    
    void StartGrab()
    {
        m_Renderer.material = MaterialManager.Instance.m_GrabableGrabbed;
    }

    public override void Interact()
    {
        StartGrab();
    }

    public void ThrowObject()
    {
        this.transform.Translate(new Vector3(0, 0, GlobalSettings.ForceThrowObject));
    }
}

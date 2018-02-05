using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    protected virtual void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
        if (m_Renderer == null)
        {
            m_Renderer = GetComponentInChildren<Renderer>(true);
        }
    }
    public abstract void Interact();
    protected Renderer m_Renderer;
}

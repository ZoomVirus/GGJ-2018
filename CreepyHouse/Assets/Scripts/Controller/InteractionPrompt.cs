using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPrompt : MonoBehaviour
{
    static InteractionPrompt m_Instance;
    static public InteractionPrompt Instance
    {
        get { return m_Instance; }
    }

    Renderer m_Renderer;
    private void Start()
    {
        m_Instance = this;
        m_Renderer = GetComponent<Renderer>();
    }
    public bool PromptActive
    {
        set { m_Renderer.enabled = value; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    static MaterialManager m_Instance;
    public static MaterialManager Instance
    {
        get { return m_Instance; }
    }
    private void Awake()
    {
        m_Instance = this;
    }
    public Material m_GrabableDefault;
    public Material m_GrabableGrabbed;
    public Material m_StaticMovableDefault;

}

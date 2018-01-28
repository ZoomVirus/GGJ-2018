using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    EventSystem m_EventSystem;
    public Button[] m_Buttons; 
    private void Start()
    {
        m_EventSystem = GetComponent<EventSystem>();
    }
    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (m_EventSystem.currentSelectedGameObject == null)
        {
            float forward = Input.GetAxis("Forward");
            if (forward  > 0.1f)
                m_EventSystem.SetSelectedGameObject(m_Buttons[m_Buttons.Length - 1].gameObject);
            else if (forward < -0.1f)
                m_EventSystem.SetSelectedGameObject(m_Buttons[0].gameObject);
        }
    }
}

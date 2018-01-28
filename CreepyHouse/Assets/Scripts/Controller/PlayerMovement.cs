using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    //with oculus still need all these inouts due to users hardware options
    // Use this for initialization
    public float rayCastSize;

    void Start()
    {
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
            this.transform.Translate(translate);
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
        //   this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Debug.Log("wdhush");
    }
}



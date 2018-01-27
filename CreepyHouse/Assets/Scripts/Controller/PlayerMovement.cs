using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public float translateSpeed;
    public float rotateHoriSpeed;
    public float rotateVertSpeed;
    public bool XboxContoller;
    public bool RiftContoller;
    public float xboxControllerToKeyboardRatioTranslation;
    public float xboxControllerToMouseRatioRotation;
    public float riftControllerToKeyboardRatioTranslation;
    Quaternion previousAngles;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float ratioMultiplyer = 1;
        if (RiftContoller)
        {
            ratioMultiplyer = riftControllerToKeyboardRatioTranslation;
        }
        else if (XboxContoller)
        {
            ratioMultiplyer = xboxControllerToKeyboardRatioTranslation;
        }
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Forward");
        Vector3 translate = new Vector3(ratioMultiplyer * Input.GetAxisRaw("Horizontal") * translateSpeed * Time.deltaTime, 0, ratioMultiplyer * Input.GetAxisRaw("Forward") * translateSpeed * Time.deltaTime);
        this.gameObject.transform.Translate(translate);

        if (RiftContoller)
        {
            ratioMultiplyer = riftControllerToKeyboardRatioTranslation;
            this.gameObject.transform.Rotate(Vector3.up, Input.GetAxisRaw("HorizontalRotation") * ratioMultiplyer * rotateHoriSpeed * Time.deltaTime);
        }
        else if (XboxContoller)
        {
            ratioMultiplyer = xboxControllerToMouseRatioRotation;
            this.gameObject.transform.Rotate(Vector3.up, Input.GetAxisRaw("HorizontalRotation") * ratioMultiplyer * rotateHoriSpeed * Time.deltaTime);
        }
        else
        {
            this.gameObject.transform.Rotate(Vector3.up, Input.GetAxisRaw("Mouse X") * ratioMultiplyer * rotateHoriSpeed * Time.deltaTime);
        }
        Quaternion q = gameObject.transform.rotation;
        q.eulerAngles = new Vector3(q.eulerAngles.x, q.eulerAngles.y, 0);
        gameObject.transform.rotation = q;
        Vector3 v = gameObject.transform.position;
        v = new Vector3(v.x, 0, v.z);
        gameObject.transform.position = v;

    }
}



﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
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
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Forward");
        Vector3 translate = new Vector3(ratioMultiplyer * Input.GetAxisRaw("Horizontal") * GlobalSettings.translateSpeed * Time.deltaTime, 0, ratioMultiplyer * Input.GetAxisRaw("Forward") * GlobalSettings.translateSpeed * Time.deltaTime);
        this.gameObject.transform.Translate(translate);

        if (GlobalSettings.RiftContoller)
        {
            ratioMultiplyer = GlobalSettings.riftControllerToKeyboardRatioTranslation;
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
        v = new Vector3(v.x, 0, v.z);
        gameObject.transform.position = v;

    }
}



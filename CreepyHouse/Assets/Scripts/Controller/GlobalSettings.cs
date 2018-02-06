using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public static float translateSpeed = 10;
    public static float rotateHoriSpeed = 10.0f;
    public static float rotateVertSpeed = -10.0f;
    public static bool XboxContoller = false;
    public static bool RiftContoller = false;
    public static float xboxControllerToKeyboardRatioTranslation = 10;
    public static float xboxControllerToMouseRatioRotation = 20;
    public static float riftControllerToKeyboardRatioTranslation = 0.05f;
    public static float riftControllerToMouseRatioTranslation = 2f;
    public static float ForceThrowObject = 2;
    public static bool AllowedToMove = true;
    // Use this for initialization
    void Start()
    {
        var vrscript = this.gameObject.GetComponent<OVRCameraRig>();

        if (RiftContoller && vrscript == null)
        {
            this.gameObject.SetActive(false);
        }
        if (!RiftContoller && vrscript != null)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public static float translateSpeed = 20;
    public static float rotateHoriSpeed = 30.0f;
    public static float rotateVertSpeed = -10.0f;
    public static bool XboxContoller = false;
    public static bool RiftContoller = true;
    public static float xboxControllerToKeyboardRatioTranslation = 10;
    public static float xboxControllerToMouseRatioRotation = 20;
    public static float riftControllerToKeyboardRatioTranslation = 0.1f;
    public static float riftControllerToMouseRatioTranslation = 1f;
    public static float ForceThrowObject = 2;
    public static bool AllowedToMove = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

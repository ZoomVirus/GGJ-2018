﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GlobalSettings.AllowedToMove = false;
        GlobalSettings.RiftContoller = UnityEngine.XR.XRSettings.enabled;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

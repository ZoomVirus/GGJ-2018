﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour
{

    public bool heldInLeft = false;
    public bool heldInRight = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ThrowObject()
    {
        this.transform.Translate(new Vector3(0, 0, GlobalSettings.ForceThrowObject));
    }
}

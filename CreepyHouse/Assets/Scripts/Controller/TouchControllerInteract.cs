using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControllerInteract : MonoBehaviour
{
    public bool HoldingItem = false;
    public bool LeftHand;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var leftTrigger = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        var leftShoulder = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);

    }

    void OnTriggerEnter(Collider other)
    {
        if (!HoldingItem)
        {
            var GrabAbleScript = other.gameObject.GetComponent("Grabable");
            if (GrabAbleScript != null)
            {
                var holdableItem = GrabAbleScript as Grabable;
                if (!holdableItem.heldInRight && !holdableItem.heldInLeft)
                {
                    holdableItem.heldInLeft = true;
                    HoldingItem = true;
                    other.gameObject.transform.SetParent(gameObject.transform);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControllerInteract : MonoBehaviour
{
    public bool HoldingItem = false;
    public bool LeftHand = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (HoldingItem)
        {
            bool sideTrigger = false;
            if (LeftHand)
            {
                sideTrigger = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
                if (!sideTrigger)
                {
                    sideTrigger = Input.GetAxis("xboxTriggers") > 0;
                }
            }
            else
            {
                sideTrigger = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
                if (!sideTrigger)
                {
                    sideTrigger = Input.GetAxis("xboxTriggers") < 0;
                }
            }

            if (!sideTrigger)
            {
                var heldItem = this.gameObject.transform.GetChild(0).gameObject;
                var heldItemScript = heldItem.gameObject.GetComponent("Grabable") as Grabable;
                if (LeftHand)
                {
                    heldItemScript.heldInLeft = false;
                }
                else
                {
                    heldItemScript.heldInRight = false;
                }

                HoldingItem = false;
                heldItem.transform.parent = null;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PickUpItem(other);
    }

    private void OnTriggerStay(Collider other)
    {
        PickUpItem(other);
    }

    void PickUpItem(Collider other)
    {
        var GrabAbleScript = other.gameObject.GetComponent("Grabable");
        if (GrabAbleScript != null)
        {
            if (!HoldingItem)
            {
                bool sideTrigger = false;
                if (LeftHand)
                {
                    sideTrigger = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
                    if (!sideTrigger)
                    {
                        if (!sideTrigger)
                        {
                            sideTrigger = Input.GetAxis("xboxTriggers") > 0;
                        }
                    }
                }
                else
                {
                    sideTrigger = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
                    if (!sideTrigger)
                    {
                        sideTrigger = Input.GetAxis("xboxTriggers") < 0;
                    }
                }

                if (sideTrigger)
                {
                    var holdableItem = GrabAbleScript as Grabable;
                    if (!holdableItem.heldInRight && !holdableItem.heldInLeft)
                    {
                        if (LeftHand)
                        {
                            holdableItem.heldInLeft = true;
                        }
                        else
                        {
                            holdableItem.heldInRight = true;
                        }
                        HoldingItem = true;
                        other.gameObject.transform.SetParent(gameObject.transform);
                    }
                }
            }
        }
    }
}

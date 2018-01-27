using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastGrab : MonoBehaviour
{
    public bool LeftHand;
    public bool HoldingItem;
    public Vector3 RayCastValuesForPick;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, RayCastValuesForPick, out hit))
            {
                PickUpItem(hit.collider);
            }
        }
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
                        sideTrigger = Input.GetAxis("xboxTriggers") > 0;
                        if (!sideTrigger)
                        {
                            sideTrigger = Input.GetMouseButtonDown(0);
                        }
                    }
                }
                else
                {
                    sideTrigger = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
                    if (!sideTrigger)
                    {
                        sideTrigger = Input.GetAxis("xboxTriggers") < 0;
                        if (!sideTrigger)
                        {
                            sideTrigger = Input.GetMouseButtonDown(1);
                        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControllerInteract : MonoBehaviour
{
    public bool HoldingItem = false;
    public bool LeftHand = true;
    public Vector3 RayCastValuesForPick;
    // Use this for initialization
    void Start()
    {
        if (!GlobalSettings.RiftContoller)
        {
            //   this.gameObject.GetComponent<Renderer>().enabled = false;           
        }
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
                    sideTrigger = Input.GetAxis("xboxTriggers") > 0.1;
                    if (!sideTrigger)
                    {
                        sideTrigger = Input.GetMouseButton(0);
                    }
                }
            }
            else
            {
                sideTrigger = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
                if (!sideTrigger)
                {
                    sideTrigger = Input.GetAxis("xboxTriggers") < -0.1;
                    if (!sideTrigger)
                    {
                        sideTrigger = Input.GetMouseButton(1);
                    }
                }
            }


            if (!sideTrigger)
            {
                DropItem();
            }

            if (LeftHand)
            {
                if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
                {
                    DropItem(true);
                }
                else if ((Input.GetKeyDown("q")))
                {
                    DropItem(true);
                }
                else if (Input.GetButtonDown("LeftXboxBumper"))
                {
                    DropItem(true);
                }
            }
            else
            {
                if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
                {
                    DropItem(true);
                }
                else if ((Input.GetKeyDown("e")))
                {
                    DropItem(true);
                }
                else if (Input.GetButtonDown("RightsXboxBumper"))
                {
                    DropItem(true);
                }
            }
        }

        if (OVRInput.Get(OVRInput.RawButton.B, OVRInput.Controller.RTouch))
        {
            EmitSound();
        }
        else if (OVRInput.Get(OVRInput.RawButton.Y, OVRInput.Controller.LTouch))
        {
            EmitSound();
        }
        else if ((Input.GetKeyDown("r")))
        {
            EmitSound();
        }
    }

    void EmitSound()
    {
        EmitManager.Instance.Emit(this.transform.position);
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
                        holdableItem.GetComponent<Rigidbody>().isKinematic = true;
                        other.gameObject.transform.SetParent(gameObject.transform);
                    }
                }
            }
        }
    }

    void DropItem(bool throwObject = false)
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
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        if (throwObject)
        {
            heldItemScript.ThrowObject();
        }
    }
}

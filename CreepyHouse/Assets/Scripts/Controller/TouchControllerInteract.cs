using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class TouchControllerInteract : MonoBehaviour

{

    public static bool HoldingItem = false;

    public bool LeftHand = true;

    public Vector3 RayCastValuesForPick;
    public Transform Camera;

    // Use this for initialization

    void Start()

    {

        if (!GlobalSettings.RiftContoller)

        {

            this.gameObject.GetComponent<Renderer>().enabled = false;

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

        else if (Input.GetButtonDown("BYXbox EmitSound"))

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
        Interact(other);
    }

    private void OnTriggerStay(Collider other)
    {
        Interact(other);
    }

    void Interact(Collider other)
    {
        var InteractableScript = other.gameObject.GetComponent<Interactable>();
        if (InteractableScript != null)
        {
            InteractableScript.Interact();
            if (InteractableScript is Grabable && !HoldingItem)
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
                    var holdableItem = InteractableScript as Grabable;
                    if (!holdableItem.Held)
                    {

                        holdableItem.Held = true;


                        HoldingItem = true;

                        //holdableItem.GetComponent<Rigidbody>().isKinematic = true;

                        other.gameObject.transform.SetParent(gameObject.transform);
                        if (GlobalSettings.RiftContoller)
                        {
                            this.gameObject.GetComponent<Renderer>().enabled = false;
                        }
                        //       interactableScript.Action();

                    }

                }

            }

        }

    }

    void Interacte(Collider other)

    {



        var interactableScript = other.gameObject.GetComponent("Grabable");

        if (interactableScript != null)

        {

            if (OVRInput.Get(OVRInput.RawButton.A, OVRInput.Controller.RTouch))

            {

                //    interactableScript.Action();

            }

            else if (OVRInput.Get(OVRInput.RawButton.X, OVRInput.Controller.LTouch))

            {

                //    interactableScript.Action();

            }

            else if ((Input.GetKeyDown("f")))

            {

                //     interactableScript.Action();

            }

            else if (Input.GetButtonDown("AXXboxInteract"))

            {

                //   interactableScript.Action();

            }

            else

            {

                Interact(other);

            }

        }

    }

    void DropItem(bool throwObject = false)

    {

        var heldItem = this.gameObject.transform.GetChild(0).gameObject;

        var heldItemScript = heldItem.gameObject.GetComponent("Grabable") as Grabable;

        heldItemScript.Held = false;

        HoldingItem = false;

        heldItem.transform.parent = null;


        if (throwObject)

        {
            if (Camera != null)
                heldItemScript.ThrowObject(Camera.forward);

        }
        if (GlobalSettings.RiftContoller)
        {
            this.gameObject.GetComponent<Renderer>().enabled = true;
        }
    }
}



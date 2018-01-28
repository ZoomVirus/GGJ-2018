using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastGrab : MonoBehaviour
{
    Grabable CurrentlyHeldItem;
    void Start()
    {

    }
    float CurrentxboxTriggers = 0, PreviousxboxTriggers = 0;
    bool Prompt = false;
    // Update is called once per frame
    void Update()
    {
        Prompt = false;
        CurrentxboxTriggers = Input.GetAxis("xboxTriggers");
        if (CurrentlyHeldItem == null)
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {

                var InteractableItem = hit.collider.gameObject.GetComponent<Interactable>();
                if (InteractableItem != null)
                {
                    Prompt = true;
                    if (Input.GetMouseButtonDown(0) || (CurrentxboxTriggers > 0 && PreviousxboxTriggers <= 0) || Input.GetKeyDown("e"))
                    {
                        Interact(InteractableItem);
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) || (CurrentxboxTriggers < -0.1f && PreviousxboxTriggers >= -0.1f) || Input.GetKeyDown("e"))
            {
                DropItem(false);
            }
            else if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("LeftXboxBumper") || Input.GetKeyDown("q"))
            {
                if (CurrentlyHeldItem != null)
                    DropItem(true);
            }
        }
        PreviousxboxTriggers = CurrentxboxTriggers;

        if (InteractionPrompt.Instance != null)
            InteractionPrompt.Instance.PromptActive = Prompt;

        if ((Input.GetKeyDown("r")))
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

    void DropItem(bool Throw)
    {
        CurrentlyHeldItem.transform.parent = null;
        CurrentlyHeldItem.Held = false;
        if (Throw)
            CurrentlyHeldItem.ThrowObject();
        CurrentlyHeldItem = null;
    }

    void Interact(Interactable InteractableItem)
    {
        InteractableItem.Interact();
        if (InteractableItem is Grabable)
        {
            if (CurrentlyHeldItem == null)
            {
                var holdableItem = InteractableItem as Grabable;
                {
                    holdableItem.Held = true;
                    CurrentlyHeldItem = holdableItem;
                    InteractableItem.transform.SetParent(gameObject.transform);
                }
            }
        }
    }
}

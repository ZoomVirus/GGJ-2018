using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float translateSpeed;
    public float rotateHoriSpeed;
    public float rotateVertSpeed;
    public bool Contoller;
    public float controllerToKeyboardRatioTranslation;
    public float controllerToMouseRatioRotation;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float ratioMultiplyer = 1;
        if (Contoller)
        {
            ratioMultiplyer = controllerToKeyboardRatioTranslation;
        }

        Vector3 translate = new Vector3(controllerToKeyboardRatioTranslation * Input.GetAxisRaw("Horizontal") * translateSpeed * Time.deltaTime, 0, ratioMultiplyer * Input.GetAxisRaw("Forward") * translateSpeed * Time.deltaTime);
        this.gameObject.transform.Translate(translate);

        if (Contoller)
        {

            ratioMultiplyer = controllerToMouseRatioRotation;
            this.gameObject.transform.Rotate(Vector3.right, Input.GetAxisRaw("VerticalRotation") * ratioMultiplyer * rotateVertSpeed * Time.deltaTime);
            this.gameObject.transform.Rotate(Vector3.up, Input.GetAxisRaw("HorizontalRotation") * ratioMultiplyer * rotateHoriSpeed * Time.deltaTime);
        }
        else
        {
            this.gameObject.transform.Rotate(Vector3.right, Input.GetAxisRaw("Mouse Y") * ratioMultiplyer * rotateVertSpeed * Time.deltaTime);
            this.gameObject.transform.Rotate(Vector3.up, Input.GetAxisRaw("Mouse X") * ratioMultiplyer * rotateHoriSpeed * Time.deltaTime);
        }

        Quaternion q = gameObject.transform.rotation;
        q.eulerAngles = new Vector3(q.eulerAngles.x, q.eulerAngles.y, 0);
        gameObject.transform.rotation = q;

    }
}



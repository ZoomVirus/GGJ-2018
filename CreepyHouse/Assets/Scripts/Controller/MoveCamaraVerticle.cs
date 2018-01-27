using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MoveCamaraVerticle : MonoBehaviour
{
    public float upperViewLimit = 0;
    public float lowerViewLimit = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float ratioMultiplyer = 1;
        if (GlobalSettings.RiftContoller)
        {

        }
        else
        if (GlobalSettings.XboxContoller)
        {
            ratioMultiplyer = GlobalSettings.xboxControllerToMouseRatioRotation;
            this.gameObject.transform.Rotate(Vector3.right, Input.GetAxisRaw("VerticalRotation") * ratioMultiplyer * GlobalSettings.rotateVertSpeed * Time.deltaTime);
        }
        else
        {
            this.gameObject.transform.Rotate(Vector3.right, Input.GetAxisRaw("Mouse Y") * ratioMultiplyer * GlobalSettings.rotateVertSpeed * Time.deltaTime);
        }

        Quaternion q = gameObject.transform.rotation;
        q.eulerAngles = new Vector3(q.eulerAngles.x, q.eulerAngles.y, 0);
        gameObject.transform.rotation = q;

    }
}

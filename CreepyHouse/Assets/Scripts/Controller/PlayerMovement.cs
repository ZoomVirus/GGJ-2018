using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float translateSpeed;
    public float rotateSpeed;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 translate = new Vector3(Input.GetAxisRaw("Horizontal") * translateSpeed * Time.deltaTime, 0, Input.GetAxisRaw("Forward") * translateSpeed * Time.deltaTime);
        Debug.Log(translate.x + "," + translate.y + "," + translate.z);
        this.gameObject.transform.Translate(translate);

              this.gameObject.transform.Rotate(Vector3.right, Input.GetAxisRaw("VerticalRotation") * rotateSpeed * Time.deltaTime);
              this.gameObject.transform.Rotate(Vector3.up, Input.GetAxisRaw("HorizontalRotation") * rotateSpeed * Time.deltaTime);
    }
}



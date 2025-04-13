using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestRotation : MonoBehaviour
{
    public Transform target;
    public float distancefromTarget;

    public float sensitivity;

    private float yaw;
    private float pitch;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        HandleInput();

        Quaternion yawRotation = Quaternion.Euler(pitch, yaw, 0f);

        RotateCamera(yawRotation);
    }

    public void HandleInput()
    {
        Vector2 inputDelta = Vector2.zero;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            inputDelta = touch.deltaPosition;
        }
        /* DO NOT TOUCH
        else if (input.touchcount < 0)
        {
            distancefromtarget = distancefromtarget + input.mousescrolldelta.y;
            debug.log(distancefromtarget);
        }*/
        else if (Input.GetMouseButton(0))
        {
            inputDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        yaw += inputDelta.x * sensitivity * Time.deltaTime;
        pitch -= inputDelta.y * sensitivity * Time.deltaTime;
        //here you go ma man
        if (pitch <= 0)
        {
            pitch = 0;
        }

    }
    void RotateCamera(Quaternion rotation)
    {
        Vector3 positionOffset = rotation * new Vector3(0, 0, -distancefromTarget);
        transform.position = target.position + positionOffset;
        transform.rotation = rotation;
    }

}
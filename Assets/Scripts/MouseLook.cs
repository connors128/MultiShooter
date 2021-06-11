using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class MouseLook : NetworkBehaviour
{
    float yRotation = 0, xRotation = 0, lookSensitivity = 5, currentXRotation = 0, 
    currentYRotation = 0, yRotationV = 0, xRotationV = 0, lookSmoothnes = 0.1f;

    void Start(){
        if(!IsLocalPlayer)
        {
            this.transform.GetComponent<AudioListener>().enabled = false;
            this.transform.GetComponent<Camera>().enabled = false;
        }
    }
    void Update()
    {
        if(IsLocalPlayer)
        {
            yRotation += Input.GetAxis("Mouse X") * lookSensitivity;
            xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;
            xRotation = Mathf.Clamp(xRotation, -90, 90);
            currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationV, lookSmoothnes);
            currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationV, lookSmoothnes);
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }

    }
}

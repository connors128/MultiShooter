using UnityEngine;
using MLAPI;

public class FPSMovement : NetworkBehaviour
{
    public float walkSpeed = 6.0f,
                 jumpSpeed = 8.0f,
                 runMultiplyer = 1.5f,
                 gravity = 20.0f;
        
    public Vector3 moveDirection = Vector3.zero;

    public CharacterController controller;
    Transform cameraTransform;
    void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        if (IsLocalPlayer)
        {
            controller = GetComponent<CharacterController>();
        }
        else
        {
            //disable other camera and audio listener
            cameraTransform.GetComponent<Camera>().enabled = false;
            cameraTransform.GetComponent<AudioListener>().enabled = false;
        }
    }

    void Update()
    {
        if(IsLocalPlayer && controller)
        {
            MovePlayer();
        }

    }

    void MovePlayer(){
        if(controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = Camera.main.transform.TransformDirection(moveDirection);
            moveDirection.y = 0.0f; //important for not 'jumping' when looking up
            
            moveDirection *= walkSpeed;
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveDirection *= runMultiplyer;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

}
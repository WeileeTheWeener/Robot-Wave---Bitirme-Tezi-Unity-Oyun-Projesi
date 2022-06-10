using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : MonoBehaviour
{
    CharacterController characterController;
    public float Velocity;
    public float walkVelocity;
    public float runVelocity;
    public float runAcceleration;
    public float runDeceleration;
    public float walkSpeedMultiplyer;
    public float runSpeedMultiplyer;
    private float speedMult;
    private Camera cam;
    private Vector3 mouseWorldPosition;
    public LayerMask layerMask;
    public Vector3 inputVector;
    public Vector3 moveDirection;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
     
    }


    void Update()
    {
        LookAtMouseDirection();
        HandleMovement();
        HandleGravity();     
    }

    void HandleGravity()
    {
        if (characterController.isGrounded)
        {
            characterController.Move(new Vector3(0, -0.1f, 0));
        }
        else if (!characterController.isGrounded)
        {
            characterController.Move(new Vector3(0, -9.8f, 0));
        }
    }

    void HandleMovement()
    {
        bool leftShiftPressed = Input.GetKey(KeyCode.LeftShift);

        inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
       

        if (leftShiftPressed)
        {
            Velocity = Mathf.MoveTowards(Velocity, runVelocity, runAcceleration * Time.deltaTime);
            speedMult = runSpeedMultiplyer;
        }
        if(!leftShiftPressed)
        {
            Velocity = Mathf.MoveTowards(Velocity, walkVelocity, runDeceleration * Time.deltaTime);
            speedMult = walkSpeedMultiplyer;
        }
        if(inputVector.x == 0f && inputVector.z ==0f)
        {
            Velocity = 0f;
        }
        if (!leftShiftPressed && inputVector.x == 0f && inputVector.z == 0f)
        {
            Velocity = Mathf.MoveTowards(Velocity, 0f, runDeceleration * Time.deltaTime);
        }
        
        characterController.Move(inputVector * Velocity * Time.deltaTime * speedMult); //move in global axis
       
       

    }
    private void LookAtMouseDirection()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        if (Physics.Raycast(ray, out hit,layerMask))
        {
            mouseWorldPosition = new Vector3(hit.point.x,transform.position.y,hit.point.z);
            gameObject.transform.forward = (mouseWorldPosition - gameObject.transform.position);
        }

      

    }



}

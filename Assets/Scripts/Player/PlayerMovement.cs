using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    
    public float speed = 5f;
    public float jumpForce = 5f;
    public float rotationSpeed;

    private CharacterController controller;
    private Vector3 move;
    private Vector3 velocity;
    public float gravity = -9.81f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jump();
        ApplyGravity();
       

    }

    void Movement(){
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // move = (transform.right * horizontal + transform.forward * vertical).normalized;
        move = new Vector3(horizontal, 0f, vertical).normalized;

        controller.Move(speed * Time.deltaTime * move);
        
        if (move != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
    void Jump(){
        if(Input.GetKeyDown(KeyCode.Space) && controller.transform.position.y < 4){
            velocity.y =  jumpForce;
        }
       
    }
     void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
   
}

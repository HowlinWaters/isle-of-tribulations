using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    
    public float speed = 5f;
    public float jumpForce = 5f;

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
        move = transform.right * horizontal + transform.forward * vertical;
        move = move.normalized;

        controller.Move(move * speed * Time.deltaTime);

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

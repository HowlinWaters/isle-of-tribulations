using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMode : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject activeChar;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float rotateSpeed = 120f;
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravityValue = -20f;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Animator animator;
    private bool isAttacking;

    void Start()
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        animator = activeChar.GetComponent<Animator>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.Rotate(0f, horizontal * rotateSpeed * Time.deltaTime, 0f);

        Vector3 move = transform.forward * vertical * speed;

        if (Input.GetKeyDown(KeyCode.Space) && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            animator.SetTrigger("Jump");
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        Vector3 finalMove = move;
        finalMove.y = playerVelocity.y;

        controller.Move(finalMove * Time.deltaTime);

        animator.SetFloat("Speed", Mathf.Abs(vertical));
        animator.SetBool("IsGrounded", controller.isGrounded);


        if (Input.GetKeyDown(KeyCode.Return) && !isAttacking)
        {
        animator.SetTrigger("Attack");
        isAttacking = true;
        StartCoroutine(ResetAttack());
        }
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.61f); 
        isAttacking = false;
    }
    }
}

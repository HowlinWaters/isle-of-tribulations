using System;
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
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();

        Debug.Log($"AudioSource: {audioSource}, Clip: {audioSource?.clip}, Volume: {audioSource?.volume}");
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)
                     || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = speed * vertical * transform.forward;

        transform.Rotate(0f, horizontal * rotateSpeed * Time.deltaTime, 0f);

        FootStepSound(isMoving);

        if (Input.GetKeyDown(KeyCode.Space) && groundedPlayer)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Return) && !isAttacking)
        {
            Attack();
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        Vector3 finalMove = move;
        finalMove.y = playerVelocity.y;

        controller.Move(finalMove * Time.deltaTime);

        animator.SetFloat("Speed", Mathf.Abs(vertical));
        animator.SetBool("IsGrounded", controller.isGrounded);
    
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        isAttacking = true;
        StartCoroutine(ResetAttack());
    }

    void Jump()
    {
        playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        animator.SetTrigger("Jump");
    }

    void FootStepSound(bool isMoving)
    {
        if (isMoving && !audioSource.isPlaying)
        {
            audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(footstepClip);
        }
        else if (!isMoving)
        {
            audioSource.Stop();
        }
    }
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.61f);
        isAttacking = false;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayMode : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject activeChar;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravityValue = -20f;
    [SerializeField] private AudioSource audioSource;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Animator animator;
    private bool isAttacking;
    private bool canMove;
    private float footstepTimer = 0f;
    private readonly float footstepInterval = 0.3f;

    void Start()
    {
        
        controller = GetComponent<CharacterController>();
        animator = activeChar.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Debug.Log($"{audioSource} acquired");
        Debug.Log($"{audioSource.clip} is loaded");
    }

    void Update()
    {
        if (!canMove) return;

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 move = Vector3.ClampMagnitude(new Vector3(horizontal, 0f, vertical), 1f) * speed;
        PlayFootsteps();

        if (move != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && groundedPlayer) Jump();
        if (Input.GetKeyDown(KeyCode.Return) && !isAttacking) Attack();

        playerVelocity.y += gravityValue * Time.deltaTime;

        Vector3 finalMove = move;
        finalMove.y = playerVelocity.y;
        controller.Move(finalMove * Time.deltaTime);

        animator.SetFloat("Speed", move.magnitude);
        animator.SetBool("IsGrounded", groundedPlayer);
    }

    void Attack()
    {
        LockMovement();
        animator.SetTrigger("Attack");
        isAttacking = true;
        StartCoroutine(ResetAttack());
    }

    void Jump()
    {
        playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        animator.SetTrigger("Jump");
    }
    
    void PlayFootsteps()
    {
        bool isRunning = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) 
        || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        if (isRunning)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.Play();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
            audioSource.Stop();
        }
    }
    
    public void LockMovement()
    {
        if (canMove) {
            canMove = false;
            audioSource.Stop();
        }
    }
    
    public void UnlockMovement()
    {
        if (!canMove) canMove = true;
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.61f);
        isAttacking = false;
        UnlockMovement();
    }
}
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds0_61 = new WaitForSeconds(0.61f);
    private static WaitForSeconds _waitForSeconds0_1 = new WaitForSeconds(0.1f);
    [Header("Character")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject activeChar;

    [Header("Attributes")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float gravityValue = -20f;
    public int hp = 5;
    [SerializeField] private Vector3 startingPosition;
    public bool isAttacking;
    // X: 2.559774
    // Y: 1.723569
    // Z: -11.34251
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    
    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    // Attributes for scripts only
    private Vector3 playerVelocity;
    private Animator animator;
    private new SkinnedMeshRenderer renderer;

    private bool canMove = true;
    private bool groundedPlayer;
    private float invincible;
    private readonly float invincibleCD = 3f;
    private float footstepTimer = 0f;
    private readonly float footstepInterval = 0.3f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = activeChar.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        activeChar.transform.position = new Vector3(
            startingPosition.x,
            startingPosition.y,
            startingPosition.z
        );
        renderer = activeChar.GetComponentInChildren<SkinnedMeshRenderer>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        if (invincible > 0) invincible -= Time.deltaTime;

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        
        if (canMove) Move();

        if (Input.GetKeyDown(KeyCode.Return) && canMove && !isAttacking)
        {
            // Collider[] hits = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("RockLayer"));
        
            // if (hits.Length == 0)
            // {
            Attack(); // no rocks nearby, attack normally
            // }
        }
    }

    void Move()
    {
        if (!canMove) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical"); 

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * vertical + camRight * horizontal;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        Vector3 move = moveDirection * speed;
        playerVelocity.y += gravityValue * Time.deltaTime;

        Vector3 finalMove = move;
        finalMove.y = playerVelocity.y;
        controller.Move(finalMove * Time.deltaTime);

        animator.SetFloat("Speed", moveDirection.magnitude);
        animator.SetBool("IsGrounded", groundedPlayer);
        PlayFootsteps(moveDirection);


        // Turn character to moving direction after stopping
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                toRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
    
    void LockMovement()
    {
        if (canMove)
        {
            canMove = false;
            audioSource.Stop();
        }
        Debug.Log("Resting...");
    }

    void UnlockMovement()
    {
        if (!canMove) canMove = true;
        Debug.Log("Let's go!");
    }

    void Attack()
    {
        LockMovement();
        animator.SetTrigger("Attack");
        isAttacking = true;
        StartCoroutine(ResetAttack());
    }
    IEnumerator ResetAttack()
    {
        yield return _waitForSeconds0_61;
        isAttacking = false;
        UnlockMovement();
    }

    void PlayFootsteps(Vector3 moveDirection)
    {
        bool isRunning = moveDirection != Vector3.zero && groundedPlayer;

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
    
    public void TakeDamage(int hpLost)
    {
        hp -= hpLost;
        Debug.Log($"Ouch! You have {hp} hits remaining!");
        invincible = invincibleCD;
        float hurtDuration = invincible;
        if (invincible > 0) StartCoroutine(BlinkCoroutine(hurtDuration));
    }
    IEnumerator BlinkCoroutine(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            renderer.enabled = !renderer.enabled;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Debug.Log($"Recovered!");
        renderer.enabled = true;
    }

    public void ApplyKnockback(Vector3 direction, float force, float duration)
    {
        Debug.Log("EEEER!!! (Knockback applied)");
        StartCoroutine(KnockbackCoroutine(direction, force, duration));
    }
    IEnumerator KnockbackCoroutine(Vector3 direction, float force, float duration)
    {
        float elapsed = 0f;
        LockMovement();
        while (elapsed < duration)
        {
            Debug.Log($"Position: {activeChar.transform.position}");
            controller.Move(force * Time.deltaTime * direction);
            elapsed += Time.deltaTime;
            yield return null;
        }
        UnlockMovement();
    }

    
}
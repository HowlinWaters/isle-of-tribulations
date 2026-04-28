using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds0_61 = new WaitForSeconds(0.61f);

    [Header("Character")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject activeChar;

    [Header("Attributes")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float gravityValue = -20f;
    [SerializeField] private int hp = 5;
    [SerializeField] private Vector3 startingPosition;
    [SerializeField] private TextMeshProUGUI HPText;
    public bool isAttacking;
    // X: 2.559774
    // Y: 1.723569
    // Z: -11.34251
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource hurtSFX;
    [SerializeField] private AudioSource deathSFX;
    
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
    private bool isDead = false;

    
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        controller.enabled = false;
        Debug.Log($"Starting position: {startingPosition}");
        transform.position = startingPosition;
        controller.enabled = true;
    }

    // Initialize necessary components
    void Start()
    {
        
        animator = activeChar.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        renderer = activeChar.GetComponentInChildren<SkinnedMeshRenderer>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        
        Debug.Log($"Current audio source: {audioSource.name}");
        Debug.Log($"Sounds - Hurt: {hurtSFX}; Death: {deathSFX}");
        
        SetHPText();
    }

    void Update()
    {
        // Invincibility cooldown by time
        if (invincible > 0) invincible -= Time.deltaTime;

        groundedPlayer = controller.isGrounded;

        // Gravity applied
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        
        // Movement
        if (canMove) Move();

        // Attack
        if (Input.GetKeyDown(KeyCode.Return) && canMove && !isAttacking)
        {
            Attack(); 
        }
        
        // Keep player's HP updated
        SetHPText();
    }

    void Move()
    {
        if (!canMove) return;

        // Captures movement with key input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical"); 

        // Player's input is preserved as camera rotates
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // Movement directions
        // Diagonal direction must have its speed clamped
        Vector3 moveDirection = camForward * vertical + camRight * horizontal;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        // Speed to affect movement
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

    // Attack animation is played
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

    // Play footsteps on every movement
    internal void PlayFootsteps(Vector3 moveDirection)
    {
        bool isRunning = moveDirection != Vector3.zero && groundedPlayer;

        if (isRunning)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                // Variations of pitch for realistic footsteps
                // Footstep plays in intervals
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
        // Invincibility or death prevent this function
        if (invincible > 0 || isDead) return;

        hp -= hpLost;

        // Game Over
        if (hp <= 0)
        {
            Debug.Log($"Playing {deathSFX.name}");
            deathSFX.Play();
            hp = 0;
            isDead = true;

            Debug.Log("Player died");

        
            FindObjectOfType<GameUIManager>().GameOver();

        
            canMove = false;

            return;
    }

        Debug.Log($"Ouch! You have {hp} hits remaining!");
        Debug.Log($"Playing {hurtSFX.name}");

        hurtSFX.Play();
        invincible = invincibleCD;

        StartCoroutine(BlinkCoroutine(invincible));
    }
    IEnumerator BlinkCoroutine(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            renderer.enabled = !renderer.enabled; // Player character flashes from taking damage
            elapsed += Time.deltaTime;
            yield return null;
        }
        Debug.Log($"Recovered!");
        renderer.enabled = true;
    }

    // Apply knockback to player (unused)
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
    
    // Other classes should call this function for HP increase
    public void GainHP(int amount)
    {
        hp += amount;
    }
    
    // Set HP counter on HUD
    private void SetHPText()
    {
        HPText.text = $"HP x{hp}";
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Player unlocks gate with the right key
        if (hit.gameObject.CompareTag("Gate"))
        {
            UnlockGate gate = hit.gameObject.GetComponent<UnlockGate>();
            if (gate != null)
            {
                gate.TryUnlock(GetComponent<Inventory>());
            }
        }
    }
}
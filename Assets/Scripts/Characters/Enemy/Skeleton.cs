using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Skeleton : MonoBehaviour
{
    
    [Header("Character")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject activeChar;
    
    [Header("Attributes")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.1f;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    
    [Header("Boundaries")]
    [SerializeField] private BoxCollider roomBounds;
    private float directionInterval;
    private Animator animator;
    private Vector3 startingPos;
    private Plane[] planes;
    private new Renderer renderer;
    private Color originalColor;
    private Vector3 currentDirection;
    private float directionTimer = 0f;
    private int hp = 3;
    private float invincible = 0;
    private readonly float invincibleCD = 2f;
    private float cooldown;
    private readonly float cooldownDuration = 2f;
    private bool canMove;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = activeChar.GetComponent<Animator>();
        cam = Camera.main;
        startingPos = transform.position;
        renderer = activeChar.GetComponentInChildren<Renderer>();
        originalColor = renderer.material.color;
        canMove = true;
        
        float roomSizeX = roomBounds.bounds.size.x;
        float roomSizeZ = roomBounds.bounds.size.z;
        float minRoomSize = Mathf.Min(roomSizeX, roomSizeZ);
        directionInterval = minRoomSize / speed;
        PickNewDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0) Die();
        else
        {
            planes = GeometryUtility.CalculateFrustumPlanes(cam);
            if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
                Move();
            if (invincible > 0) invincible -= Time.deltaTime;
            if (cooldown > 0) cooldown -= Time.deltaTime;
        }
    }
    
    void PickNewDirection()
    {
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        currentDirection = directions[UnityEngine.Random.Range(0, directions.Length)];
        directionTimer = directionInterval;
    }
    
    void Move()
    {
        if (directionTimer <= 0f && canMove) PickNewDirection();
        directionTimer -= Time.deltaTime;

        float padding = 1f;
        Vector3 nextPosition = transform.position + speed * Time.deltaTime * currentDirection.normalized;
        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(nextPosition.x, roomBounds.bounds.min.x + padding, roomBounds.bounds.max.x - padding),
            nextPosition.y,
            Mathf.Clamp(nextPosition.z, roomBounds.bounds.min.z + padding, roomBounds.bounds.max.z - padding)
        );

        // Boundary hit detected via clamp
        if (clampedPosition != nextPosition)
        {
            currentDirection = -currentDirection;
            directionTimer = directionInterval;
        }

        if (canMove) controller.Move(clampedPosition - transform.position);
        animator.SetFloat("Speed", speed);

        if (currentDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(currentDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("RoomBounds"))
        {
            currentDirection = -currentDirection;
            directionTimer = directionInterval;
        }

        if (hit.gameObject.CompareTag("Player") && cooldown <= 0f)
        {
            Player player = hit.gameObject.GetComponent<Player>();
            player.TakeDamage(1);
            
            Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
            player.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
            cooldown = cooldownDuration;
        }
    }
    
    void Die()
    {
        animator.SetTrigger("Death");
        animator.SetFloat("Speed", 0);
    }
    
    void LockMovement()
    {
        if (canMove)
        {
            canMove = false;
        }
    }

    void UnlockMovement()
    {
        if (!canMove) canMove = true;
    }
    
    public void TakeDamage(int hpLost)
    {
        hp -= hpLost;
        if (hp <= 0) Debug.Log($"{gameObject.name} is dead!");
        else Debug.Log($"{gameObject.name} has {hp} hits left!");
        invincible = invincibleCD;
    }

    public void ApplyKnockback(Vector3 direction, float force, float duration)
    {
        StartCoroutine(KnockbackCoroutine(direction, force, duration));
    }
    
    IEnumerator KnockbackCoroutine(Vector3 direction, float force, float duration)
    {
        float elapsed = 0f;
        LockMovement();
        animator.SetFloat("Speed", 0);
        while (elapsed < duration)
        {
            float currentForce = Mathf.Lerp(force, 0f, elapsed / duration);
            controller.Move(currentForce * Time.fixedDeltaTime * direction);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        UnlockMovement();
    }
}

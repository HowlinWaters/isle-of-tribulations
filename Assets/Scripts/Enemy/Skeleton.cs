using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Skeleton : Enemy
{

    // Parameter ID generated from string
    private static readonly int DeathHash = Animator.StringToHash("Death");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    
    [Header("Attributes")]
    [SerializeField] private float padding;

    private Vector3 startingPos;
    private Plane[] planes;
    private float cooldown;
    private readonly float cooldownDuration = 2f;

    // Start is called before the first frame update
    // Initialize necessary components + Enemy components
    protected override void Start()
    {
        base.Start();
        cam = Camera.main;
        startingPos = transform.position;
        
        float roomSizeX = roomBounds.bounds.size.x;
        float roomSizeZ = roomBounds.bounds.size.z;
        float minRoomSize = Mathf.Min(roomSizeX, roomSizeZ);
        directionInterval = Mathf.Min(minRoomSize / speed, 1.5f);
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
            if (isDamaged) renderer.material.color = Color.red;
            if (!isDamaged) renderer.material.color = originalColor;
        }
    }
    
    void PickNewDirection()
    {
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        currentDirection = directions[Random.Range(0, directions.Length)];
        directionTimer = directionInterval;
    }
    
    void Move()
    {
        if (!canMove) return;
        if (directionTimer <= 0f) PickNewDirection(); // Directions are randomized
        directionTimer -= Time.deltaTime;

        Vector3 nextPosition = transform.position + speed * Time.deltaTime * currentDirection.normalized;
        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(nextPosition.x, roomBounds.bounds.min.x + padding, roomBounds.bounds.max.x - padding),
            nextPosition.y,
            Mathf.Clamp(nextPosition.z, roomBounds.bounds.min.z + padding, roomBounds.bounds.max.z - padding)
        );
        
        // Detect wall collision - enemy gets unstuck
        if (clampedPosition.x != nextPosition.x) currentDirection.x = -currentDirection.x;
        if (clampedPosition.z != nextPosition.z) currentDirection.z = -currentDirection.z;

        controller.Move(clampedPosition - transform.position);
        animator.SetFloat(SpeedHash, speed);

        // Skeleton faces direction it last moved in
        if (currentDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(currentDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Detect room bounds
        if (other.gameObject.layer == LayerMask.NameToLayer("RoomBounds"))
        {
            currentDirection = -currentDirection;
            directionTimer = directionInterval;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Damage the player
        if (hit.gameObject.CompareTag("Player") && cooldown <= 0f)
        {
            Player player = hit.gameObject.GetComponent<Player>();
            player.TakeDamage(1);
            
            Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
            player.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
            cooldown = cooldownDuration;
        }
    }
    
    // Death function
    protected override void Die()
    {
        animator.SetTrigger(DeathHash);
        animator.SetFloat(SpeedHash, 0);
        
        // Separate light, fire, and audio source from soon-to-be deleted skeleton
        light.transform.SetParent(null);
        Debug.Log($"Playing {fireVFX.name}");
        fireVFX.transform.SetParent(null);
        fireVFX.Simulate(1f, true, true);
        fireVFX.Play();
        GameObject temp = new GameObject("FireDeath");
        AudioSource tempAudio = temp.AddComponent<AudioSource>();
        tempAudio.PlayOneShot(deathClip);

        Destroy(activeChar);
        Destroy(fireVFX.gameObject, 2f);
        Destroy(light.gameObject, 2f);
        Destroy(tempAudio.gameObject, deathClip.length);
        StartCoroutine(FlashLight());
    }
    // Show light from fire after death
    IEnumerator FlashLight()
    {
        light.enabled = true;
        yield return new WaitForSeconds(0.5f);
        light.enabled = false;
    }
    
    public override void TakeDamage(int hpLost, Vector3 direction)
    {
        TakeDamage(hpLost);
    }

    // Skeleton takes damage
    public override void TakeDamage(int hpLost)
    {
        isDamaged = true;
        base.TakeDamage(hpLost);
        audioSource.PlayOneShot(swordHitClip);
        StartCoroutine(BlinkRed(invincible));
    }
    IEnumerator BlinkRed(float duration)
    {
        float elapsed = 0f;
        float blinkInterval = 0.1f;
        while (elapsed < duration)
        {
            mat.color = Color.red * 2f;
            yield return new WaitForSeconds(blinkInterval);

            mat.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);
            
            elapsed += blinkInterval * 2f;
        }
        mat.color = originalColor;
        isDamaged = false;   
    }
}

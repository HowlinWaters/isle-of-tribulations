using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Skeleton : Enemy
{

    // Parameter ID generated from string
    private static readonly int DeathHash = Animator.StringToHash("Death");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    
    private Vector3 startingPos;
    private Plane[] planes;
    private float cooldown;
    private readonly float cooldownDuration = 2f;
    private bool isDamaged;


    // Start is called before the first frame update
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
        if (directionTimer <= 0f) PickNewDirection();
        directionTimer -= Time.deltaTime;

        float padding = 1f;
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

        if (currentDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(currentDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("RoomBounds"))
        {
            currentDirection = -currentDirection;
            directionTimer = directionInterval;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player") && cooldown <= 0f)
        {
            Player player = hit.gameObject.GetComponent<Player>();
            player.TakeDamage(1);
            
            Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
            player.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
            cooldown = cooldownDuration;
        }
    }
    
    protected override void Die()
    {
        animator.SetTrigger(DeathHash);
        animator.SetFloat(SpeedHash, 0);
        light.transform.SetParent(null);
        Debug.Log($"Playing {fireVFX.name}");
        fireVFX.transform.SetParent(null);
        fireVFX.Simulate(1f, true, true);
        fireVFX.Play();
        Destroy(activeChar);
        Destroy(fireVFX.gameObject, 2f);
        Destroy(light.gameObject, 2f);
        StartCoroutine(FlashLight());
    }
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

    public override void TakeDamage(int hpLost)
    {
        hp -= hpLost;
        isDamaged = true;
        if (hp <= 0) Debug.Log($"{gameObject.name} is dead!");
        else Debug.Log($"{gameObject.name} has {hp} hits left!");
        invincible = invincibleCD;
        isDamaged = false;
    }
}

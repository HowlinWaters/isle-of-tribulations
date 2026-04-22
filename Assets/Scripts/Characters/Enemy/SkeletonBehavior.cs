using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SkeletonBehavior : MonoBehaviour
{
    
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject activeChar;
    [SerializeField] private BoxCollider roomBounds;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float rotationSpeed = 720f;
    // [SerializeField] private float directionInterval = 1.0f;
    private float directionInterval;
    private Animator animator;
    private Vector3 startingPos;
    private Plane[] planes;
    private new Renderer renderer;
    private Vector3 currentDirection;
    private float directionTimer = 0f;
    private int hp = 3;
    private float invincible = 0;
    private readonly float invincibleCD = 2f;
    private bool canAttack;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = activeChar.GetComponent<Animator>();
        cam = Camera.main;
        startingPos = transform.position;
        renderer = activeChar.GetComponentInChildren<Renderer>();
        canAttack = false;
        
        float roomSizeX = roomBounds.bounds.size.x;
        float roomSizeZ = roomBounds.bounds.size.z;
        float minRoomSize = Mathf.Min(roomSizeX, roomSizeZ);
        directionInterval = minRoomSize / speed;
        PickNewDirection();
    }

    // Update is called once per frame
    void Update()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
            Move();
        if (invincible > 0)
        {
            invincible -= Time.deltaTime;
        }
        if (invincible <= 0)
        {
            canAttack = true;
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
        directionTimer -= Time.deltaTime;
        if (directionTimer <= 0f) PickNewDirection();

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

        CollisionFlags flags = controller.Move(clampedPosition - transform.position);

        // Wall collision detected via physics
        if (!flags.Equals(CollisionFlags.None))
        {
            currentDirection = -currentDirection;
            directionTimer = directionInterval;
        }

        if (currentDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(currentDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    public void TakeDamage(int hpLost)
    {
        if (hp == 0) Debug.Log($"{gameObject.name} is dead");
        hp -= hpLost;
        Debug.Log($"{gameObject.name} has {hp} hits left!");
        invincible = invincibleCD;
        canAttack = false;
    }
}

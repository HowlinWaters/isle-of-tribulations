using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour, IHittable
{
    // Parameter ID generated from string
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    
    [Header("Character")]
    [SerializeField] protected CharacterController controller;
    [SerializeField] protected GameObject activeChar;

    [Header("Attributes")]
    [SerializeField] protected float speed;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected float knockbackForce;
    [SerializeField] protected float knockbackDuration;
    [SerializeField] protected int hp = 3;

    [Header("Camera")]
    [SerializeField] protected Camera cam;
    
    [Header("Boundaries")]
    [SerializeField] protected BoxCollider roomBounds;
    
    [Header("VFX")]
    [SerializeField] protected ParticleSystem fireVFX;
    [SerializeField] protected new Light light;

    protected Animator animator;
    protected float invincible = 0;
    protected readonly float invincibleCD = 2f;
    protected Vector3 currentDirection;
    protected float directionTimer = 0f;
    protected float directionInterval;
    protected bool canMove;
    protected new Renderer renderer;
    protected Color originalColor;

    protected virtual void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = activeChar.GetComponent<Animator>();
        renderer = activeChar.GetComponentInChildren<Renderer>();
        light = activeChar.GetComponentInChildren<Light>();
        fireVFX = activeChar.GetComponentInChildren<ParticleSystem>();
        originalColor = renderer.material.color;
        canMove = true;
        light.enabled = false;
    }

    public virtual void TakeDamage(int hpLost, Vector3 direction) => TakeDamage(hpLost);
    public virtual void TakeDamage(int hpLost)
    {
        if (invincible > 0) return;
        hp -= hpLost;
        invincible = invincibleCD;
        if (hp <= 0) Debug.Log($"{gameObject.name} is dead!");
        else Debug.Log($"{gameObject.name} has {hp} hits left!");
    }

    public void ApplyKnockback(Vector3 direction, float force, float duration)
    {
        StartCoroutine(KnockbackCoroutine(direction, force, duration));
    }

    IEnumerator KnockbackCoroutine(Vector3 direction, float force, float duration)
    {
        float elapsed = 0f;
        LockMovement();
        animator.SetFloat(SpeedHash, 0);
        while (elapsed < duration)
        {
            float currentForce = Mathf.Lerp(force, 0f, elapsed / duration);
            float padding = 1f;
            
            Vector3 nextPosition = transform.position + currentForce * Time.fixedDeltaTime * direction;
            Vector3 clampedPosition = new Vector3(
                Mathf.Clamp(nextPosition.x, roomBounds.bounds.min.x + padding, roomBounds.bounds.max.x - padding),
                nextPosition.y,
                Mathf.Clamp(nextPosition.z, roomBounds.bounds.min.z + padding, roomBounds.bounds.max.z - padding)
            );

            controller.Move(clampedPosition - transform.position);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        UnlockMovement();
    }

    protected void LockMovement() { if (canMove) canMove = false; }
    protected void UnlockMovement() { if (!canMove) canMove = true; }
    protected virtual void Die() { }
}

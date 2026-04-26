using System;
using System.Collections;
using UnityEngine;

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

    protected Animator animator;
    protected float invincible = 0;
    protected readonly float invincibleCD = 2f;
    protected bool canMove;
    protected new Renderer renderer;
    protected Color originalColor;

    protected virtual void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = activeChar.GetComponent<Animator>();
        renderer = activeChar.GetComponentInChildren<Renderer>();
        originalColor = renderer.material.color;
        canMove = true;
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
            controller.Move(currentForce * Time.fixedDeltaTime * direction);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        UnlockMovement();
    }

    protected void LockMovement() { if (canMove) canMove = false; }
    protected void UnlockMovement() { if (!canMove) canMove = true; }
    protected virtual void Die() { }
}

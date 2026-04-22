using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class Attack : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject weapon;
    [SerializeField] internal PlayerMovement player;
    
    [Header("Cooldown")]
    [SerializeField] private float cooldownDuration = 0.5f;
    
    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float knockbackDuration = 1.5f;
    
    private float cooldown = 0f;
    private HashSet<GameObject> enemiesHit = new HashSet<GameObject>();
    // private Collider weaponCollider;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Player is holding {weapon}");
        // weaponCollider = weapon.GetComponent<Collider>();
    }
    
    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        SkeletonBehavior skeleton = other.GetComponent<SkeletonBehavior>();
        if (other.CompareTag("Enemy") && player.isAttacking && 
        !enemiesHit.Contains(other.gameObject) && cooldown <= 0f)
        {
            // Enemy gets hurt
            Debug.Log($"{other.gameObject.name} is hit!");
            enemiesHit.Add(other.gameObject);
            skeleton.TakeDamage(1);

            // Enemy takes knockback
            Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
            skeleton.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
            cooldown = cooldownDuration;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
            enemiesHit.Remove(other.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject weapon;
    [SerializeField] internal Player player;
    
    [Header("Attributes")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector3 attackSize = new Vector3(1f, 1f, 1f);
    [SerializeField] private LayerMask enemyLayer;
    
    [Header("Knockback")]
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackDuration;
    
    private HashSet<GameObject> enemiesHit = new HashSet<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Player is holding {weapon}");
        attackPoint = weapon.GetComponent<Transform>();
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    // Hit is registered via an event in the slash animation
    void HitRegister()
    {
        Collider[] hits = Physics.OverlapBox(attackPoint.position, attackSize, transform.rotation, enemyLayer);
        foreach (Collider hit in hits)
        {
            if (!enemiesHit.Contains(hit.gameObject))
            {
                // Enemy gets hurt
                enemiesHit.Add(hit.gameObject);
                Skeleton skeleton = hit.GetComponent<Skeleton>();
                Debug.Log($"{skeleton.gameObject.name} is hit!");
                skeleton.TakeDamage(1);
                
                // Enemy takes knockback
                Vector3 knockbackDirection = (skeleton.transform.position - attackPoint.position).normalized;
                skeleton.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
            }
        }
    }
    
    // Hit is reset once the slash animation ends
    void ResetHit()
    {
        enemiesHit.Clear();
    }
}
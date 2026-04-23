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
    [SerializeField] private float knockbackForce = 80f;
    [SerializeField] private float knockbackDuration = 0.15f;
    
    private HashSet<GameObject> enemiesHit = new HashSet<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Player is holding {weapon}");
        attackPoint = weapon.GetComponent<Transform>();
        Debug.Log($"The target for {weapon.name} is {enemyLayer}");
    }

    // Function is called by an animation event.
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
    
    // Function is called by an animation event.
    void ResetHit()
    {
        enemiesHit.Clear();
    }
}
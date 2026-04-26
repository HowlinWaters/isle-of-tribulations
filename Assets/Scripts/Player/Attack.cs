using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Attack : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject weapon;
    [SerializeField] internal Player player;
    
    [Header("Attributes")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector3 attackSize = new Vector3(1f, 1f, 1f);
    [SerializeField] private LayerMask hittableLayer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip swordClip;
    [SerializeField] private AudioClip hammerClip;
    
    [Header("Knockback")]
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackDuration;
    
    private HashSet<GameObject> enemiesHit = new HashSet<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Player is holding {weapon}");
        attackPoint = weapon.GetComponent<Transform>();
    }

    // Hit is registered via an event in the slash animation
    void HitRegister()
    {
        Collider[] hits = Physics.OverlapBox(attackPoint.position, attackSize, transform.rotation, hittableLayer);
        foreach (Collider hit in hits)
        {
            GameObject root = hit.transform.root.gameObject;
            if (enemiesHit.Contains(root)) continue;
            // Enemy gets hurt
            enemiesHit.Add(root);
            IHittable hittable = hit.GetComponent<IHittable>() ?? hit.GetComponentInParent<IHittable>();
            if (hittable != null)
            {
                Vector3 hitDirection = (hit.transform.position - attackPoint.position).normalized;
                hittable.TakeDamage(1, hitDirection);
                Enemy enemy = hit.GetComponent<Enemy>();
                Rock rock = hit.GetComponent<Rock>() ?? hit.GetComponentInParent<Rock>();
                if (enemy != null)
                {
                    Debug.Log($"{enemy.gameObject.name} is hit!");
                    
                    // Enemy takes knockback
                    Vector3 knockbackDirection = (enemy.transform.position - attackPoint.position).normalized;
                    enemy.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
                }
                if (rock != null)
                {
                    if (audioSource != null)
                    {
                        audioSource.PlayOneShot(hammerClip);
                    }
                }
            }
        }
    }
    
    // Hit is reset once the slash animation ends
    void ResetHit()
    {
        enemiesHit.Clear();
    }
}
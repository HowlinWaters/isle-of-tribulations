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
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip swordClip;
    [SerializeField] private AudioClip hammerClip;
    
    [Header("Knockback")]
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackDuration;
    
    // Keep a hash set of enemies to prevent multi-damage hits
    private HashSet<GameObject> enemiesHit = new HashSet<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Player is holding {weapon}");
        attackPoint = weapon.GetComponent<Transform>();
    }
    
    // VFX is played only if the player slashes a fake wall
    void PlayVFX(GameObject vfxPrefab)
    {
        if (vfxPrefab == null) return;

        GameObject vfx = Instantiate(vfxPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

        ParticleSystem[] particles = vfx.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem ps in particles)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Play();
        }

        Destroy(vfx, 2f);
    }
    
    // Sword slash audio clip is played via an animation event
    void PlaySwordClip()
    {
        audioSource.PlayOneShot(swordClip);
    }

    // Hit is registered via an event in the slash animation
    void HitRegister()
    {
        // Enemies or other objects get hit within the range of the sword's hitbox/overlap box
        Collider[] hits = Physics.OverlapBox(attackPoint.position, attackSize, transform.rotation, hittableLayer);
        foreach (Collider hit in hits)
        {
            GameObject root = hit.transform.root.gameObject;
            if (enemiesHit.Contains(root)) continue; // Single damage to one enemy only
            // Enemy gets hurt
            enemiesHit.Add(root);
            
            // Apply damage to objects that are "hittable". An object's "hittability" is based on interface IHittable
            IHittable hittable = hit.GetComponent<IHittable>() ?? hit.GetComponentInParent<IHittable>();
            if (hittable != null)
            {
                Debug.Log($"Striking {hit.gameObject.name} with tag {hit.tag}");
                Vector3 hitDirection = (hit.transform.position - attackPoint.position).normalized;
                
                // Depending on type, either an enemy takes damage or a rock gets applied attack force
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
                    // Play hammer clip when hitting rocks
                    if (audioSource != null)
                    {
                        audioSource.PlayOneShot(hammerClip);
                    }
                }
                
            }
            if (hit.CompareTag("Wall"))
            {
                // Play hammer clip and VFX once fake wall is struck open
                PlayVFX(hitVFX);
                audioSource.PlayOneShot(hammerClip);
                Destroy(hit.gameObject);
            }
            if (hit.CompareTag("Button"))
            {
                // Button is pushed
                RockDestroyManager rdm = FindObjectOfType<RockDestroyManager>();
                if (rdm != null)
                {
                    rdm.PushButton();
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
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
    
    private HashSet<GameObject> enemiesHit = new HashSet<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Player is holding {weapon}");
        attackPoint = weapon.GetComponent<Transform>();
    }
    
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
    
    void PlaySwordClip()
    {
        audioSource.PlayOneShot(swordClip);
    }

    // Hit is registered via an event in the slash animation
    void HitRegister()
    {
        Collider[] hits = Physics.OverlapBox(attackPoint.position, attackSize, transform.rotation, hittableLayer);
        foreach (Collider hit in hits)
        {
            Debug.Log($"Striking {hit.gameObject.name} with tag {hit.tag}");
            GameObject root = hit.transform.root.gameObject;
            if (enemiesHit.Contains(root)) continue;
            // Enemy gets hurt
            enemiesHit.Add(root);
            IHittable hittable = hit.GetComponent<IHittable>() ?? hit.GetComponentInParent<IHittable>();
            if (hittable != null)
            {
                Debug.Log($"Striking {hit.gameObject.name} with tag {hit.tag}");
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
            if (hit.CompareTag("Wall"))
            {
                Debug.Log($"{hit.gameObject.name} hit!");
                PlayVFX(hitVFX);
                audioSource.PlayOneShot(hammerClip);
                Destroy(hit.gameObject);
            }
            if (hit.CompareTag("Button"))
            {
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
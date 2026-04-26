using UnityEngine;

public class Rock : MonoBehaviour, IHittable
{
    [SerializeField] private LayerMask RockLayer;
    [SerializeField] private float checkDistance = 1.5f;
    [SerializeField] private float rayHeight = 1f;

    [Header("VFX")]
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject breakVFX;

    [SerializeField] private int hitsToBreak = 3;

    private int hitCount = 0;
    private RockDestroyManager puzzleManager;

    void Start()
    {
        puzzleManager = FindObjectOfType<RockDestroyManager>();
    }
    
    public void TakeDamage(int hpLost, Vector3 direction)
    {
        Hit(direction);
    }

    public void Hit(Vector3 direction)
    {
        Debug.Log($"Number of hits: {hitCount}");
        hitCount++;

        PlayVFX(hitVFX);

        if (hitCount >= hitsToBreak)
        {
            Break(direction);
        }
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

    public void Break(Vector3 direction)
    {
        direction.y = 0f;
        direction = new Vector3(Mathf.Round(direction.x), 0f, Mathf.Round(direction.z)).normalized;

        Vector3 origin = transform.position + Vector3.up * rayHeight;

        Rock nextBlock = null;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, checkDistance, RockLayer))
        {
            nextBlock = hit.collider.GetComponent<Rock>();
        }

        PlayVFX(breakVFX);

        if (puzzleManager != null)
        {
            puzzleManager.BlockDestroyed();
        }

        Destroy(gameObject);

        if (nextBlock != null)
        {
            nextBlock.Break(direction);
        }
    }
}
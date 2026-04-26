using UnityEngine;

public class UnlockGate : MonoBehaviour
{
    [SerializeField] private int requiredkey = 1;
    [SerializeField] private AudioSource gateopenSound;
    [SerializeField] private GameObject gateopenVFX;

    private bool unlocked = false;

    private void OnTriggerEnter(Collider c)
    {
        if (unlocked) return;
        if (!c.CompareTag("Player")) return;

        Inventory inventory = c.GetComponent<Inventory>();

        if (inventory != null && inventory.HashKey(requiredkey))
        {
            unlocked = true;
            inventory.UseKey(requiredkey);

            if (gateopenSound != null)
                gateopenSound.Play();

            if (gateopenVFX != null)
            {
                gateopenVFX.transform.SetParent(null);
                gateopenVFX.SetActive(true);

                ParticleSystem[] particles = gateopenVFX.GetComponentsInChildren<ParticleSystem>(true);

                foreach (ParticleSystem ps in particles)
                {
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    ps.Play();
                }

                Destroy(gateopenVFX, 2f);
            }

            Collider[] cols = GetComponentsInChildren<Collider>();
            foreach (Collider col in cols)
                col.enabled = false;

            Renderer[] rends = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rends)
                r.enabled = false;

            Destroy(gameObject, 2f);
        }
        else
        {
            Debug.Log("Door is locked");
        }
    }
}
using UnityEngine;

public class KeyBehavior : MonoBehaviour
{
    // Key IDs as "hashes" for gates
    [SerializeField] private int keyid = 1;
    [SerializeField] private GameObject keyVFX;
    [SerializeField] private AudioSource keySound;

    private bool pickedUp = false;

    void Update()
    {
        // Key appears as a collectible for the player to grab
        if (!pickedUp)
            transform.Rotate(0f, 45f * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider c)
    {
        if (pickedUp) return; // Key is already in inventory
        if (!c.CompareTag("Player")) return; // Everyone but the player cannot obtain the key

        // Player's inventory must be found
        // Key is added to player's inventory
        Inventory inventory = c.GetComponent<Inventory>();
        Debug.Log($"Found {inventory.name}");

        if (inventory != null)
        {
            pickedUp = true;
            inventory.AddKey(keyid);
            Debug.Log("Picked up key " + keyid);

            if (keySound != null)
            {
                keySound.Play();
            }

            if (keyVFX != null)
            {
                keyVFX.SetActive(false);
            }

            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            Renderer r = GetComponentInChildren<Renderer>();
            if (r != null) r.enabled = false;

            Destroy(gameObject, 1.5f);
        }
    }
}
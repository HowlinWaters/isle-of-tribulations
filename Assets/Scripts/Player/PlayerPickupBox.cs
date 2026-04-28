using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    // Player's position as they hold the box
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float pickupRange = 2f;

    // The box being held by player
    private GameObject heldObject;

    [SerializeField] private AudioSource pickupsound;

    void Update()
    {
        // Pick up functionality
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (heldObject == null)
            {
                TryPickUp();
            }
            else
            {
                DropObject();
            }
        }
    }

    void TryPickUp()
    {
        // Pick ups are based on the player's overlap sphere
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);

        foreach (Collider hit in hits)
        {
            // Boxes must be labeled as "Pickup" for them to be carried
            if (hit.CompareTag("Pickup"))
            {
                heldObject = hit.gameObject;

                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                heldObject.transform.SetParent(holdPoint);
                
                // Stabilize the box as the player holds it
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;
                if(pickupsound != null){
                    pickupsound.Play();
                }

                break; // ???
            }
        }
    }

    void DropObject()
    {
        if (heldObject == null) return; // Player cannot drop a box twice

        // Player drops the box
        heldObject.transform.SetParent(null);

        // Box gets stabilized
        if (heldObject.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if(pickupsound != null){
                pickupsound.Play();
        }

        heldObject = null; // No box is being carried
    }
}
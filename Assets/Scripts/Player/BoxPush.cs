using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPush : MonoBehaviour
{
    [SerializeField] private float force;

    private void OnControllerColliderHit(ControllerColliderHit hit){
        Rigidbody rb = hit.collider.attachedRigidbody;

        // Apply force on a box to push based on direction
        if (rb != null){
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize(); // Normalize force to prevent unintentional behavior

            // ForceMode.Impulse - Force is instantly applied based on a box's mass
            rb.AddForceAtPosition(forceDirection * force, transform.position, ForceMode.Impulse);
        }
    }
}

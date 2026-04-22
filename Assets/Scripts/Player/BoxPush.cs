using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPush : MonoBehaviour
{
    [SerializeField] private float force;

    private void OnControllerColliderHit(ControllerColliderHit hit){
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb != null){
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rb.AddForceAtPosition(forceDirection * force, transform.position, ForceMode.Impulse);
        }
    }
}

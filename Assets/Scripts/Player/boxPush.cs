using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxPush : MonoBehaviour
{
    [SerializeField] private float force;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

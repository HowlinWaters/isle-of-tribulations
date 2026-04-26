using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBehavior : MonoBehaviour
{
    [SerializeField] private GameObject heart;
    // Start is called before the first frame update
    
    private bool isPickedUp;
    private float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 45f;
    } 

    // Update is called once per frame
    void Update()
    {
        if (!isPickedUp)
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPickedUp) return;
        if (!other.CompareTag("Player")) return;

        if (other.TryGetComponent<Player>(out var player))
        {
            player.GainHP(1);
            Destroy(gameObject);
        }
    }
}

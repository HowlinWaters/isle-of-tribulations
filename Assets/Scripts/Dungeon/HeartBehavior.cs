using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBehavior : MonoBehaviour
{
    [SerializeField] protected GameObject heart;
    [SerializeField] protected AudioClip heartSFX;
    
    protected bool isPickedUp;
    protected float rotationSpeed;

    // Start is called before the first frame update
    protected void Start()
    {
        rotationSpeed = 45f;
    } 

    // Update is called once per frame
    void Update()
    {
        // Heart is a collectible for the player to grab
        if (!isPickedUp)
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPickedUp) return; // Heart is already obtained
        if (!other.CompareTag("Player")) return; // Everyone but the player cannot obtain the heart

        // Player gains an extra hit point
        if (other.TryGetComponent<Player>(out var player))
        {
            player.GainHP(1);

            // Temporary object and its audio source is instantiated to play the heart sound effect
            float clipLength = heartSFX.length;
            GameObject temp = new GameObject("HeartGain");
            AudioSource tempAudio = temp.AddComponent<AudioSource>();
            tempAudio.PlayOneShot(heartSFX);

            // Destroy the temporary object before the original one
            Destroy(temp, clipLength);
            Destroy(gameObject);
        }
    }
}

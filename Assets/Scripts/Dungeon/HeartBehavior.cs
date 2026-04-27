using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBehavior : MonoBehaviour
{
    [SerializeField] protected GameObject heart;
    [SerializeField] protected AudioClip heartSFX;
    // Start is called before the first frame update
    
    protected bool isPickedUp;
    protected float rotationSpeed;
    private AudioSource audioSource;

    // Start is called before the first frame update
    protected void Start()
    {
        rotationSpeed = 45f;
        audioSource = heart.GetComponent<AudioSource>();
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

            float clipLength = heartSFX.length;
            GameObject temp = new GameObject("HeartGain");
            AudioSource tempAudio = temp.AddComponent<AudioSource>();
            tempAudio.PlayOneShot(heartSFX);

            Destroy(temp, clipLength);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeartSpecialBehavior : HeartBehavior
{
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
            
            // GameStatus status = FindObjectOfType<GameStatus>();
            GameUIManager ui = FindObjectOfType<GameUIManager>();
            Debug.Log("You win! Thank you for playing!");
            if (ui != null)
            {
                ui.GameWin();
            }
            Destroy(gameObject);
        }
    }
}


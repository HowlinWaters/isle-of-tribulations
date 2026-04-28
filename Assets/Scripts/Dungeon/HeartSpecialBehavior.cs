using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeartSpecialBehavior : HeartBehavior
{
    // Update is called once per frame
    void Update()
    {
        // Special heart is a collectible for the player to grab
        if (!isPickedUp)
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPickedUp) return; // Special heart is already obtained
        if (!other.CompareTag("Player")) return; // Everyone but the player cannot obtain the special heart

        /*
         * Player not only gets an additional hit point, but they also successfully
         * completed the dungeon. The special heart grants them an extra hit point once
         * starting a new dungeon.
         */
        if (other.TryGetComponent<Player>(out var player))
        {
            player.GainHP(1);
            
            float clipLength = heartSFX.length;
            GameObject temp = new GameObject("HeartGain");
            AudioSource tempAudio = temp.AddComponent<AudioSource>();
            tempAudio.PlayOneShot(heartSFX);

            Destroy(temp, clipLength);
            
            GameUIManager ui = FindObjectOfType<GameUIManager>();
            Debug.Log("You win! Thank you for playing!");
            if (ui != null)
            {
                ui.GameWin();
            }
            
            // Destroy object last to prevent errors
            Destroy(gameObject);
        }
    }
}


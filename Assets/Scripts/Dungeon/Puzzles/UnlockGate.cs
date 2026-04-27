using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockGate : MonoBehaviour
{
    [SerializeField] private int requiredkey;
    [SerializeField] private AudioSource audioSource;
    
    private AudioClip audioClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log($"{audioSource.name} initialized. {audioSource.clip} loaded");
    }

    public void TryUnlock(Inventory inventory){
        if (inventory != null && inventory.HashKey(requiredkey)) {
            inventory.UseKey(requiredkey);
            Debug.Log("Unlock door");
            
            // Create a temporary GameObject just for the audio
            GameObject tempAudioObject = new GameObject("GateUnlockAudio");
            tempAudioObject.transform.position = transform.position;
            AudioSource tempAudio = tempAudioObject.AddComponent<AudioSource>();
            tempAudio.clip = audioSource.clip;
            tempAudio.volume = 0.5f;
            tempAudio.Play();
            Destroy(tempAudioObject, audioSource.clip.length);

            gameObject.SetActive(false);
        }
        else{
            Debug.Log("Door is locked");
        }
    }
}

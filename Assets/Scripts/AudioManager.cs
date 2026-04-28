using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This class only plays on UI!
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource audioSource;

    // One and only one instance of AudioManager must be made in any scene
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0.1f;
        }
    }

    // Play sounds for UI
    public static void PlaySound(AudioClip clip)
    {
        if (Instance != null)
        {
            Instance.audioSource.PlayOneShot(clip);
        }
    }
}

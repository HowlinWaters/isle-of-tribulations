using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.1f;
    }

    public void PlaySound(AudioClip clip)
    {
        Debug.Log($"Playing: {clip}, Volume: {audioSource.volume}, Muted: {audioSource.mute}");
        audioSource.PlayOneShot(clip);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource audioSource;

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

    public static void PlaySound(AudioClip clip)
    {
        if (Instance != null)
        {
            Instance.audioSource.PlayOneShot(clip);
        }
    }
}

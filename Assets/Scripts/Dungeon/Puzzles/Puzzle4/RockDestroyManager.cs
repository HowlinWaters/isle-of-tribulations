using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RockDestroyManager : MonoBehaviour
{
    [SerializeField] private DoorUpward3 Door3;
    [SerializeField] private GameObject button;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonPress;

    private bool isPushed = false;
    private int remainingBlocks;

    void Start()
    {
        remainingBlocks = FindObjectsOfType<Rock>().Length;
        audioSource = GetComponent<AudioSource>();
        Debug.Log("Total ice blocks: " + remainingBlocks);
    }
    
    public void PushButton()
    {
        audioSource.PlayOneShot(buttonPress);
        isPushed = true;
        if (isPushed)
        {
            if (Door3 != null)
            {
                Door3.OpenDoor();
                Debug.Log("Door opened");
            }
        }
        
    }

    public void BlockDestroyed()
    {
        remainingBlocks--;
        Debug.Log("Remaining blocks: " + remainingBlocks);

        if (remainingBlocks <= 0)
        {
            Debug.Log("Are you missing something?");
        }
    }
}

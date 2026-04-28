using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUpward : MonoBehaviour
{
    [SerializeField] private Vector3 openOffset = new Vector3(0f, 4f, 0f);
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private ParticleSystem left;
    [SerializeField] private ParticleSystem right;
    [SerializeField] private GameObject closeDoorFX;

    [Header("Audio")]
    [SerializeField] private AudioSource loopAudio;
    [SerializeField] private AudioSource openAudio;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;

    // Initialize open and close positions of the door
    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;

        // Audio of the portal-shaped figure plays when the door is closed
        if(loopAudio != null){
            loopAudio.Play();
        }
    }

    void Update()
    {
        if (isOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPosition, openSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, openSpeed * Time.deltaTime);
        }
    }

    public void OpenDoor()
    {
        if(isOpen) return; // Doors cannot open twice

        isOpen = true;

        if(loopAudio != null && loopAudio.isPlaying){
            loopAudio.Stop();
        }
        if(openAudio != null){
            openAudio.Play();
        }

        // Open door VFX replaces that of the closed door
        if(left != null && right != null){
            left.Play();
            right.Play();
        } 
        if(closeDoorFX != null){
            closeDoorFX.SetActive(false);
        }

        
    }

    public void CloseDoor()
    {
        if(!isOpen) return; // Doors cannot close twice
        isOpen = false;
        if(closeDoorFX != null){
            closeDoorFX.SetActive(true); // Closed door VFX shows a portal-shaped figure again
        }

        if (loopAudio != null && !loopAudio.isPlaying)
        {
            loopAudio.Play();
        }
    }
}

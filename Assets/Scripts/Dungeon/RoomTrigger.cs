using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    
    protected CameraController cam;
    protected BoxCollider boxCol;
    protected bool canTrigger = true;

    // Start is called before the first frame update
    // Camera and box collider must be initialized
    protected virtual void Start()
    {
       cam = Camera.main.GetComponent<CameraController>(); 
       boxCol = GetComponent<BoxCollider>();
       Debug.Log($"Camera controller found: {cam}");
    }

    /*
     * When player enters a new room, the camera will shift to that new room.
     * The player cannot trigger a room shift until they leave the room
     * bounds.
     */
    protected virtual void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player") && canTrigger)
       {
            Debug.Log($"Entered: {gameObject.name}");
            canTrigger = false;
            cam.ShiftToRoom(boxCol.bounds);
       } 
    }
    
    // Player can trigger the camera shift again
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTrigger = true;
        } 
    }
}

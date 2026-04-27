using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    
    protected CameraController cam;
    protected BoxCollider boxCol;
    protected bool canTrigger = true;

    // Start is called before the first frame update
    protected virtual void Start()
    {
       cam = Camera.main.GetComponent<CameraController>(); 
       boxCol = GetComponent<BoxCollider>();
       Debug.Log($"Camera controller found: {cam}");
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player") && canTrigger)
       {
            Debug.Log($"Entered: {gameObject.name}");
            canTrigger = false;
            cam.ShiftToRoom(boxCol.bounds);
       } 
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTrigger = true;
        } 
    }
}

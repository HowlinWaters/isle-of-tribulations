using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [Header("Boss Room")]
    [SerializeField] private bool isBossRoom;
    [SerializeField] private GameObject gateReference;
    [SerializeField] private GameObject boss;

    private CameraController cam;
    private BoxCollider boxCol;
    private bool canTrigger = true;

    // Start is called before the first frame update
    void Start()
    {
       cam = Camera.main.GetComponent<CameraController>(); 
       boxCol = GetComponent<BoxCollider>();
       Debug.Log($"Camera controller found: {cam}");
    }
    
    void Update()
    {
        if (boss == null)
        {
            gateReference.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player") && canTrigger)
       {
            Debug.Log($"Entered: {gameObject.name}");
            canTrigger = false;
            cam.ShiftToRoom(boxCol.bounds);
            
            if (isBossRoom && gateReference != null)
            {
                gateReference.SetActive(true);
            }
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

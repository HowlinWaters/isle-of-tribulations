using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : RoomTrigger
{
    [Header("Boss Room")]
    [SerializeField] private GameObject gateReference;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject heartSP;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        heartSP.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (boss == null)
        {
            gateReference.SetActive(false);
            heartSP.SetActive(true);
        }
    }
    
    protected override void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player") && canTrigger)
       {
            Debug.Log($"Entered: {gameObject.name}");
            canTrigger = false;
            cam.ShiftToRoom(boxCol.bounds);
            
            if (gateReference != null)
            {
                gateReference.SetActive(true);
            }
       }
    }
}

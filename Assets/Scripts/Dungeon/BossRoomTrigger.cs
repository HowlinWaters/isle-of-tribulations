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
    /*
     * Like any room triggers, the boss room trigger is for the camera to move across various rooms.
     * The key difference is that it holds a special heart, the artifact for the player.
     */
    protected override void Start()
    {
        base.Start();
        heartSP.SetActive(false);
    }

    // Update is called once per frame
    /*
     * The gate locking the player in the boss room must be released
     * and the special heart must be visible to obtain
     */
    void Update()
    {
        if (boss == null)
        {
            gateReference.SetActive(false);
            heartSP.SetActive(true);
        }
    }
    
    // Camera shifts to the room, but a gate closes in on the player, indicating a boss battle
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

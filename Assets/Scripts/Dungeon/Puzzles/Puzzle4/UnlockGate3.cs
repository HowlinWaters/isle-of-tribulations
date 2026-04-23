using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockGate3 : MonoBehaviour
{
    [SerializeField] private int requiredkey = 4;
    private void OnTriggerEnter(Collider c){
        if(!c.CompareTag("Player")) return;

        Inventory inventory = c.GetComponent<Inventory>();

        if(inventory != null && inventory.HashKey(requiredkey)){
            inventory.UseKey(requiredkey);
            Debug.Log("Unlock door");
            gameObject.SetActive(false);
        }
        else{
            Debug.Log("Door is locked");
        }
    }
}

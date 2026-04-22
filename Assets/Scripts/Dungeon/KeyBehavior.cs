using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehavior : MonoBehaviour
{
    [SerializeField] private int keyid = 1;
    void Update()
    {
        transform.Rotate(new Vector3(0f, 45f, 0f) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider c)
    {
        if(!c.CompareTag("Player")) return;

        Inventory inventory = c.GetComponent<Inventory>();
        if(inventory != null)
        {
            inventory.AddKey(keyid);
            Debug.Log("Picked up key" + keyid);
            gameObject.SetActive(false);
        }
        
    }
}

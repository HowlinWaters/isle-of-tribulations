using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    private List<int> collectedKeys = new List<int>();
    public TextMeshProUGUI countkeyText;

    // Initialize key counter on the HUD
    void Start(){
       SetKeyText();
    }

    // Add the key to inventory if the player obtains it
    public void AddKey(int keyid){
        if(!collectedKeys.Contains(keyid)){
            collectedKeys.Add(keyid);
            Debug.Log("key added" + keyid);
            SetKeyText();
        }
    }
    
    // Check a specific key based on its ID/"hash"
    public bool HashKey(int keyid){
        return collectedKeys.Contains(keyid);
    }
    public void UseKey(int keyid){
        if(collectedKeys.Contains(keyid)){
            collectedKeys.Remove(keyid);
            Debug.Log("key removed" + keyid);
            SetKeyText();
        }
    }

    // Update key counter on HUD when needed
    public void SetKeyText(){
        countkeyText.text = "Keys x" + collectedKeys.Count.ToString();
    }

}

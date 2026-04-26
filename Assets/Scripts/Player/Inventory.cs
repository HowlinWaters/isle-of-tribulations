using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    private List<int> collectedKeys = new List<int>();
    public TextMeshProUGUI countkeyText;

    void Start(){
       SetKeyText();
    }

    public void AddKey(int keyid){
        if(!collectedKeys.Contains(keyid)){
            collectedKeys.Add(keyid);
            Debug.Log("key added" + keyid);
            SetKeyText();
        }
    }
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

    public void SetKeyText(){
        countkeyText.text = "Keys x" + collectedKeys.Count.ToString();
    }

}

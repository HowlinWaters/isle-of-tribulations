using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnterGame : MonoBehaviour
{
    // private Button button;

    // Start is called before the first frame update
    /* void Start()
    {
    //    button = GetComponent<Button>();
       Debug.Log($"Play button is generated");
    } */
    
    public void StartGame()
    {
        OnClick();
    }

    // Player begins the game
    void OnClick()
    {
       SceneManager.LoadScene(sceneName:"Dungeon1"); 
    }
}

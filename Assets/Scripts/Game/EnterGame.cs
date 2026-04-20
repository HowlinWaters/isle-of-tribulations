using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnterGame : MonoBehaviour
{
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

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    
    public void Quit()
    {
        OnClick();
    }

    void OnClick()
    {
        Application.Quit();
    }
}

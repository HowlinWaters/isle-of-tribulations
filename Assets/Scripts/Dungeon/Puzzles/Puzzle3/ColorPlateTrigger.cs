using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPlateTrigger : MonoBehaviour
{
    public string requiredcolor;
    public bool iscorrect = false;

    private void OnTriggerEnter(Collider other)
    {
        ColorPickup pc = other.GetComponent<ColorPickup>();

        if (pc != null && pc.Color == requiredcolor)
        {
            iscorrect = true;
            FindObjectOfType<ColorPlateManager>().CheckAll();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ColorPickup pc = other.GetComponent<ColorPickup>();

        if (pc != null && pc.Color == requiredcolor)
        {
            iscorrect = false;
        }
    }
}

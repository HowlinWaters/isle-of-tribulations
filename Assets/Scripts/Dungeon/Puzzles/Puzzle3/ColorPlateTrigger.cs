using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPlateTrigger : MonoBehaviour
{
    public string requiredcolor;
    public bool iscorrect = false;
    private ColorPlateManager manager;

    void Start()
    {
        manager = FindObjectOfType<ColorPlateManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckBox(other);
    }

    private void OnTriggerStay(Collider other)
    {
        CheckBox(other);
    }

    private void OnTriggerExit(Collider other)
    {
        ColorPickup pc = other.GetComponent<ColorPickup>();

        if (pc != null && pc.Color == requiredcolor)
        {
            iscorrect = false;
        }
    }

    private void CheckBox(Collider other)
    {
        ColorPickup pc = other.GetComponent<ColorPickup>();

        if (pc != null && pc.Color == requiredcolor)
        {
            iscorrect = true;
            Debug.Log(gameObject.name + " correct: " + requiredcolor);

            if (manager != null)
            {
                manager.CheckAll();
            }
        }
    }
}

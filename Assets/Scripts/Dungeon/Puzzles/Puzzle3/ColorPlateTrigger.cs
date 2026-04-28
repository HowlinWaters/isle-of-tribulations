using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPlateTrigger : MonoBehaviour
{
    public string requiredColor;
    public bool isCorrect = false;
    private ColorPlateManager cpm;

    void Start()
    {
        // Trigger needs the color plate manager
        cpm = FindObjectOfType<ColorPlateManager>();
    }

    // OnTriggerEnter and OnTriggerStay both identify the box's color
    private void OnTriggerEnter(Collider other)
    {
        CheckBox(other);
    }

    private void OnTriggerStay(Collider other)
    {
        CheckBox(other);
    }

    // ???
    private void OnTriggerExit(Collider other)
    {
        ColorPickup pc = other.GetComponent<ColorPickup>();

        if (pc != null && pc.Color == requiredColor)
        {
            isCorrect = false;
        }
    }

    private void CheckBox(Collider other)
    {
        ColorPickup pc = other.GetComponent<ColorPickup>();

        // Box's color matches the color plate - puzzle is solved
        if (pc != null && pc.Color == requiredColor)
        {
            isCorrect = true;
            Debug.Log(gameObject.name + " correct: " + requiredColor);

            if (cpm != null)
            {
                cpm.CheckAll();
            }
        }
    }
}

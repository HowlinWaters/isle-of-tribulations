using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDestroyManager : MonoBehaviour
{
    [SerializeField] private DoorUpward3 Door3;

    private int remainingBlocks;

    void Start()
    {
        remainingBlocks = FindObjectsOfType<Rock>().Length;
        Debug.Log("Total ice blocks: " + remainingBlocks);
    }

    public void BlockDestroyed()
    {
        remainingBlocks--;
        Debug.Log("Remaining blocks: " + remainingBlocks);

        if (remainingBlocks <= 0)
        {
            if (Door3 != null)
            {
                Door3.OpenDoor();
                Debug.Log("Door opened");
            }
        }
    }
}

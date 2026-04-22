using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public int[] order = { 1, 2, 3, 4, 5 };
    private int stepcount = 0;

    [SerializeField] private DoorUpward Door;

    public void TilePressed(int id)
    {
        Debug.Log("Pressed tile id = " + id);

        if (id == order[stepcount])
        {
            stepcount++;
            Debug.Log("correct step, current stepcount = " + stepcount);

            if (stepcount == order.Length)
            {
                Debug.Log("Puzzle Solved");

                if (Door != null)
                {
                    Door.OpenDoor();
                    Debug.Log("Door open called");
                }
                else
                {
                    Debug.Log("Door is NULL");
                }
            }
        }
        else
        {
            Debug.Log("Wrong tile");
            stepcount = 0;
        }
    }
}
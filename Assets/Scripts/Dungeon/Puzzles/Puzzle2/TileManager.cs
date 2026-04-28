using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public int[] order = { 1, 2, 3, 4, 5 };
    private int stepcount = 0;
    private bool puzzleComplete = false;

    [SerializeField] private DoorUpward1 Door;

    public void TilePressed(int id)
    {
        if (puzzleComplete)
        {
            return;
        }
        Debug.Log("Pressed tile id = " + id);

        if (id == order[stepcount])
        {
            // Correct order is tracked by a step count of each tile
            stepcount++;
            Debug.Log("correct step, current stepcount = " + stepcount);

            // Puzzle solved
            if (stepcount >= order.Length)
            {
                Debug.Log("Puzzle Solved");
                puzzleComplete = true;

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
            // Reset step count once player follows the wrong order
            Debug.Log("Wrong tile");
            stepcount = 0;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceTile : MonoBehaviour
{
    public int tileID;
    // Tile manager keeps track of the order of tiles pressed
    [SerializeField] private TileManager tm;

    public void Pressed()
    {
        Debug.Log("SequenceTile pressed, tileid = " + tileID);

        if (tm != null)
        {
            tm.TilePressed(tileID);
        }
        else
        {
            Debug.Log("TileManager is NULL");
        }
    }
}
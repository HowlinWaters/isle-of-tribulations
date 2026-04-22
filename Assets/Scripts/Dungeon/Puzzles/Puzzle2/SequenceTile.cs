using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceTile : MonoBehaviour
{
    public int tileid;
    [SerializeField] private TileManager tm;

    public void Pressed()
    {
        Debug.Log("SequenceTile pressed, tileid = " + tileid);

        if (tm != null)
        {
            tm.TilePressed(tileid);
        }
        else
        {
            Debug.Log("TileManager is NULL");
        }
    }
}
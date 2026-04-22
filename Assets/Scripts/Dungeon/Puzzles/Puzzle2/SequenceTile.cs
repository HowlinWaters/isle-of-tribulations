using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceTile : MonoBehaviour
{
    public int tileid;
    private TileManager tm;
    void Start(){
        tm = FindObjectOfType<TileManager>();
    }
    public void Pressed(){
        tm.TilePressed(tileid);
    }
}

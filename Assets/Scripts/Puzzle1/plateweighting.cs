using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plateweighting : MonoBehaviour
{
    [SerializeField] private int requiredWeight = 3;
    [SerializeField] private DoorUpward door1;

    private List<WeightObject> objonplate = new List<WeightObject>();
    private int curWeight = 0;
    private bool puzzelsolved = false;

    private void onTriggerEnter(Collider c)
    {
        WeightObject wb = c.GetComponent<WeightObject>();
        if(wb != null && !objonplate.Contains(wb)){
            objonplate.Add(wb);
            calculateWeight();
        }
    }
    private void onTriggerExit(Collider c)
    {
        WeightObject wb = c.GetComponent<WeightObject>();
        if(wb != null && objonplate.Contains(wb)){
            objonplate.Remove(wb);
            calculateWeight();
        }
    }

    private void calculateWeight(){
        curWeight = 0;
        foreach (WeightObject wbj in objonplate){
            if (wbj != null){
                curWeight += wbj.weight;
            }
        }
        Debug.Log("Current Weight: " + curWeight);
        if(!puzzelsolved && curWeight == requiredWeight){
            puzzelsolved = true;
            Debug.Log("Puzzle solved. Door opened");

            if(door1 != null){
                door1.OpenDoor();
            }
        }
    }
}

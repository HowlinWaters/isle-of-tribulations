using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plateweighting : MonoBehaviour
{
    [SerializeField] private int requiredWeight = 3;
    [SerializeField] private DoorUpward Door;

    private List<WeightObject> objonplate = new List<WeightObject>();
    private int curWeight = 0;
    private bool puzzelsolved = false;

    private void OnTriggerEnter(Collider c)
    {
        WeightObject wb = c.GetComponent<WeightObject>();
        if(wb != null && !objonplate.Contains(wb)){
            Debug.Log("Something entered plate: " + c.name);
            objonplate.Add(wb);
            calculateWeight();
        }
    }
    private void OnTriggerExit(Collider c)
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

            if(Door != null){
                Door.OpenDoor();
            }
        }
        else if (puzzelsolved && curWeight < requiredWeight)
        {
            puzzelsolved = false;

            if (Door != null)
            {
                Door.CloseDoor();
            }
        }
    }
}

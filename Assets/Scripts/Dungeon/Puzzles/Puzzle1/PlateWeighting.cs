using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateWeighting : MonoBehaviour
{
    [SerializeField] private int requiredWeight = 3;
    [SerializeField] private DoorUpward Door;
    [SerializeField] private AudioSource trigger;

    private List<WeightObject> objonplate = new List<WeightObject>();
    private int curWeight = 0;
    private bool puzzleSolved = false;

    public ParticleSystem magicCircle;

    private void Start()
    {
        if (magicCircle != null)
        {
            magicCircle.Stop();
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        // Checking weight of an object
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

            if(trigger != null){
                trigger.Stop();
            }
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
        
        // Box weight must be heavy enough to push the pressure plate completely
        // and open the door
        if (!puzzleSolved && curWeight == requiredWeight){
            puzzleSolved = true;
            Debug.Log("Puzzle solved. Door opened");

            if( magicCircle != null && !magicCircle.isPlaying)
            {
               magicCircle.Play();
            }

            if(trigger != null)
            {
               trigger.Play();
            }

            if(Door != null)
            {
               Door.OpenDoor();
            }
        }
        
        // Pressure plate goes back up, closing the door
        else if (puzzleSolved && curWeight < requiredWeight)
        {
            puzzleSolved = false;
            if (magicCircle != null && magicCircle.isPlaying)
            {
                magicCircle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            if (Door != null)
            {
                Door.CloseDoor();
            }

            if (trigger != null){
                trigger.Stop();
            }
        }
    }
}


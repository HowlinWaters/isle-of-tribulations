using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTrigger : MonoBehaviour
{
    public SequenceTile st;
    [SerializeField] private AudioSource stepontile;

    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger: " + gameObject.name);

            if (st != null)
            {
                st.Pressed();
                if(stepontile != null){
                    stepontile.Play();
                }
                Debug.Log("It is pressed");
            }
            else
            {
                Debug.Log("SequenceTile reference is NULL on " + gameObject.name);
            }
        }
    }
}
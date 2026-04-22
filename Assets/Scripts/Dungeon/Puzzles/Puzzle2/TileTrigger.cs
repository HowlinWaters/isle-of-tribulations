using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTrigger : MonoBehaviour
{
    public SequenceTile st;

    private void OnTriggerEnter(Collider c){
        if(c.CompareTag("Player")){
            st.Pressed();
        }
    }
}

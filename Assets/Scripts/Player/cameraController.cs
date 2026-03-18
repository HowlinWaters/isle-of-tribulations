using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(offset);
        //compute initial offset between the camera position and player current position
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if(player != null){
        //maintatin sameoffest between player and camera
        //camera will keep following player with the same offset;
            transform.position = player.transform.position + offset;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offset = new(0, 0, -15f);
    [SerializeField] private float transitionSpeed = 8f;
    [SerializeField] private Transform startRoom;

    private PlayMode playMode;
    private bool isTransitioning = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"This camera: {gameObject.name}, Position: {transform.position}");
        BoxCollider startCollider = startRoom.GetComponent<BoxCollider>();
        playMode = player.GetComponent<PlayMode>();
        offset = transform.position - startCollider.bounds.center;
    }

    public void ShiftToRoom(Bounds roomBounds)
    {
        if (!isTransitioning)
            StartCoroutine(SlideToRoom(roomBounds));
    }

    IEnumerator SlideToRoom(Bounds roomBounds)
    {
        isTransitioning = true;
        
        playMode.LockMovement();

        Vector3 destination = new(
            roomBounds.center.x + offset.x,
            transform.position.y,
            roomBounds.center.z + offset.z
        );
        Vector3 rawDirection = destination - transform.position;
        Vector3 direction;
        
        Debug.Log($"Room center: {roomBounds.center}, Destination: {destination}");

        if (Mathf.Abs(rawDirection.x) > Mathf.Abs(rawDirection.z))
        {
           direction = rawDirection.x > 0 ? Vector3.right : Vector3.left; 
        } 
        else
        {
            direction = rawDirection.z > 0 ? Vector3.forward : Vector3.back;
        }

        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, destination, transitionSpeed * Time.deltaTime);
            yield return null;
        }
        
        transform.position = destination;
        
        Vector3 playerEntry = Vector3.zero;

        if (direction == Vector3.forward) playerEntry = new(player.transform.position.x, player.transform.position.y, roomBounds.min.z + 1f);
        else if (direction == Vector3.back) playerEntry = new(player.transform.position.x, player.transform.position.y, roomBounds.max.z - 1f);
        else if (direction == Vector3.right) playerEntry = new(roomBounds.min.x + 1f, player.transform.position.y, player.transform.position.z);
        else if (direction == Vector3.left) playerEntry = new(roomBounds.max.x - 1f, player.transform.position.y, player.transform.position.z);
        
        player.transform.position = playerEntry;
        
        playMode.UnlockMovement();

        isTransitioning = false;
    }
}

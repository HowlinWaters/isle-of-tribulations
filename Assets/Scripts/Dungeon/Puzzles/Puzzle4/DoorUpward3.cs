using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUpward3 : MonoBehaviour
{
    [SerializeField] private Vector3 openOffset = new Vector3(0f, 4f, 0f);
    [SerializeField] private float openSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;

    [SerializeField] private ParticleSystem left;
    [SerializeField] private ParticleSystem right;
    [SerializeField] private GameObject closeDoorFX;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
    }

    void Update()
    {
        if (isOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPosition, openSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, openSpeed * Time.deltaTime);
        }
    }

    public void OpenDoor()
    {
        if(isOpen) return;

        isOpen = true;
        if(left != null && right != null){
            left.Play();
            right.Play();
        } 
        if(closeDoorFX != null){
            closeDoorFX.SetActive(false);
        }
    }

    public void CloseDoor()
    {
        if(!isOpen) return;
        isOpen = false;
        if(closeDoorFX != null){
            closeDoorFX.SetActive(true);
        }
    }
}

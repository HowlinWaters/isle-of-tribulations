using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pause))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float transitionSpeed = 8f;
    [SerializeField] private Transform startRoom;

    [Header("Rotation")]
    [SerializeField] private float rotateStep = 90f;
    [SerializeField] private float rotateSpeed = 5f;

    private Pause status;
    private bool isTransitioning = false;

    private float currentYaw = 0f;
    private float targetYaw = 0f;

    private Vector3 currentRoomCenter;
    private Quaternion initialRotation;

    void Start()
    {
        BoxCollider startCollider = startRoom.GetComponent<BoxCollider>();
        offset = transform.position - startCollider.bounds.center;
        status = GetComponent<Pause>();

        currentRoomCenter = startCollider.bounds.center;
        initialRotation = transform.rotation;

        currentYaw = 0f;
        targetYaw = 0f;
    }

    void Update()
    {
        if (isTransitioning) return;

        // Rotate in steps
        if (Input.GetKeyDown(KeyCode.Q))
        {
            targetYaw -= rotateStep;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            targetYaw += rotateStep;
        }

        // Smooth rotation
        currentYaw = Mathf.LerpAngle(currentYaw, targetYaw, rotateSpeed * Time.deltaTime);
        Quaternion rotation = Quaternion.Euler(0f, currentYaw, 0f);
        Vector3 rotatedOffset = rotation * offset;
        transform.SetPositionAndRotation(currentRoomCenter + rotatedOffset, rotation * initialRotation);
    }

    public void ShiftToRoom(Bounds roomBounds)
    {
        if (!isTransitioning)
            StartCoroutine(SlideToRoom(roomBounds));
    }

    IEnumerator SlideToRoom(Bounds roomBounds)
    {
        isTransitioning = true;

        status.PauseGame();

        currentRoomCenter = roomBounds.center;

        Quaternion rotation = Quaternion.Euler(0f, currentYaw, 0f);
        Vector3 rotatedOffset = rotation * offset;
        Vector3 destination = currentRoomCenter + rotatedOffset;

        Vector3 rawDirection = destination - transform.position;
        Vector3 direction;

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
            transform.position = Vector3.Lerp(
                transform.position,
                destination,
                transitionSpeed * Time.unscaledDeltaTime
            );
            yield return null;
        }

        transform.position = destination;

        Vector3 playerEntry = Vector3.zero;

        if (direction == Vector3.forward)
            playerEntry = new(player.transform.position.x, player.transform.position.y, roomBounds.min.z + 1f);
        else if (direction == Vector3.back)
            playerEntry = new(player.transform.position.x, player.transform.position.y, roomBounds.max.z - 1f);
        else if (direction == Vector3.right)
            playerEntry = new(roomBounds.min.x + 1f, player.transform.position.y, player.transform.position.z);
        else if (direction == Vector3.left)
            playerEntry = new(roomBounds.max.x - 1f, player.transform.position.y, player.transform.position.z);

        player.transform.position = playerEntry;

        status.ResumeGame();

        isTransitioning = false;
    }
}
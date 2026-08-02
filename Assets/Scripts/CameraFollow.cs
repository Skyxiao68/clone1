using System;
using UnityEngine;

using UnityEngine;

public class CameraRoomFollow : MonoBehaviour
{
    public Transform player;

    public float roomWidth = 16f;
    public float roomHeight = 9f;

    public float moveSpeed = 8f;

    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
    }

    void LateUpdate()
    {
        int roomX = Mathf.RoundToInt(player.position.x / roomWidth);
        int roomY = Mathf.RoundToInt(player.position.y / roomHeight);

        targetPosition = new Vector3(
            roomX * roomWidth,
            roomY * roomHeight,
            transform.position.z);

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime);
    }
}
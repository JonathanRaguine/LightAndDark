using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlatformMovement : NetworkBehaviour
{
    public enum MovementDirection { Up, Down, Left, Right };
    public MovementDirection moveDirection = MovementDirection.Up;
    public float moveSpeed = 5f;
    public float distanceToStop = 10f; // Distance the platform should travel before stopping
    private bool isMoving = false;
    private Vector2 initialPosition;
    private float distanceMoved = 0f;

    public void Move(bool activate)
    {
        isMoving = activate;
        if (activate)
        {
            Debug.Log("activating platform");
            // Store the initial position of the platform when movement starts
            initialPosition = transform.position;
            distanceMoved = 0f;
        }
    }

    private void Update()
    {
        if (isMoving)
        {   
            
            // Calculate movement direction based on the enum value
            Vector2 direction = Vector2.zero;
            switch (moveDirection)
            {
                case MovementDirection.Up:
                    direction = Vector2.up;
                    break;
                case MovementDirection.Down:
                    direction = Vector2.down;
                    break;
                case MovementDirection.Left:
                    direction = Vector2.left;
                    break;
                case MovementDirection.Right:
                    direction = Vector2.right;
                    break;
            }

            // Move the platform in the specified direction at the specified speed
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            // Calculate the distance moved since movement started
            distanceMoved = Vector2.Distance(transform.position, initialPosition);

            // If the platform has moved the specified distance, stop moving
            if (distanceMoved >= distanceToStop)
            {
                isMoving = false;
            }
        }
    }
}

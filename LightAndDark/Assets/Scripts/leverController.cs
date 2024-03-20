using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class leverController : NetworkBehaviour
{
    [System.Serializable]
    public struct PlatformData
    {
        public GameObject platform;
        public Vector2 initialPosition;
        public Vector2 endPoint;
        public float moveSpeed;
    }

    public PlatformData[] platformsToControl;
    private bool isActivated = false;

    private void Update()
    {
        if (isActivated)
        {
            foreach (PlatformData platformData in platformsToControl)
            {
                GameObject platform = platformData.platform;
                Vector2 endPoint = platformData.endPoint;
                float movementSpeed = platformData.moveSpeed;

                // Move the platform towards the target position
                platform.transform.position = Vector2.MoveTowards(platform.transform.position, endPoint,
                    movementSpeed * Time.deltaTime);
            }
        }
        else
        {
            foreach (PlatformData platformData in platformsToControl)
            {
                GameObject platform = platformData.platform;
                Vector2 initialPosition = platformData.initialPosition;
                float movementSpeed = platformData.moveSpeed;

                // Move the platform towards the initial position
                platform.transform.position = Vector2.MoveTowards(platform.transform.position, initialPosition,
                    movementSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActivated = !isActivated;

        }
    }

    /*[ServerRpc]
    private void UpdateLeverStateServerRpc(bool activated)
    {
        isActivated = activated;
        UpdateLeverStateClientRpc(activated);
    }

    [ClientRpc]
    private void UpdateLeverStateClientRpc(bool activated)
    {
        if (IsLocalPlayer) return;
        isActivated = activated;
    }*/
}

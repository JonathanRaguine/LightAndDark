using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class leverController : NetworkBehaviour
{
    public GameObject prefab; // Reference to the platform to control
    public bool isActivated;


    public void Interact(bool activate)
    {
        isActivated = !isActivated;
        Debug.Log("activated lever");

        // Check if the platform reference is not null before trying to control it
        PlatformMovement platform = prefab.GetComponent<PlatformMovement>();
        platform.Move(true);
        UpdateLeverStateServerRpc(isActivated);
    }

    [ServerRpc]
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
    }
}

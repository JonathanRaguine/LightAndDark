using System;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

public class elementController : NetworkBehaviour
{
    public GameObject prefab;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (!NetworkManager.Singleton.IsServer) return;
        NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, prefab);
        NetworkObject.Despawn();
    }
}

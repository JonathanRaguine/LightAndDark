using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class ObjectSpawner : NetworkBehaviour
{
    public GameObject PrefabToSpawn;
    private NetworkObject SpawnedNetworkObject;
    
    [SerializeField] private List<Vector2> spawnPositions = new List<Vector2>();

    private void Start()
    {
        enabled = IsServer;
        if (!enabled || PrefabToSpawn == null)
        {
            return;
        }
        
        foreach (Vector2 spawnPosition in spawnPositions)
        {
            GameObject prefabInstance = Instantiate(PrefabToSpawn, spawnPosition, Quaternion.identity);
            SpawnedNetworkObject = prefabInstance.GetComponent<NetworkObject>();
            SpawnedNetworkObject.Spawn();
        }
    }
    
    public void DespawnObject(GameObject objectToDespawn)
    {
        if (IsServer && SpawnedNetworkObject.IsSpawned)
        {
            var networkObject = objectToDespawn.GetComponent<NetworkObject>();
            networkObject.Despawn(true);
        }
    }
    
}
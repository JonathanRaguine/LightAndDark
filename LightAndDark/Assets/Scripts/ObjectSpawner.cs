using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class ObjectSpawner : NetworkBehaviour
{   
    
    public GameObject PrefabToSpawn;
    private NetworkObject SpawnedNetworkObject;
    public static ObjectSpawner Instance;
    [SerializeField] private List<Vector2> spawnPositions = new List<Vector2>();
    
    private void Awake()
    {
        Instance = this; // Set the singleton instance reference
    }
    
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
    
    [ServerRpc(RequireOwnership = false)]
    public void DespawnObjectServerRpc(ulong objectId)
    {
        if (IsServer && SpawnedNetworkObject.IsSpawned)
        {
            NetworkObject networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectId];
            networkObject.Despawn(true);
        }
    }
    
    
}
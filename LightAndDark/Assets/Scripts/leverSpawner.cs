using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class leverSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private List<Vector2> spawnPositions;
    
    private const int MaxPrefabCount = 10;
    // Start is called before the first frame update
    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += SpawnDropStart;
    }

    private void SpawnDropStart()
    {
        if (!NetworkManager.Singleton.IsServer) return;
        NetworkManager.Singleton.OnServerStarted -= SpawnDropStart;
        NetworkObjectPool.Singleton.OnNetworkSpawn();

        foreach (Vector2 position in spawnPositions)
        {
            if (NetworkObjectPool.Singleton.GetCurrentPrefabCount(prefab) < MaxPrefabCount)
            {
                SpawnDrop(position);
            }
        }
    }

    private void SpawnDrop(Vector2 position)
    {
        NetworkObject obj = NetworkObjectPool.Singleton.GetNetworkObject(prefab, position, Quaternion.identity);

        obj.GetComponent<leverController>().prefab = prefab;
        if(!obj.IsSpawned) obj.Spawn(true);
    }
}

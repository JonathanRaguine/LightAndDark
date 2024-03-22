using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

public class elementController : NetworkBehaviour
{
    public GameObject prefab;
    public static int score = 0;
    //trigger for when player touches coin to despawn
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (!NetworkManager.Singleton.IsServer) return;
        NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, prefab);
        NetworkObject.Despawn();
        IncrementScore();
    }

    public void IncrementScore()
    {
        score++;
        print(score);
    }
    
    public static int GetTotalScore()
    {
        return score;
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class ScoreUI : NetworkBehaviour
{
    public TextMeshProUGUI scoreText;
    void Update()
    {
        if (IsServer)
        {
            UpdateScoreServerRpc();
        }
    }


    [ServerRpc]
    void UpdateScoreServerRpc()
    {
        int score = elementController.GetTotalScore();
        scoreText.text = score.ToString();
        UpdateScoreClientRpc(score);
    }

    [ClientRpc]
    void UpdateScoreClientRpc(int score)
    {
        scoreText.text = score.ToString();

    }
}

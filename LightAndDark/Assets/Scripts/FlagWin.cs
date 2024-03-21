using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class FlagWin : NetworkBehaviour
{
    public int flagNum = 0;
    [SerializeField] private GameObject winUI;
    public AudioSource victoryAudio;
    private bool canWin = false;
    private bool hasWon = false;

    private void Update()
    {
        if (flagNum == 2)
        {
            canWin = true;
            CanWin();
            hasWon = true;
        }

    }

    private void CanWin()
    {
        if (canWin && !hasWon)
        {
            winUI.SetActive(true);
            victoryAudio.Play();
        }
    }
}

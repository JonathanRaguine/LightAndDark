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
        //condition to win game
        if (flagNum == 2)
        {
            canWin = true;
            CanWin();
            hasWon = true;
        }

    }
    
    //uses hasWon boolean since sound wouldnt play without it
    private void CanWin()
    {
        if (canWin && !hasWon)
        {
            winUI.SetActive(true);
            victoryAudio.Play();
        }
    }
}

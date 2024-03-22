using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagCheck : MonoBehaviour
{
    [SerializeField] private FlagWin flagWin;
    public AudioSource flagAudio;
    private void Start()
    {
        flagWin = GetComponentInParent<FlagWin>();
    }
    
    
    //counts each time a play has reached the flag, needs 2 to win
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            flagWin.flagNum++;
            flagAudio.Play();
        }
    }
}

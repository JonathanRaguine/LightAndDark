using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class leverController : NetworkBehaviour
{
    public AudioSource leverAudio;
    public AudioSource platformAudio;
    [System.Serializable]
    public struct PlatformData
    {
        public GameObject platform;
        public Vector2 initialPosition;
        public Vector2 endPoint;
        public float moveSpeed;
    }

    public PlatformData[] platformsToControl;
    private bool isActivated = false;
    private bool isMoving = false;
    

    private void Update()
    {
        if (!IsServer && !IsClient) return;
        foreach (PlatformData platformData in platformsToControl)
        {
            GameObject platform = platformData.platform;
            Vector2 initialPoint = platformData.initialPosition;
            Vector2 endPoint = platformData.endPoint;
            Vector2 position = platform.transform.position;
            

            if (isActivated)
            {
                platform.transform.position = Vector2.MoveTowards(platform.transform.position, endPoint, Time.deltaTime);

            }
            else
            {
                platform.transform.position = Vector2.MoveTowards(platform.transform.position, initialPoint, Time.deltaTime);
            }

            if (Vector2.Distance(position, initialPoint) < 0.1f || Vector2.Distance(position, endPoint) < 0.01f)
            {
                platformAudio.Stop();
            }
            Debug.Log(Vector2.Distance(position, initialPoint));
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActivated = !isActivated;
            leverAudio.Play();
            StartCoroutine(PlatformAudioDelay());

        }
    }
    private IEnumerator PlatformAudioDelay()
    {
        yield return new WaitForSeconds(0.25f);
        platformAudio.Play();
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class elementController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            ObjectSpawner objectSpawner = col.gameObject.GetComponentInParent<ObjectSpawner>();
            objectSpawner.DespawnObject(col.gameObject);
        }
    }
}

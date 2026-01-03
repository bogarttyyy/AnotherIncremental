using System;
using System.Collections;
using System.Collections.Generic;
using NSBLib.Helpers;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    [SerializeField] List<GameObject> objects = new List<GameObject>();
    
    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        NSBLogger.Log("Hello every 5secs");
        yield return new WaitForSeconds(5f);
    }
}

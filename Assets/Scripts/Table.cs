using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NSBLib.Helpers;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] GameObject[] objs;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform slotGroup;
    [SerializeField] private bool hasGuide;
    private List<Transform> slotList;

    private void OnEnable()
    {
        SetupSlots();
        objs = new GameObject[slotList.Count];
    }

    private void SetupSlots()
    {
        SetupVisualGuide();
        
        var transforms = slotGroup.GetComponentsInChildren<Transform>();
        var slotTransforms = transforms.ToList();
        // Remove first transform since it's the transform of the table
        slotTransforms.RemoveAt(0);
        slotList = slotTransforms;

        NSBLogger.Log($"{slotList.Count}");
    }

    private void SetupVisualGuide()
    {
        if (!hasGuide)
        {
            var renderers = slotGroup.GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in renderers)
            {
                r.enabled = false;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // StartCoroutine(SpawnTableEnvelopes());
    }

    IEnumerator SpawnTableEnvelopes()
    {
        for (int i = 0; i < objs.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            var newEnv =  Instantiate(prefab, slotList[i], true);
            newEnv.transform.position = new  Vector3(slotList[i].transform.position.x, slotList[i].transform.position.y, slotList[i].transform.position.z -1);
            
            objs[i] = newEnv;
        }
    }

    public List<Transform> GetSlotList()
    {
        return slotList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

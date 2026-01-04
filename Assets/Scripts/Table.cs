using System;
using System.Collections.Generic;
using System.Linq;
using NSBLib.Helpers;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] Envelope[] envelopes;
    private List<Transform> slotList;

    private void OnEnable()
    {
        SetupSlots();
        envelopes = new Envelope[slotList.Count];
    }

    private void SetupSlots()
    {
        var transforms = GetComponentsInChildren<Transform>();
        var slotTransforms = transforms.ToList();
        // Remove first transform since it's the transform of the table
        slotTransforms.RemoveAt(0);
        slotList = slotTransforms;

        NSBLogger.Log($"{slotList.Count}");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

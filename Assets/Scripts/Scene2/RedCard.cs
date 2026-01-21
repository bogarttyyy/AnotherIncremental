using System;
using NSBLib.EventChannelSystem;
using NSBLib.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RedCard : MonoBehaviour
{
    [SerializeField] private Image cooldownImage;

    [SerializeField] private IntEventChannel OnAddPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnClicked()
    {
        OnAddPoints.Invoke(1);
    }

    public void UpdateCooldownUI(float p)
    {
        cooldownImage.fillAmount = p;
    }
}

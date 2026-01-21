using System;
using NSBLib.EventChannelSystem;
using NSBLib.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RedCard : MonoBehaviour, IClickable
{

    [SerializeField] private float timer;
    [SerializeField] private float duration = 5f;
    
    [SerializeField] private Image cooldownImage;

    // private float percent = 0f;
    private bool cooldownDone = true;

    [SerializeField] private IntEventChannel OnAddPoints;
    [SerializeField] private EventChannel<GameObject> OnCooldownFinished;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateCooldownUI(timer / duration);

            if (timer <= 0)
            {
                timer = 0;
                OnCooldownFinished?.Invoke(gameObject);
                cooldownDone = true;
            }
            else
            {
                cooldownDone = false;
            }
        }
    }

    public void OnClicked()
    {
        Debug.Log("Clicked");
        if (cooldownDone)
        {
            OnAddPoints.Invoke(1);
            SetCooldown();
        }
    }
    
    private void SetCooldown()
    {
        Debug.Log("SetCooldown");
        cooldownDone = false;
        timer = duration;
    }

    private void UpdateCooldownUI(float p)
    {
        cooldownImage.fillAmount = p;
    }
}

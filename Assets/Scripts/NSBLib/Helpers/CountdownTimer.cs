using System;
using NSBLib.EventChannelSystem;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private float currentTime;
    [SerializeField] private FloatEventChannel updateTime;
    [SerializeField] private float duration = 60f;

    private bool hasStarted = false;

    private void FixedUpdate()
    {
        if (currentTime > 0 && hasStarted)
        {
            currentTime -= Time.deltaTime;
            updateTime?.Invoke(currentTime / duration);

            if (currentTime <= 0)
            {
                currentTime = 0;
                updateTime?.Invoke(0f);
            }
        }
    }

    public void StartTimer()
    {
        hasStarted = true;
        currentTime = duration;
    }
}

using NSBLib.EventChannelSystem;
using NSBLib.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace NSBLib.Components
{
    [RequireComponent(typeof(Collider2D))]
    public class ClickableWithCooldown: MonoBehaviour, IClickable
    {
        [SerializeField] private float timer;
        [SerializeField] private float duration = 5f;
        public float percent = 0f;

        [SerializeField] private UnityEvent OnClick;
        [SerializeField] private UnityEvent<float> OnTimerUpdate;

        // private float percent = 0f;
        private bool cooldownDone = true;
        
        [SerializeField] private EventChannel<GameObject> OnCooldownFinished;
        
        private void FixedUpdate()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                percent = timer / duration;
                OnTimerUpdate?.Invoke(percent);

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
                SetCooldown();
                OnClick?.Invoke();
            }
        }
    
        private void SetCooldown()
        {
            Debug.Log("SetCooldown");
            cooldownDone = false;
            timer = duration;
        }
    }
}
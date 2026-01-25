using NSBLib.EventChannelSystem;
using UnityEngine;
using UnityEngine.Events;

namespace NSBLib.Components
{
    public class CountTimer : MonoBehaviour
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
    }
}
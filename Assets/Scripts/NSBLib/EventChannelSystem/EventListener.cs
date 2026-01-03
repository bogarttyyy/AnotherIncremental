using System;
using UnityEngine;
using UnityEngine.Events;

namespace NSBLib.EventChannelSystem
{
    public abstract class EventListener<T> : MonoBehaviour
    {
        [SerializeField] private EventChannel<T> eventChannel;
        [SerializeField] private UnityEvent<T> unityEvent;

        protected void OnEnable()
        {
            eventChannel.Register(this);
        }

        protected void OnDisable()
        {
            eventChannel.Unregister(this);
        }

        public void Raise(T value)
        {
            unityEvent?.Invoke(value);
        }
    }
    
    public class EventListener : EventListener<Empty> { }
}
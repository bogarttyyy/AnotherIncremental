using NSBLib.EventChannelSystem;
using UnityEngine;

namespace EventChannels
{
    [CreateAssetMenu(fileName = "CardEventChannel", menuName = "Events/CardEventChannel")]
    public class CardEventChannel : EventChannel<Card> {}
}
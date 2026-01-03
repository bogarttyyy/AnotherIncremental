using UnityEngine;

namespace NSBLib.EventChannelSystem
{
    [CreateAssetMenu(fileName = "StringEventChannel", menuName = "Events/StringEventChannel")]
    public class StringEventChannel : EventChannel<string> {}
}
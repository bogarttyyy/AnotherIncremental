using UnityEngine;

namespace NSBLib.EventChannelSystem
{
    [CreateAssetMenu(fileName = "SpriteEventChannel", menuName = "Events/SpriteEventChannel")]
    public class SpriteEventChannel : EventChannel<Sprite> {}
}
using NSBLib.Enums;
using UnityEngine;

namespace NSBLib.EventChannelSystem
{
    [CreateAssetMenu(fileName = "GameStateEventChannel", menuName = "Events/GameStateEventChannel")]
    public class GameStateEventChannel : EventChannel<EGameState> {}
}
using Enums;
using UnityEngine;

namespace NSBLib.EventChannelSystem
{
    [CreateAssetMenu(fileName = "BigEnvelopeEventChannel", menuName = "Events/BigEnvelopeEventChannel")]
    public class BigEnvelopeEventChannel : EventChannel<BigEnvelope> {}
}
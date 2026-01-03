using Enums;
using UnityEngine;

namespace NSBLib.EventChannelSystem
{
    [CreateAssetMenu(fileName = "AddressEventChannel", menuName = "Events/AddressEventChannel")]
    public class AddressEventChannel : EventChannel<Address> {}
}
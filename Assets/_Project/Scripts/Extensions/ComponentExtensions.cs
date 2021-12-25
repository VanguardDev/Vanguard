using FishNet.Object;
using UnityEngine;

namespace Vanguard.Extensions
{
    public static class ComponentExtensions
    {
        public static bool IsLocalPlayer(this Component gameObject)
        {
            return gameObject.GetComponentInParent<NetworkObject>().IsOwner;
        }
    }
}
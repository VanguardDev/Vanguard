using Mirror;
using UnityEngine;

namespace Vanguard
{
    public static class VanguardUtilities
    {
        public static bool IsDedicatedServer { get => NetworkServer.active && !NetworkClient.active; }
    }
}
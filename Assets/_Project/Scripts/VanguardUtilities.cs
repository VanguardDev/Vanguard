using Mirror;
using UnityEngine;
//TODO
namespace Vanguard
{
    public static class VanguardUtilities
    {
        public static bool IsDedicatedServer { get => NetworkServer.active && !NetworkClient.active; }
    }
}
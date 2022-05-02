using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

namespace Vanguard
{
    public class OutOfBounds : NetworkBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            if (base.IsServer) other.GetComponent<Health>().getShot(100,-1);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class OutOfBounds : NetworkBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (isServer) other.GetComponent<Health>().getShot(100);
    }
}

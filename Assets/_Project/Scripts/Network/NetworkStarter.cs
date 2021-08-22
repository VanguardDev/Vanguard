using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkStarter : MonoBehaviour
{
    NetworkManager nm;
    public void Start()
    {
        if (Application.isBatchMode) return;
        nm = GetComponent<NetworkManager>();
        if (ConnectionInfo.Host)nm.StartHost();
        else
        {
            nm.networkAddress = ConnectionInfo.ip;
            nm.StartClient();
        }
    }
}

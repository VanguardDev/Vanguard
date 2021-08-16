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
        if (ParamaterPasss.Host)nm.StartHost();
        else
        {
            nm.networkAddress = ParamaterPasss.ip;
            nm.StartClient();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Net;
public class NetworkStarter : MonoBehaviour
{
    NodeListServer.NodeListServerNetManager nm;
    public void Start()
    {

        string externalIP = new WebClient().DownloadString("http://icanhazip.com").Trim('\n'); // could be the cause of process leak ?
        if (Application.isBatchMode) return;
        nm = GetComponent<NodeListServer.NodeListServerNetManager>();
        if (ConnectionInfo.Mode == 1) nm.StartHost();
        else if (ConnectionInfo.Mode == 2) nm.StartServer();
        else
        {
            if (externalIP == ConnectionInfo.ip) ConnectionInfo.ip = "localhost";
            Debug.Log(ConnectionInfo.ip == externalIP);
            Debug.Log(externalIP);
            nm.networkAddress = ConnectionInfo.ip;
            nm.StartClient();
        }
    }
}
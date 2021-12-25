using System.Collections;
using System.Collections.Generic;
using FishNet.Managing;
using UnityEngine;
using System.Net;
public class NetworkStarter : MonoBehaviour
{
    // NodeListServer.NodeListServerNetManager nm;
    [SerializeField]
    private NetworkManager _networkManager;

    public void Start()
    {
        if (!ConnectionInfo.isOpenedFromMenu)
        {
            // FindObjectOfType<NetworkManagerHUD>().enabled = true;
            return;
        }

        if(_networkManager == null) return;

        string externalIP = new WebClient().DownloadString("http://icanhazip.com").Trim('\n'); // could be the cause of process leak ?
        if (Application.isBatchMode) return;

        switch (ConnectionInfo.mode)
        {
            case ConnectionInfo.Mode.Client:
            default: //By default is client connection
                Debug.Log("Client Connection...");
                _networkManager.TransportManager.Transport.SetClientAddress(ConnectionInfo.connectionIp.Trim());
                _networkManager.ClientManager.StartConnection();
                Debug.Log($"Connected to {_networkManager.TransportManager.Transport.GetClientAddress()}:{_networkManager.TransportManager.Transport.GetPort()}");
            break;
            case ConnectionInfo.Mode.Host:
                Debug.Log("Hosting...");
                _networkManager.TransportManager.Transport.SetServerBindAddress("localhost");
                _networkManager.ServerManager.StartConnection();
                _networkManager.TransportManager.Transport.SetClientAddress("localhost");
                _networkManager.ClientManager.StartConnection();
                Debug.Log($"Hosting on: {_networkManager.TransportManager.Transport.GetServerBindAddress()}:{_networkManager.TransportManager.Transport.GetPort()}");
            break;
            case ConnectionInfo.Mode.DedicatedServer:
                Debug.Log("Dedicated Server...");
                _networkManager.TransportManager.Transport.SetServerBindAddress("localhost");
                _networkManager.ServerManager.StartConnection();
                Debug.Log($"Addres: {_networkManager.TransportManager.Transport.GetServerBindAddress()}:{_networkManager.TransportManager.Transport.GetPort()}");
            break;
        }

        // nm = GetComponent<NodeListServer.NodeListServerNetManager>();
        // if (ConnectionInfo.Mode == 1){
        //     _networkManager.ServerManager.StartConnection(); // nm.StartHost();
        //     _networkManager.ClientManager.StartConnection();
        // }
        // else if (ConnectionInfo.Mode == 2) _networkManager.ServerManager.StartConnection();
        // else{
        //     Client

        //     if (externalIP == ConnectionInfo.ip) ConnectionInfo.ip = "localhost";
        //     Debug.Log(ConnectionInfo.ip == externalIP);
        //     Debug.Log(externalIP);
        //     nm.networkAddress = ConnectionInfo.ip;
        //     nm.StartClient();
            
        //     _networkManager.ClientManager.StartConnection();
        // }

    }
}
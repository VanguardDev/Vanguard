using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
namespace NodeListServer
{
    public class NodeListServerCommunicationManager : MonoBehaviour
    {
        public static NodeListServerCommunicationManager Instance;

        // You can modify this data.
        public ServerInfo CurrentServerInfo = new ServerInfo()
        {
            Name = "Untitled Server",
            Port = 7777,
            PlayerCount = 0,
            PlayerCapacity = 0,
            ExtraInformation = string.Empty
        };
        bool serverListed;
        private const string AuthKey = "NodeListServerDefaultKey";

        // Change this to your NodeLS Server instance URL.
        private const string Server = "http://projectvanguard.uk.to:8889";

        // Don't modify, this is randomly generated.
        private string InstanceServerId = string.Empty;

        private void Awake()
        {
            // If singleton was somehow loaded twice...
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Duplicate NodeLS Communication Manager detected in scene. This one will be destroyed.");
                Destroy(this);
                return;
            }

            Instance = this;

            // Generate a new identification string

            print("NodeLS Communication Manager Initialized.");
        }
        public void AddUpdateServerEntry()
        {
            StartCoroutine(nameof(AddUpdateInternal));
        }

        public void RemoveServerEntry()
        {
            StartCoroutine(nameof(RemoveServerInternal));
        }

        // Internal things
        private IEnumerator AddUpdateInternal()
        {
            WWWForm serverData = new WWWForm();
            print("NodeLS Communication Manager: Adding/Updating Server Entry");

            serverData.AddField("serverKey", AuthKey);
            CurrentServerInfo.Name = ConnectionInfo.name;
            if (serverListed) serverData.AddField("serverUuid", InstanceServerId);
            serverData.AddField("serverName", CurrentServerInfo.Name);
            serverData.AddField("serverPort", CurrentServerInfo.Port);
            serverData.AddField("serverPlayers", CurrentServerInfo.PlayerCount);
            serverData.AddField("serverCapacity", CurrentServerInfo.PlayerCapacity);
            serverData.AddField("serverExtras", CurrentServerInfo.ExtraInformation);
            Debug.Log(InstanceServerId);
            using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Post(Server + "/add", serverData))
            {

                yield return www.SendWebRequest();
                Debug.Log(Encoding.ASCII.GetString(www.downloadHandler.data));
                if (!serverListed) InstanceServerId = Encoding.ASCII.GetString(www.downloadHandler.data);
                if (www.responseCode == 200)
                {
                    print("Successfully registered server with the NodeListServer instance!");
                    serverListed = true;
                }
                else
                {
                    Debug.LogError($"Failed to register the server with the NodeListServer instance: {www.error}");
                }
            }

            yield break;
        }

        private IEnumerator RemoveServerInternal()
        {
            WWWForm serverData = new WWWForm();
            print("NodeLS Communication Manager: Removing Server Entry");

            // Assign all the fields required.
            serverData.AddField("serverKey", AuthKey);
            serverData.AddField("serverUuid", InstanceServerId);

            using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Post(Server + "/remove", serverData))
            {
                yield return www.SendWebRequest();

                if (www.responseCode == 200)
                {
                    print("Successfully deregistered server with the NodeListServer instance!");
                }
                else
                {
                    Debug.LogError($"Failed to deregister the server with the NodeListServer instance: {www.error}");
                }
            }

            yield break;
        }
    }

}

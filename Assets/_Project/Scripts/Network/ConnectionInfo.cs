using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ConnectionInfo : MonoBehaviour
{
    public enum Mode{
        Client,
        Host,
        DedicatedServer
    }
    public static string ip = "projectvanguard.uk.to", name = "Name Not Set.";
    public static string connectionIp;
    public static Mode mode;
    public static bool isOpenedFromMenu;
    public void setClient(string sceneName)
    {
        mode = Mode.Client;
        isOpenedFromMenu = true;
        GetComponent<SceneController>().LoadScene(sceneName);
    }
    public void setIp(string ipNew)
    {
        ip = ipNew;
    }
    public void setName(string nameNew)
    {
        name = nameNew;
    }
    public void setConnectionIP(string text){
        connectionIp = text;
    }
    public void setHost(string sceneName)
    {
        mode = Mode.Host;
        isOpenedFromMenu = true;
        GetComponent<SceneController>().LoadScene(sceneName);
    }
    public void setDedicatedServer(string sceneName)
    {
        isOpenedFromMenu = true;
        mode = Mode.DedicatedServer;
        GetComponent<SceneController>().LoadScene(sceneName);
    }
}

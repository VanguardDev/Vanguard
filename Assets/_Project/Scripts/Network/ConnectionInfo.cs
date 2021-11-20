using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionInfo : MonoBehaviour
{
    public static string ip = "projectvanguard.uk.to", name = "Name Not Set.";
    public static int Mode=0;
    public static bool isOpenedFromMenu;
    public void setClient()
    {
        Mode = 0;
        isOpenedFromMenu = true;
    }
    public void setIp(string ipNew)
    {
        ip = ipNew;
    }
    public void setName(string nameNew)
    {
        name = nameNew;
    }
    public void setHost(string sceneName)
    {
        Mode= 1;
        isOpenedFromMenu = true;
        GetComponent<SceneController>().LoadScene(sceneName);
    }
    public void setDedicatedServer(string sceneName)
    {
        isOpenedFromMenu = true;
        Mode = 2;
        GetComponent<SceneController>().LoadScene(sceneName);
    }
}

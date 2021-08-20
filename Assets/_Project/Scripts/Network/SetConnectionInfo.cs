using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetConnectionInfo : MonoBehaviour
{
    public void setIp(string ipNew)
    {
        ConnectionInfo.ip = ipNew;
    }
    public void setName(string nameNew)
    {
        ConnectionInfo.name = nameNew;
    }
    public void setHost(string sceneName)
    {
        ConnectionInfo.Host= true;
        GetComponent<SceneController>().LoadScene(sceneName);
    }
}

public static class ConnectionInfo
{

   public  static string ip="projectvanguard.uk.to", name= "Name Not Set";
    public static bool Host;
    
}

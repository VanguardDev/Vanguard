using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableController : MonoBehaviour
{
    public void ChangeIp(string newIp)
    {
        ConnectionInfo.ip = newIp;
    }
    public void ChangeName(string newName)
    {
        ConnectionInfo.name = newName;
    }
    public void SetHost(string sceneName)
    {
        ConnectionInfo.Host = true;
        GetComponent<SceneController>().LoadScene(sceneName);
    }
}
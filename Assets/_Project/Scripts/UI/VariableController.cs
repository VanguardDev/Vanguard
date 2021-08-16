using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableController : MonoBehaviour
{
    public void ChangeIp(string newIp)
    {
        ParamaterPasss.ip = newIp;
    }
    public void ChangeName(string newName)
    {
        ParamaterPasss.name = newName;
    }
    public void SetHost(string sceneName)
    {
        ParamaterPasss.Host = true;
        GetComponent<SceneController>().LoadScene(sceneName);
    }
}
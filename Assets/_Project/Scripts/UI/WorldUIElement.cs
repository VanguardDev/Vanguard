using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class WorldUIElement : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject camera;
    void Awake()
    {
        if (NetworkServer.active && !NetworkClient.active)enabled = false;
        
    }
    public void Start()
    {
         camera = Camera.main.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.transform);
    }
}

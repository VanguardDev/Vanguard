using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WorldUIElement : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject camera;
    void Start()
    {
        if (Application.isBatchMode) enabled = false;
        camera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bilboard : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject camera;
    void Start()
    {
        camera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.transform);
    }
}

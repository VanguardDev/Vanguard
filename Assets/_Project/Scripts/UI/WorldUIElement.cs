using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Vanguard.UI
{
    public class WorldUIElement : MonoBehaviour
    {
        GameObject camera;

        void Awake()
        {
            if (VanguardUtilities.IsDedicatedServer)
            {
                enabled = false;
            }

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
}
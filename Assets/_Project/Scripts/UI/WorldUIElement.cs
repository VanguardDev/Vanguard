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
        public void setCamera(GameObject cam)
        {
            camera = cam;
        }
        // Update is called once per frame
        void Update()
        {
           if(camera) transform.LookAt(camera.transform);
        }
    }
}
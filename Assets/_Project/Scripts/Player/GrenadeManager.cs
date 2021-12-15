using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//TODO: Maybe implement a grenade ready animation (animation or something gis played when grenade key is held down)


// Equivalent to PlayerGunManager but for grenades. no weapon object is needed

namespace Vanguard
{
    public class GrenadeManager : NetworkBehaviour
    {
        public GameObject grenadePrefab;
         Rigidbody playerRigdbody;
        // Start is called before the first frame update
        void Start()
        {
            if (!isLocalPlayer)
            {
                
                enabled = false;
            }
            else
            {
                playerRigdbody = GetComponent<Rigidbody>();
                InputManager.OnGrenadeStopped += GrenadeTriggerUp;
            }
        }


        private void GrenadeTriggerUp()
        {
            CmdThrowCommand(gameObject.transform.position + gameObject.transform.forward * 2, Camera.main.transform.rotation, playerRigdbody.velocity);

        }

        [Command]
        public void CmdThrowCommand(Vector3 Position, Quaternion Rotation, Vector3 playerVelocity)
        {
            GameObject grenadeObject = Instantiate(grenadePrefab, Position, Rotation);
            grenadeObject.GetComponent<Rigidbody>().AddForce(playerVelocity, ForceMode.VelocityChange);
            NetworkServer.Spawn(grenadeObject);
        }
    }
}
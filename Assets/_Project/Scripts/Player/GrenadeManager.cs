using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Equivalent to PlayerGunManager but for grenades. no weapon object is needed

namespace Vanguard
{
    public class GrenadeManager : NetworkBehaviour
    {
        public GameObject grenadePrefab;
        public Rigidbody playerRigdbody;
        // Start is called before the first frame update
        void Start()
        {
            if (!isLocalPlayer)
            {
                enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.G))
            {
                CmdThrowCommand(gameObject.transform.position + gameObject.transform.forward * 2, gameObject.transform.rotation, playerRigdbody.velocity);
            }
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
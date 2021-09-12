using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Equivalent to PlayerGunManager but for grenades. no weapon object is needed

namespace Vanguard
{
    public class GrenadeManager : NetworkBehaviour
    {
        public GameObject grenade;
        public Rigidbody playerRigdbody;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.G))
            {
                CmdThrowCommand(gameObject.transform.position + gameObject.transform.forward * 2, gameObject.transform.rotation);
            }
        }

        [Command]
        public void CmdThrowCommand(Vector3 Position, Quaternion Rotation)
        {
            GameObject grenadeObject = Instantiate(grenade, Position, Rotation);
            grenadeObject.GetComponent<Rigidbody>().velocity = playerRigdbody.velocity;
            grenadeObject.GetComponent<Rigidbody>().AddRelativeForce(0, 0, grenadeObject.GetComponent<Grenade>().nadeVelocity, ForceMode.VelocityChange);
            NetworkServer.Spawn(grenadeObject);
        }
    }
}
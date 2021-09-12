using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace Vanguard
{
    //Equivalent to Projectile
    public class Grenade : NetworkBehaviour
    {
        public float nadeLife = 5;
        public GameObject particleSystem; //Make particleSystems automatically destroy themselves
        public Rigidbody rb;
        public float nadeVelocity = 20;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public override void OnStartServer()
        {
            Invoke(nameof(DestroySelf), nadeLife);
        }
        [Server]
        void DestroySelf()
        {
            NetworkServer.Destroy(gameObject);
            GameObject newGrenadeExplosion = Instantiate(particleSystem,gameObject.transform.position,new Quaternion(0,1,0,0));
            NetworkServer.Spawn(newGrenadeExplosion);
        }


    }
}
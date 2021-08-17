using UnityEngine;
using Mirror;
    public class Projectile : NetworkBehaviour
    {
        public int damage;
        public float destroyAfter = 5;
        public Rigidbody rigidBody;
        public float speed = 1000;
        [HideInInspector]public GameObject playerWhoShoot;
        public override void OnStartServer()
        {
            Invoke(nameof(DestroySelf), destroyAfter);
        }

        // set velocity for server and client. this way we don't have to sync the
        // position, because both the server and the client simulate it.
        void Start()
        {
        rigidBody.velocity = speed * transform.forward;
        }

        // destroy for everyone on the server
        [Server]
        void DestroySelf()
        {
            NetworkServer.Destroy(gameObject);
        }

        // ServerCallback because we don't want a warning if OnTriggerEnter is
        // called on the client
        [ServerCallback]

        void OnTriggerEnter(Collider co)
        {
        Debug.Log(co.name);
        if (co.gameObject == playerWhoShoot)return;
        if(co.GetComponent<Health>()) co.GetComponent<Health>().getShot(damage);
        DestroySelf();
        }
    }


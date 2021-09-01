using UnityEngine;
using Mirror;
    public class Projectile : NetworkBehaviour
    {
        public int damage;
        public float destroyAfter = 5,raycastRange;
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
    RaycastHit rayHit;
    public void Update()
    {
        if (!isServer) return;
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out rayHit, raycastRange))
        {
            Debug.Log(rayHit.collider.gameObject.name);
            if (rayHit.collider.gameObject == playerWhoShoot) return;
            if (rayHit.collider.GetComponent<Health>()) rayHit.collider.GetComponent<Health>().getShot(damage);
            DestroySelf();
        }
    }
}


using UnityEngine;
using Mirror;
namespace Vanguard
{
    public class Projectile : NetworkBehaviour
    {
        public int damage;
        public float destroyAfter = 5, raycastRange;
        public Rigidbody rigidBody;
        public float speed = 1000;
        public bool isserver;//isServer Returned false even in host so had to make a new one
        [HideInInspector] public GameObject playerWhoShoot;
        Vector3 lastPos;
        ObjectPooling objectPooling;
        // set velocity for server and client. this way we don't have to sync the
        // position, because both the server and the client simulate it.
        void Start()
        {
            objectPooling = FindObjectOfType<ObjectPooling>();
        }
        public void StartBullet()
        {
            Invoke(nameof(DestroySelf), destroyAfter);
            rigidBody.velocity = speed * transform.forward;
            lastPos = transform.position;
        }
        // destroy for everyone on the server
        void DestroySelf()
        {
            objectPooling.PutBackInPool(gameObject);
        }
        RaycastHit rayHit;
        public void FixedUpdate()
        {
            Ray ray = new Ray(lastPos, (transform.position - lastPos).normalized);
            lastPos = transform.position;

            if (Physics.Raycast(ray, out rayHit, raycastRange))
            {
                if (rayHit.collider.gameObject == playerWhoShoot) return;
                if (rayHit.collider.GetComponent<Health>() && isserver)rayHit.collider.GetComponent<Health>().getShot(damage);
                
                else if (rayHit.collider.GetComponent<TargetHealth>() && isserver)rayHit.collider.GetComponent<TargetHealth>().getShot(damage);
                DestroySelf();
            }

        }
    }
}
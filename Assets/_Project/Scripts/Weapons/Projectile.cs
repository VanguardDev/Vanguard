using UnityEngine;
using FishNet.Object;
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
        public int owner;
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
            Vector3 travelledVector = (transform.position - lastPos);
            Ray ray = new Ray(lastPos, travelledVector.normalized);
            lastPos = transform.position;
            raycastRange = travelledVector.magnitude;
            
            if (Physics.Raycast(ray, out rayHit, raycastRange))
            {
                if (rayHit.collider.gameObject == playerWhoShoot) return;
                if (rayHit.collider.GetComponent<Health>() && isserver)
                {
                    Debug.Log("hit " + rayHit.collider.gameObject.name);
                    rayHit.collider.GetComponent<Health>().getShot(damage,owner);
                }
                DestroySelf();
            }

        }
    }
}
using UnityEngine;
using Mirror;

public class Projectile : NetworkBehaviour
{
    public float lifetime = 5;
    public float maxDistance = 5000;
    public float bulletSpeed = 500;
    public float bulletDrop = 0.2f;
    public float collisionRadius = 0.002f;
    public GameObject hitEffectPrefab;
    public LayerMask hitMask;

    private float distanceTravelled;

    public override void OnStartServer()
    {
        Invoke("DestroySelf", lifetime);
    }

    void FixedUpdate()
    {   
        Vector3 velocity = transform.forward * Time.fixedDeltaTime * bulletSpeed;
        distanceTravelled += velocity.magnitude;
        transform.position += velocity + (Vector3.up * -(bulletDrop * Time.deltaTime));

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, collisionRadius, hitMask);
        if (hitColliders.Length > 0) {
            // foreach (var hitCollider in hitColliders)
            // {
            //     hitCollider.SendMessage("AddDamage");
            // }
            if (hitEffectPrefab != null)
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            DestroySelf();
        }

        if (distanceTravelled > maxDistance)
            DestroySelf();
    }

    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
        Debug.Log("DestroySelf");
    }
}
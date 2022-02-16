using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
//JamesFrowen.MirrorExamples
namespace Vanguard
{
    public class ObjectPooling : NetworkBehaviour
    {
        [Header("Settings")]
        public int startSize = 5;
        public int maxSize = 20;
        public GameObject prefab;

        [Header("Debug")]
        [SerializeField] Queue<GameObject> pool;
        [SerializeField] int currentCount;


        // void Start()
        // {
        //     InitializePool();
        // }

        public override void OnStartClient()
        {
            InitializePool();
            base.OnStartClient();
        }

        private void InitializePool()
        {
            pool = new Queue<GameObject>();
            for (int i = 0; i < startSize; i++)
            {
                GameObject next = CreateNew();

                pool.Enqueue(next);
            }
        }

        GameObject CreateNew()
        {
            if (currentCount > maxSize)
            {
                Debug.LogError($"Pool has reached max size of {maxSize}");
                return null;
            }

            // use this object as parent so that objects dont crowd hierarchy
            GameObject next = Instantiate(prefab, transform);
            next.name = $"{prefab.name}_pooled_{currentCount}";
            next.SetActive(false);
            currentCount++;
            return next;
        }


        /// <summary>
        /// Used to take Object from Pool.
        /// <para>Should be used on server to get the next Object</para>
        /// <para>Used on client by ClientScene to spawn objects</para>
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public GameObject GetFromPool(Vector3 position, Quaternion rotation,int damage,float projectileSpeed)
        {
            if (IsServer) RpcGetFromPool(position, rotation,  damage,  projectileSpeed);
            GameObject next = pool.Count > 0
                ? pool.Dequeue() // take from pool
                : CreateNew(); // create new because pool is empty
                               // CreateNew might return null if max size is reached
            if (next == null) { return null; }

            // set position/rotation and set active


            next.transform.position = position;
            next.transform.rotation = rotation;
            next.GetComponent<Projectile>().damage = damage;
            next.GetComponent<Projectile>().speed = projectileSpeed;
            next.SetActive(true);
            next.GetComponent<Projectile>().isserver = IsServer;
            next.GetComponent<Projectile>().StartBullet();
            return next;
        }
        [ObserversRpc]
        void RpcGetFromPool(Vector3 position, Quaternion rotation, int damage, float projectileSpeed)
        {
            if (!IsServer) GetFromPool(position, rotation,  damage,  projectileSpeed);
        }

        /// <summary>
        /// Used to put object back into pool so they can b
        /// <para>Should be used on server after unspawning an object</para>
        /// <para>Used on client by ClientScene to unspawn objects</para>
        /// </summary>
        /// <param name="spawned"></param>
        public void PutBackInPool(GameObject spawned)
        {
            spawned.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // disable object
            spawned.SetActive(false);

            // add back to pool
            pool.Enqueue(spawned);
        }
    }
}
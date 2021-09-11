using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Vanguard
{
    public class ProjectileWeapon : Weapon
    {
        public GameObject projectilePrefab;
        public Transform ShootingPoint;
        private PlayerGunManager gunManager;

        protected override void Start()
        {
            base.Start();
            gunManager = GetComponentInParent<PlayerGunManager>();
        }

        protected override void Shoot()
        {
            base.Shoot();
            gunManager.CmdShootCommand(transform.position, transform.rotation, damage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vanguard
{
    public class ProjectileWeapon : Weapon
    {
        public GameObject projectilePrefab;
        public Transform ShootingPoint;
        private PlayerGunManager gunManager;
        public Health player;

        protected override void Start()
        {
            
            base.Start();
            gunManager = GetComponentInParent<PlayerGunManager>();
            player = GetComponentInParent<Health>();
        }

        protected override void Shoot()
        {
            base.Shoot();
            gunManager.CmdShootCommand(ShootingPoint.position, ShootingPoint.rotation, damage,player.id);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProjectileWeapon : Weapon 
{
    public GameObject projectilePrefab;
    public Transform ShootingPoint;
    private PlayerGunManager gunManager;
    
    void Start()
    {
        gunManager = GetComponentInParent<PlayerGunManager>();
    }
    public override void Shoot()
    {
        if (ammo > 0 && canShoot&& !reloading)
        {
            base.Shoot();
            gunManager.CmdShootCommand();
            ConsumeAmmo();
        }
    }

    public override void ShootInputDown() {
        Shoot();
    }
}

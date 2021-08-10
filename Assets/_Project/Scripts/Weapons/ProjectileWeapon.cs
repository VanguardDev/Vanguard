using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProjectileWeapon : Weapon
{
    public GameObject projectilePrefab;
    private Camera camera;
    
    void Start()
    {
        camera = GetComponentInParent<Camera>();
    }

    public void ShootCommand() {
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, camera.transform.rotation);
        //NetworkServer.Spawn(projectileObject);
    }
    
    public override void Shoot()
    {
        base.Shoot();
        ShootCommand();
        ConsumeAmmo();
    }

    public override void ShootInputDown() {
        Shoot();
    }
}
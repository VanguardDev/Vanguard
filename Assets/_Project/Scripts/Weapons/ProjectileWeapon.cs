using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProjectileWeapon : Weapon 
{
    public GameObject projectilePrefab;
    public Transform ShootingPoint;
    private Camera camera;
    
    void Start()
    {
        camera = GetComponentInParent<Camera>();
    }
    [Command]
    public void CmdShootCommand() {
        GameObject projectileObject = Instantiate(projectilePrefab, ShootingPoint.transform.position, ShootingPoint.transform.rotation);
        NetworkServer.Spawn(projectileObject);
    }
    
    public override void Shoot()
    {
        base.Shoot();
        CmdShootCommand();
        ConsumeAmmo();
    }

    public override void ShootInputDown() {
        Shoot();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;
public class PlayerGunManager : NetworkBehaviour 
{
    private Weapon weapon;
    private GameObject projectilePrefab;
    private Transform shootingPoint;
    void Start()
    {
        
        weapon = GetComponentInChildren<Weapon>();
        if (!isLocalPlayer)
        {
            weapon.enabled = false;
            enabled = false;
        }
        if(weapon.GetComponent<ProjectileWeapon>())
        {
            projectilePrefab = weapon.GetComponent<ProjectileWeapon>().projectilePrefab;
            shootingPoint = weapon.GetComponent<ProjectileWeapon>().ShootingPoint;
        }
    }

    void Update() {
        if(weapon.isFullAuto && Input.GetMouseButton(0))
            weapon.ShootInputDown();
        else if (Input.GetMouseButtonDown(0) && ! weapon.isFullAuto)
                weapon.ShootInputDown();
        if (Input.GetMouseButtonUp(0))
            weapon.ShootInputUp();
        if (Input.GetKeyDown(KeyCode.R)) weapon.ReloadInputUpdate();
    }
    [Command]
    public void CmdShootCommand(Vector3 Position,Quaternion Rotation,int Damage)
    {
        GameObject projectileObject = Instantiate(projectilePrefab,Position, Rotation);
        projectileObject.GetComponent<Projectile>().playerWhoShoot = gameObject;
        projectileObject.GetComponent<Projectile>().damage = Damage;
        NetworkServer.Spawn(projectileObject);
    }
}

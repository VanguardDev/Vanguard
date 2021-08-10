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
        if (Input.GetMouseButtonDown(0))
            weapon.ShootInputDown();
        if (Input.GetMouseButtonUp(0))
            weapon.ShootInputUp();
        if (Input.GetKeyDown(KeyCode.R)) weapon.ReloadInputUpdate();
    }
    [Command]
    public void CmdShootCommand()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, shootingPoint.transform.position, shootingPoint.transform.rotation);
        NetworkServer.Spawn(projectileObject);
    }
}

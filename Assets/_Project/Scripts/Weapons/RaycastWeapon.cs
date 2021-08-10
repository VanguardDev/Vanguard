using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : Weapon
{
    public GameObject hitEffectPrefab;
    private Camera camera;
    
    void Start()
    {
        camera = GetComponentInParent<Camera>();
    }

    public override void Shoot()
    {
        base.Shoot();
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, Mathf.Infinity)) {
            Instantiate(hitEffectPrefab, hit.transform.position, Quaternion.LookRotation(hit.normal));
        }
        ConsumeAmmo();
    }

    public override void ShootInputDown() {
        Shoot();
    }
}
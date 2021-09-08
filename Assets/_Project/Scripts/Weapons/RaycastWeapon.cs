using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : Weapon
{
    public GameObject hitEffectPrefab;
    private Camera camera;
    
    protected override void Start()
    {
        base.Start();
        camera = GetComponentInParent<Camera>();
    }

    protected override void Shoot()
    {
        base.Shoot();
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, Mathf.Infinity)) {
            Instantiate(hitEffectPrefab, hit.transform.position, Quaternion.LookRotation(hit.normal));
        }
    }

    public override void TriggerDown() {
        Shoot();
    }
}

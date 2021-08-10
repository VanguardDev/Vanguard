using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int ammo;
    public int ammoCapacity;
    public float reloadTime;
    public bool canShoot = true;

    public virtual void Shoot() {
        if (canShoot)
            StartCoroutine(DelayShooting());
    }

    public virtual IEnumerator DelayShooting()
    {
        if (canShoot == false)
            yield break;

        canShoot = false;
        Debug.Log(reloadTime);
        yield return new WaitForSeconds(reloadTime);
        Debug.Log("DoneReload");

        canShoot = true;
    }

    public virtual void ShootInputDown() {  }
    public virtual void ShootInputUp() {  }

    public virtual void Reload() { ammo = ammoCapacity; }

    public virtual void ConsumeAmmo() {
        ammo -= 1;
        if (ammo < 0)
            canShoot = false;
    }
}
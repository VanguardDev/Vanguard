using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class Weapon : NetworkBehaviour
{
    public int ammo;
    public int ammoCapacity;
    public float reloadTime;
    public bool canShoot = true;
    public Text ammoCountText;

    public virtual void Shoot() {
        if (canShoot)
            StartCoroutine(DelayShooting());
    }

    public virtual IEnumerator DelayShooting()
    {
        if (canShoot == false)
            yield break;

        canShoot = false;
        yield return new WaitForSeconds(reloadTime);

        canShoot = true;
    }

    public virtual void ShootInputDown() {  }
    public virtual void ShootInputUp() {  }

    public virtual void Reload() { ammo = ammoCapacity; }

    public virtual void ConsumeAmmo() {
        ammo -= 1;
        ammoCountText.text = ammo.ToString();
        if (ammo < 0)
            canShoot = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class Weapon : MonoBehaviour
{
    public int ammo;
    public int ammoCapacity;
    public float shootInterval,reloadTime;
    public bool canShoot = true;
    public bool reloading;
    public Text ammoCountText;
    private void Start()
    {
        ammoCountText.text = ammoCapacity.ToString();
    }
    public virtual void Shoot() {
        if (canShoot)
            StartCoroutine(DelayShooting());
    }

    public virtual IEnumerator DelayShooting()
    {
        if (canShoot == false)
          yield break;

        if (reloading) yield return null;
        canShoot = false;
        yield return new WaitForSeconds(shootInterval);
        canShoot = true;
    }
    public void ReloadInputUpdate()
    {
       StartCoroutine("Reload");
    }
    public virtual IEnumerator Reload()
    {
        if (reloading) yield break;
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        ammo = ammoCapacity;
        ammoCountText.text = ammo.ToString();
        reloading = false;
    }

    public virtual void ShootInputDown() {  }
    public virtual void ShootInputUp() {  }


    public virtual void ConsumeAmmo() {
        ammo -= 1;
        ammoCountText.text = ammo.ToString();
        if (ammo <= 0)
        {
           
            canShoot = false;
            StartCoroutine("Reload");
            
        }
    }
}

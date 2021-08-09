using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Mirror;

public class Gun : NetworkBehaviour
{
    public LayerMask layermaskforraycast;
    public Transform gunShotPoint,gunParticleSpawnPoint;
    public GameObject gunShot;
    public float damage,shootInterval,reloadTime;
    public bool isGunAutomatic;
    public Text ammoText;
    public int maxammo;
    int ammo;
    bool reloading,autoFiring;
    float shootTimer,reloadTimer;
    public void Start()
    {
        ammo = maxammo;
        GetComponent<FirstPersonLook>().pilotActionControls.VanguardPilot.Shoot.performed += shootInput;
        GetComponent<FirstPersonLook>().pilotActionControls.VanguardPilot.Shoot.canceled += shootInput;
        GetComponent<FirstPersonLook>().pilotActionControls.VanguardPilot.Reload.performed += updateReloadInput;
    }
    [Command]
    public void Cmdshoot(float damagesend)
    {
        RaycastHit rayHit;
        Ray r = new Ray(gunShotPoint.position, GetComponentInChildren<Camera>().transform.forward);
        GameObject gunShotParticle = Instantiate(gunShot, gunParticleSpawnPoint.position, GetComponentInChildren<Camera>().transform.rotation);
        NetworkServer.Spawn(gunShotParticle);
        if (Physics.Raycast(r, out rayHit, 100, layermaskforraycast))
        {
            if (!rayHit.collider.GetComponent<Health>()) return;//probably there is a better pls fix if you can
            if(rayHit.collider.GetComponent<Health>().team!= GetComponent<Health>().team)
            rayHit.collider.gameObject.SendMessageUpwards("getShot", damagesend);
        }
    }
    public void shoot()
    {
        if (shootTimer >= shootInterval && ammo > 0)
        {
            ammo--;
            ammoText.text = ammo.ToString();
            shootTimer = 0;
            Cmdshoot(damage);
        }
    }
    public void shootInput(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1)
        {
            if (!isGunAutomatic) shoot();
            else autoFiring = true;
        }
        else autoFiring = false;

    }
    public void Update()
    {
        if (autoFiring) shoot();
        if (shootTimer < shootInterval) shootTimer += Time.deltaTime;
        if (ammo <= 0) reloading = true;
        if (reloading) reloadingUpdate();
    }
    public void updateReloadInput(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1)
        {
            reloading = true;
        }
    }
    public void updateReloadInput()
    {
            reloading = true;
    }
    public void reloadingUpdate()
    {
        if (reloadTimer < reloadTime) reloadTimer += Time.deltaTime;
        else
        {
            reloading = false;
            ammo = maxammo;
            ammoText.text = ammo.ToString();
            reloadTimer = 0;
        }
    }
}

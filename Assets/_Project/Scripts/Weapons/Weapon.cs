using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class Weapon : MonoBehaviour
{

    public float ProjectileSpeed; // kinda useles for hitscan weapons but it helps with less getcomponent functions
    public int damage,ammo;
    public GameObject model;
    public int ammoCapacity;
    public bool canShoot = true, isFullAuto;
    public bool reloading;
    public Text ammoCountText;
    public float roundsPerMinute,reloadTime;
    private bool isTriggerDown;
    private float fireRate;
    private float timeSinceLastShot = float.MaxValue;
    

    private bool WantsToShoot { get => isTriggerDown; }
    private bool IsReadyToShoot
    {
        get
        {
            timeSinceLastShot += Time.deltaTime;
            return !reloading && timeSinceLastShot > fireRate;
        }
    }
    private bool HasAmmo { get => ammo > 0; }

    #region Unity Overrides
    protected virtual void Start()
    {
        if(ammoCountText)
        ammoCountText.text = ammoCapacity.ToString();
        fireRate = 60f / roundsPerMinute;
    }

    protected virtual void Update()
    {
        if (IsReadyToShoot &&
            WantsToShoot &&
            HasAmmo)
        {
            Shoot();
        }
    }
    #endregion

    public virtual void TriggerDown()
    {
        isTriggerDown = true;
    }
    public virtual void TriggerUp()
    {
        isTriggerDown = false;
    }

    public void ReloadInputUpdate()
    {
        StartCoroutine("Reload");
    }

    protected virtual void Shoot()
    {
        timeSinceLastShot = 0f;
        ConsumeAmmo(1);//it might use different amount of ammo based on gun or mod idk
        // Play visual FX, sounds, etc.
    }

    private IEnumerator Reload()
    {
        if (reloading) yield break;
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        if (reloading)
        {
            ammo = ammoCapacity;

            // TODO: decouple this with an event
            ammoCountText.text = ammo.ToString();

            reloading = false;
        }
    }

    public void ConsumeAmmo(int amount) {
        ammo -= amount;
        ammoCountText.text = ammo.ToString();
        if (ammo <= 0)
        {
           
            canShoot = false;
            StartCoroutine("Reload");
            
        }
    }
}

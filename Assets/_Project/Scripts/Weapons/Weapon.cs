using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public UnityEvent<int> OnAmmoChanged;

    public float projectileSpeed;
    public GameObject model;
    public int damage,ammo;
    public int ammoConsumption;
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
        if(ammoCountText)ammoCountText.text = ammoCapacity.ToString();
        fireRate = 60f / roundsPerMinute;

        OnAmmoChanged.AddListener(UpdateAmmoText);
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

    private void UpdateAmmoText(int newCount)
    {
        ammoCountText.text = newCount.ToString();
    }

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
        ConsumeAmmo(ammoConsumption);
        OnAmmoChanged.Invoke(ammo);
        // Play visual FX, sounds, etc.
    }

    private IEnumerator Reload()
    {
        if (reloading || ammo == ammoCapacity) yield break;
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        ammo = ammoCapacity;

        OnAmmoChanged.Invoke(ammo);

        reloading = false;
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

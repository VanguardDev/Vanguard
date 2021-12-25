using UnityEngine;
using FishNet.Object;
namespace Vanguard
{
    public class PlayerGunManager : NetworkBehaviour
    {
        private Weapon weapon;
        private GameObject projectilePrefab;
        private Transform shootingPoint;
        private ObjectPooling objectPooling;

        // void Start()
        // {
        //     // Todo set this is editor, `FindObjectOfType` can be very expensive
        //     objectPooling = FindObjectOfType<ObjectPooling>();
        //     weapon = GetComponentInChildren<Weapon>();
        //     if (!IsOwner)
        //     {
        //         weapon.enabled = false;
        //         enabled = false;
        //     }
        //     else
        //     {
        //         InputManager.OnShootStarted += TriggerDown;
        //         InputManager.OnShootStopped += TriggerUp;
        //         InputManager.OnReloadStarted += Reload;
        //     }

        //     if (weapon.GetComponent<ProjectileWeapon>())
        //     {
        //         projectilePrefab = weapon.GetComponent<ProjectileWeapon>().projectilePrefab;
        //         shootingPoint = weapon.GetComponent<ProjectileWeapon>().ShootingPoint;
        //     }
        // }

        public override void OnStartClient(){
            // Todo set this is editor, `FindObjectOfType` can be very expensive
            objectPooling = FindObjectOfType<ObjectPooling>();
            weapon = GetComponentInChildren<Weapon>();
            if (!IsOwner)
            {
                weapon.enabled = false;
                enabled = false;
            }
            else
            {
                InputManager.OnShootStarted += TriggerDown;
                InputManager.OnShootStopped += TriggerUp;
                InputManager.OnReloadStarted += Reload;
            }

            if (weapon.GetComponent<ProjectileWeapon>())
            {
                projectilePrefab = weapon.GetComponent<ProjectileWeapon>().projectilePrefab;
                shootingPoint = weapon.GetComponent<ProjectileWeapon>().ShootingPoint;
            }
            base.OnStartClient();
        }

        private void TriggerDown()
        {
            weapon.TriggerDown();
        }

        private void TriggerUp()
        {
            weapon.TriggerUp();
        }

        private void Reload()
        {
            weapon.ReloadInputUpdate();
        }

        [ServerRpc]
        public void CmdShootCommand(Vector3 Position, Quaternion Rotation, int Damage)
        {
            GameObject projectileObject = objectPooling.GetFromPool(Position, Rotation);
            projectileObject.GetComponent<Projectile>().playerWhoShoot = gameObject;
            projectileObject.GetComponent<Projectile>().damage = Damage;
        }
    }
}
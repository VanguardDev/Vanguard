using UnityEngine;
using Mirror;
namespace Vanguard
{
    public class PlayerGunManager : NetworkBehaviour
    {
        private Weapon weapon;
        private GameObject projectilePrefab;
        private Transform shootingPoint;
        private ObjectPooling objectPooling;

        void Start()
        {
            // Todo set this is editor, `FindObjectOfType` can be very expensive
            objectPooling = FindObjectOfType<ObjectPooling>();
            weapon = GetComponentInChildren<Weapon>();
            if (!isLocalPlayer)
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

        [Command]
        public void CmdShootCommand(Vector3 Position, Quaternion Rotation, int Damage)
        {
            GameObject projectileObject = objectPooling.GetFromPool(Position, Rotation);
            projectileObject.GetComponent<Projectile>().playerWhoShoot = gameObject;
            projectileObject.GetComponent<Projectile>().damage = Damage;
        }
    }
}
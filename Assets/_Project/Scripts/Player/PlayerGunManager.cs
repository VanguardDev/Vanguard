using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet;



namespace Vanguard
{
    public class PlayerGunManager : NetworkBehaviour
    {
        public GameObject gunPrefab;
        public Weapon[] weapons = new Weapon[3];

         [SyncVar(OnChange = "ClientSetWeapon")] public int currentWeaponIndex = 0;
        private GameObject projectilePrefab;
        public Transform shootingPoint;
        private ObjectPooling objectPooling;
        public void copyWeaponAttiributes(Weapon from, Weapon to)
        {
            to.ammo = from.ammo;
            to.ammoCapacity = from.ammoCapacity;
            to.damage = from.damage;
            to.isFullAuto = from.isFullAuto;
            to.reloadTime = from.reloadTime;
            to.roundsPerMinute = from.roundsPerMinute;
            to.GetComponent<ProjectileWeapon>().projectilePrefab = from.GetComponent<ProjectileWeapon>().projectilePrefab;//it doesnt check for errors this will give error if it isnt a projectile weapon
            to.GetComponent<Weapon>().projectileSpeed = from.projectileSpeed;
            to.model = from.model;
        }
        [ObserversRpc]
        public void RpcPickUpWeapon(GameObject weapon,GameObject weaponOnGround)//used for replacing the current weapon with a new one 
        {

            Weapon weaponToPickUp = weapon.GetComponent<Weapon>();//convert it to weapon class to use for stats
            copyWeaponAttiributes(weapons[currentWeaponIndex], weaponOnGround.GetComponent<Weapon>());//set the stats for the weapon on the ground i couldve just spawn an object with this set but fishnet doesnt seem to work
            
           
            copyWeaponAttiributes(weaponToPickUp, weapons[currentWeaponIndex]);//set the stats of the new weapon
            Destroy(weapons[currentWeaponIndex].GetComponentInChildren<Model>().gameObject);//destroy the old model
            GameObject weaponModel = Instantiate(weaponToPickUp.model, weapons[currentWeaponIndex].transform);//spawn the new model
            if (IsOwner) weaponModel.GetComponent<Model>().setModel(0);//set the model so the weapon looks right
            else weaponModel.GetComponent<Model>().setModel(1);
            weapons[currentWeaponIndex].ConsumeAmmo(0);//consume 0 ammo to update the ammo text
            weapon.gameObject.SetActive(false);
            weaponOnGround.GetComponentInChildren<Model>().setModel(2);
            weaponOnGround.GetComponentInChildren<Model>().transform.position = weapon.transform.position;
            weaponOnGround.GetComponentInChildren<Model>().transform.rotation = weapon.transform.rotation;
        }
        Ray ray;
        RaycastHit rhit;
        public float maxPickUpRange;
        [ServerRpc]
        public void CmdPickUpWeapon()
        {
            ray = new Ray(shootingPoint.position, shootingPoint.forward);
            if (Physics.Raycast(ray, out rhit, maxPickUpRange))//check if there is a gun 
            {
                if (rhit.collider.tag == "gun")
                {
                    
                    GameObject spawnedGun = Instantiate(gunPrefab, rhit.transform.position, rhit.transform.rotation);
                    copyWeaponAttiributes(weapons[currentWeaponIndex], spawnedGun.GetComponent<Weapon>());
                    GameObject weaponModel = Instantiate(spawnedGun.GetComponent<Weapon>().model, spawnedGun.transform);//spawn the new model
                    weaponModel.GetComponent<Model>().setModel(2);
                    spawnedGun.GetComponent<BoxCollider>().enabled = true;
                    
                    InstanceFinder.ServerManager.Spawn(spawnedGun);
                    RpcPickUpWeapon(rhit.collider.gameObject,spawnedGun);// pick up the gun
                    rhit.collider.gameObject.SetActive(false);
                    Destroy(rhit.collider.gameObject, 15);//destroy it after 15 seconds(more than timeout time so it doesnt matter anyways)

                }
            }
        }
        public void getWeapons()
        {
            weapons = GetComponentsInChildren<Weapon>();
        }
        public void ClientSetWeapon(int oldIndex, int index,bool asServer)
        {
            weapons[oldIndex].enabled = false;
            foreach (MeshRenderer meshRenderer in weapons[oldIndex].GetComponentsInChildren<MeshRenderer>()) meshRenderer.enabled = false;//disables old gun model
            if (IsOwner)
            {
                weapons[oldIndex].TriggerUp();//stop the trigger so it doesnt shoot when you switch back to it 
                weapons[oldIndex].reloading = false;
                weapons[index].enabled = true;//enable the new weapon
                weapons[index].ConsumeAmmo(0);//consume 0 ammo to update ammo text
            }
            foreach (MeshRenderer meshRenderer in weapons[index].GetComponentsInChildren<MeshRenderer>()) meshRenderer.enabled = true;//enables the gun model
            Debug.Log("is owner = " + IsOwner.ToString());
            if(!IsOwner)weapons[index].GetComponentInChildren<Model>().setModel(1);
            currentWeaponIndex = index;//set the index 

        }
        [ServerRpc]
        public void CmdetWeapons(int index)
        {
            currentWeaponIndex = index;//set the index on the server so the clientSetWeapon will be called one every client using [syncvar(hook=)]
        }
        public void setWeapon()
        {
            CmdetWeapons(InputManager.wantedGunIndex);
        }
        public void setGunAsFirstPerson()
        {
            getWeapons();
            for (int i = 0; i < 3; i++)
            {
                weapons[i].transform.SetParent(shootingPoint);//changes the gunmodels parent for first person perspective
                weapons[i].transform.localPosition = new Vector3(0, 0, 0);
                weapons[i].transform.localEulerAngles = new Vector3(0, 0, 0);
                weapons[i].GetComponentInChildren<Model>().setModel(0);
                if (i != currentWeaponIndex) weapons[i].enabled = false;
            }
            Debug.Log("setting fps");
            foreach (MeshRenderer meshRenderer in weapons[currentWeaponIndex].GetComponentsInChildren<MeshRenderer>()) meshRenderer.enabled = true;//reenables guns model
        }
        public void Start()
        {
            getWeapons();
            if (!IsOwner) weapons[currentWeaponIndex].GetComponentInChildren<Model>().setModel(1);
        }
        public override void OnStartClient()
        {
            // Todo set this is editor, `FindObjectOfType` can be very expensive
            objectPooling = FindObjectOfType<ObjectPooling>();
            getWeapons();
            if (!IsOwner)
            {
                for (int i = 0; i < 3; i++) weapons[i].enabled = false;

                enabled = false;
            }
            else
            {
                InputManager.OnChangeWeapon += setWeapon;
                InputManager.OnShootStarted += TriggerDown;
                InputManager.OnShootStopped += TriggerUp;
                InputManager.OnReloadStarted += Reload;
                InputManager.OnPickup += CmdPickUpWeapon;
            }
            for (int i = 0; i < 3; i++)
            {
                if (weapons[i].GetComponent<ProjectileWeapon>())
                {
                    projectilePrefab = weapons[i].GetComponent<ProjectileWeapon>().projectilePrefab;
                    shootingPoint = weapons[i].GetComponent<ProjectileWeapon>().ShootingPoint;
                }
            }

            base.OnStartClient();
        }

        private void TriggerDown()
        {
            Debug.Log("trigger down");
            weapons[currentWeaponIndex].TriggerDown();
        }

        private void TriggerUp()
        {
            weapons[currentWeaponIndex].TriggerUp();
        }

        private void Reload()
        {
            weapons[currentWeaponIndex].ReloadInputUpdate();
        }

        [ServerRpc]
        public void CmdShootCommand(Vector3 Position, Quaternion Rotation, int Damage)
        {
            GameObject projectileObject = objectPooling.GetFromPool(Position, Rotation, weapons[currentWeaponIndex].damage, weapons[currentWeaponIndex].projectileSpeed);
            projectileObject.GetComponent<Projectile>().playerWhoShoot = gameObject;
            projectileObject.GetComponent<Projectile>().damage = Damage;
        }
    }
}
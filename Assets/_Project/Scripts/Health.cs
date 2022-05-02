using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet;
namespace Vanguard
{
    public class Health : NetworkBehaviour
    {
        [SyncVar(OnChange = "updateHealth")] public float health;
        public Text healthText, redScoreText, blueScoreText, winScreenText, nameText, healthTextWorld;
        [SyncVar(OnChange = "setTeamColor")] public int team = -1; // this isnt hidden in inspector for debug purposes
        MatchManager mm;
        [SyncVar(OnChange = "nameChanged")] public string Name;
        [SyncVar (OnChange ="SetScoreBoard")]public int  Kills=0, Deaths=0;
        [SyncVar] public int id=-1;
        public Transform GunModel;

        public void SetScoreBoard(int old,int newe,bool isserver)
        {
            mm.updateScoreBoard();
            
        }

        private void Start(){
            mm = FindObjectOfType<MatchManager>();
        }
        public override void OnStartClient(){
            mm = FindObjectOfType<MatchManager>();
            //mm.NewPlayerConnected(this);
            if (IsOwner)
            {
                mm.blueScoreText = blueScoreText;
                mm.redScoreText = redScoreText;
                mm.updateTeamScore(0, 0, false);
                mm.winScreenText = winScreenText;
                Name = ConnectionInfo.Name;
                nameText.text = "";
                healthTextWorld.text = "";
                CmdSetName(Name);
                GetComponent<PlayerGunManager>().setGunAsFirstPerson();
               
                foreach (SkinnedMeshRenderer meshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>()) meshRenderer.enabled = false;//disables the character model for first person
                foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>()) meshRenderer.enabled = false;
                foreach (MeshRenderer meshRenderer in GetComponent<PlayerGunManager>().weapons[GetComponent<PlayerGunManager>().currentWeaponIndex].GetComponentsInChildren<MeshRenderer>()) meshRenderer.enabled = true;//reenables guns model

            }
            else
            {
                healthText.GetComponentInParent<Canvas>().gameObject.SetActive(false);
                healthText = healthTextWorld;
            }
            base.OnStartClient();
        }

        [Server]
        public void getShot(float damage,int ShooterId)
        {
            health -= damage;
            if (health <= 0) mm.playerDie(this,ShooterId);
        }
        public void updateHealth(float oldhealth, float newhealth, bool asServer)
        {
            healthText.text = newhealth.ToString();
        }
        public void setTeamColor(int oldteam, int newteam, bool asServer)
        {
            if (newteam == 1) GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.blue;//sets the color to the team color (kinda useless rn)
            else GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.red;
        }
        //need to make networktransform not client authorized so this line function doesnt need to exsist
        [ObserversRpc]
        public void RpcchangePlayerspos(Vector3 newPosition)
        {
            transform.position = newPosition;
        }
        public void OnDestroy()
        {
            if (!IsServer) return;
            mm.Disconnect(this);
        }
        [ServerRpc]
        public void CmdSetName(string setName)
        {
            mm.NewPlayerConnected(this,setName);
        }
        void nameChanged(string oldName, string newName, bool asServer)
        {
            if (!IsOwner)
            {
                Name = newName;
                nameText.text = Name;
            }
        }
    }
}
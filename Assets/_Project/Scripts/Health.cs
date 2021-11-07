using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
namespace Vanguard
{
    public class Health : NetworkBehaviour
    {
        [SyncVar(hook = "updateHealth")] public float health;
        public Text healthText, redScoreText, blueScoreText, winScreenText, nameText, healthTextWorld;
        [SyncVar(hook = "setTeamColor")] public int team = -1; // this isnt hidden in inspector for debug purposes
        MatchManager mm;
        [SyncVar(hook = "nameChanged")] public string name;

        public Transform GunModel;
        public void Start()
        {
            mm = FindObjectOfType<MatchManager>();
            if (isServer) mm.NewPlayerConnected(gameObject);
            if (isLocalPlayer)
            {
                mm.blueScoreText = blueScoreText;
                mm.redScoreText = redScoreText;
                mm.winScreenText = winScreenText;
                name = ConnectionInfo.name;
                nameText.text = "";
                healthTextWorld.text = "";
                CmdSetName(name);
                GunModel.SetParent(GetComponentInChildren<Weapon>().transform);
                GetComponentInChildren<VievModel>().SetVievmodel(GunModel);
                //foreach (SkinnedMeshRenderer meshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>()) meshRenderer.enabled = false;
                //foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>()) meshRenderer.enabled = false;
                //foreach (MeshRenderer meshRenderer in GunModel.GetComponentsInChildren<MeshRenderer>()) meshRenderer.enabled = true;

            }
            else
            {
                healthText.GetComponentInParent<Canvas>().gameObject.SetActive(false);
                healthText = healthTextWorld;
            }
        }
        [Server]
        public void getShot(float damage)
        {
            health -= damage;
            if (health <= 0) mm.playerDie(this);
        }
        public void updateHealth(float oldhealth, float newhealth)
        {
            healthText.text = newhealth.ToString();
        }
        public void setTeamColor(int oldteam, int newteam)
        {
            if (newteam == 1) GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.blue;
            else GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.red;
        }
        //need to make networktransform not client authorized so this line function doesnt need to exsist
        [ClientRpc]
        public void RpcchangePlayerspos(Vector3 newPosition)
        {
            transform.position = newPosition;
        }
        public void OnDestroy()
        {
            if (!isServer) return;
            mm.Disconnect(team);
        }
        [Command]
        public void CmdSetName(string setName)
        {
            name = setName;
        }
        void nameChanged(string oldName, string newName)
        {
            name = newName;
            nameText.text = name;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Health : NetworkBehaviour
{
    [SyncVar (hook ="updateHealth")] public float health;
    public Text healthText,redScoreText,blueScoreText;
    [SyncVar (hook ="setTeamColor")] public int team=-1; // this isnt hidden in inspector for debug purposes
    MatchManager mm;
    public void Start()
    {
        mm = FindObjectOfType<MatchManager>();
        if (isServer)mm.NewPlayerConnected(gameObject);
        
        if (isLocalPlayer)
        {
            mm.blueScoreText = blueScoreText;
            mm.redScoreText = redScoreText;
        }
    }
    public void getShot(float damage)
    {
        health -= damage;
        if (health < 0) mm.playerDie(this);
    }
    public void updateHealth(float oldhealth,float newhealth)
    {
        healthText.text = newhealth.ToString();
    }
    public void setTeamColor(int oldteam, int newteam)
    {
        if (newteam == 1) GetComponent<MeshRenderer>().material.color = Color.blue;
        else GetComponent<MeshRenderer>().material.color = Color.red;
    }
    //need to make networktransform not client authorized so this line function doesnt need to exsist
    [ClientRpc]
    public void RpcchangePlayerspos(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}

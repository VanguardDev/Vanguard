using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class MatchManager : NetworkBehaviour
{
    int redPlayerCount,bluePlayerCount;
    public Transform redSpawn, blueSpawn;
    [SyncVar(hook ="updateTeamScore")] int blueScore, redScore;//this is public cause debugging nothing else
    public int matchMaxScore;
    public float respawnTimer;
    [HideInInspector]public Text redScoreText, blueScoreText,winScreenText;
    bool restarting;


    public void NewPlayerConnected(GameObject player)
    {
        if (!isServer) return;
        Debug.Log("Player Connected");
        if (redPlayerCount > bluePlayerCount)
        {
            
            player.gameObject.GetComponent<Health>().team = 1;
            player.gameObject.GetComponent<Health>().setTeamColor(-1,1);
            player.GetComponent<Health>().RpcchangePlayerspos(blueSpawn.position);
            bluePlayerCount++;
        }
        else
        {
            player.gameObject.GetComponent<Health>().team = 0;
            player.gameObject.GetComponent<Health>().setTeamColor(-1,0);
            player.GetComponent<Health>().RpcchangePlayerspos(redSpawn.position);
            redPlayerCount++;
        }
    }
    public void playerDie(Health player)
    {
        RpcDisablePlayer(player);
        StartCoroutine("DieTimer",player);
    }
    [ClientRpc]
    public void RpcDisablePlayer(Health player)
    {
        player.GetComponent<MeshRenderer>().enabled = false;
        player.GetComponent<CapsuleCollider>().enabled = false;
        if (player.isLocalPlayer)
        {
            player.GetComponent<PlayerGunManager>().enabled = false;
            player.GetComponent<Rigidbody>().isKinematic = true;
            player.GetComponent<FirstPersonLook>().DisableControls();
        }
    }
    [ClientRpc]
    public void RpcEnablePlayer(Health player)
    {
        player.GetComponent<MeshRenderer>().enabled = true;
        player.GetComponent<CapsuleCollider>().enabled = true;
        player.nameText.text = player.name;
        if (player.isLocalPlayer)
        {
            player.nameText.text = "";
            player.GetComponent<PlayerGunManager>().enabled = true;
            player.GetComponent<Rigidbody>().isKinematic = false;
            player.GetComponent<FirstPersonLook>().EnableControls();
        }
    }
    public IEnumerator DieTimer(Health player)
    {
        yield return new WaitForSeconds(respawnTimer);
        if (player.team == 1)
        {
            player.health = 100;
            player.RpcchangePlayerspos(blueSpawn.position);
            redScore++;
        }
        else
        {
            player.health = 100;
            player.RpcchangePlayerspos(redSpawn.position);
            blueScore++;
        }
        if ((redScore >= matchMaxScore || blueScore >= matchMaxScore) && !restarting)
        {
            restarting = true;
            RpcEndMatch();
            StartCoroutine("Restart");
        }
        RpcEnablePlayer(player);
    }
    public IEnumerator Restart()
    {
        yield return new WaitForSeconds(5);
        blueScore = 0;
        redScore = 0;
        RpcRestartMatch();
        restarting = false;
    }
    [ClientRpc]
    public void RpcRestartMatch()
    {
        NetworkClient.localPlayer.GetComponent<FPInput>().EnableControls();
        NetworkClient.localPlayer.GetComponent<Health>().health = 100;
        if (NetworkClient.localPlayer.GetComponent<Health>().team == 1) NetworkClient.localPlayer.transform.position=blueSpawn.position;
        else NetworkClient.localPlayer.transform.position = redSpawn.position;
        winScreenText.enabled = false;

    }
    [ClientRpc]
    public void RpcEndMatch()
    {
        NetworkClient.localPlayer.GetComponent<FPInput>().DisableControls();
        winScreenText.enabled = true;
        if (redScore >= matchMaxScore)
            winScreenText.text = "Red Won";
        else winScreenText.text = "Blue Won";

    }
    public void updateTeamScore(int oldScore,int newScore)//the paramaters are useless for this function but have to add them
    {
        redScoreText.text = redScore.ToString();
        blueScoreText.text = blueScore.ToString();
    }
    public void Disconnect(int team)
    {
        if (team == 0) redPlayerCount--;
        else bluePlayerCount--;
    }
}

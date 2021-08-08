using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class MatchManager : NetworkBehaviour
{
    int redPlayerCount,bluePlayerCount;
    public Transform redSpawn, blueSpawn;
    [SyncVar(hook ="updateTeamScore")] public int blueScore, redScore;//this is public cause debugging nothing else
    public Text redScoreText, blueScoreText;


    public void NewPlayerConnected(GameObject player)
    {
        if (!isServer) return;
        Debug.Log("Player Connected");
        if (redPlayerCount > bluePlayerCount)
        {
            
            player.gameObject.GetComponent<Health>().team = 1;
            player.gameObject.GetComponent<Health>().setTeamColor(-1,1);
            bluePlayerCount++;
        }
        else
        {
            player.gameObject.GetComponent<Health>().team = 0;
            player.gameObject.GetComponent<Health>().setTeamColor(-1,0);
            redPlayerCount++;
        }
        
    }
    public void playerDie(Health player)
    {
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

    }
    public void updateTeamScore(int oldScore,int newScore)//the paramaters are useless for this function but have to add them
    {
        redScoreText.text = redScore.ToString();
        blueScoreText.text = blueScore.ToString();
    }
}

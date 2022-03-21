using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object.Synchronizing;
using FishNet.Object;
using FishNet.Managing.Client;
using UnityEngine.UI;

namespace Vanguard
{
    public class MatchManager : NetworkBehaviour
    {
        int redPlayerCount, bluePlayerCount;
        List<Health> redTeamHealth = new List<Health>(), blueTeamHealth = new List<Health>();
        public Transform redSpawn, blueSpawn;
        [SyncVar (OnChange = "updateTeamScore")] int blueScore, redScore;//this is public cause debugging nothing else
        public int matchMaxScore;
        public float respawnTimer;
        [HideInInspector] public Text redScoreText, blueScoreText, winScreenText;
        bool restarting;
        [SyncObject]  readonly public SyncDictionary<int,Health> Players = new SyncDictionary<int, Health>();
        
        public void ListUpdate_On_Change(SyncDictionaryOperation op,int id,  Health player, bool asServer)
        {
            foreach ( Health p in  Players.Values )
            {
                if (p.IsOwner)
                {
                    p.GetComponentInChildren<ScoreBoardManager>().setScoreBoard();//set the scoreboard
                    break;
                }
            }    
        }
        public void Awake()
        {
            Players.OnChange += ListUpdate_On_Change;
        }
        public void NewPlayerConnected(Health health)
        {
                if (redPlayerCount > bluePlayerCount)
                {
                    blueTeamHealth.Add(health);
                    health.team = 1;
                    health.setTeamColor(-1, 1, true);
                    health.RpcchangePlayerspos(blueSpawn.position);
                    bluePlayerCount++;

                    for (int i = 5; i < 10; i++)
                    {
                        if (!Players.ContainsKey(i) )
                        {
                            health.id = i;
                        Players.Add(i, health);
                        
                            break;
                        }
                    }
                }
                else
                {
                    redTeamHealth.Add(health);
                    health.team = 0;
                    health.setTeamColor(-1, 0, true);
                    health.RpcchangePlayerspos(redSpawn.position);
                    redPlayerCount++;
                    for (int i = 0; i < 5; i++)
                    {
                        if (!Players.ContainsKey(i))
                    {
                            health.id = i;
                        Players.Add(i, health);
                        break;
                        }
                    }
                }
            
        }
        public void playerDie(Health player,int shooter)
        {
            player.GetComponent<CapsuleCollider>().enabled = false;
            player.Deaths++;
            if(shooter!= -1)Players[shooter].Kills++;
            RpcDisablePlayer(player);
            StartCoroutine("DieTimer", player);
        }
        [ObserversRpc]
        public void RpcDisablePlayer(Health player)
        {
            foreach (SkinnedMeshRenderer meshRenderer in player.GetComponentsInChildren<SkinnedMeshRenderer>()) meshRenderer.enabled = false;//disables the model when a player dies
            foreach (MeshRenderer meshRenderer in player.GetComponentsInChildren<MeshRenderer>()) meshRenderer.enabled = false;
            player.GetComponent<CapsuleCollider>().enabled = false;
            if (player.IsOwner)
            {
                player.GetComponent<PlayerGunManager>().enabled = false;
                player.GetComponent<Rigidbody>().isKinematic = true;
                InputManager.DisableControls();
            }
        }
        [ObserversRpc]
        public void RpcEnablePlayer(Health player)
        {
            if (!player.IsOwner)
            {
                foreach (SkinnedMeshRenderer meshRenderer in player.GetComponentsInChildren<SkinnedMeshRenderer>()) meshRenderer.enabled = true;//enables the model when a player dies
                foreach (MeshRenderer meshRenderer in player.GetComponentsInChildren<MeshRenderer>()) meshRenderer.enabled = true;
            }
            player.GetComponent<CapsuleCollider>().enabled = true;
            player.nameText.text = player.Name;
            if (player.IsOwner)
            {
                player.nameText.text = "";
                player.GetComponent<PlayerGunManager>().enabled = true;
                player.GetComponent<Rigidbody>().isKinematic = false;
                InputManager.EnableControls();
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
                Debug.Log("RedScore");
            }
            else
            {
                player.health = 100;
                player.RpcchangePlayerspos(redSpawn.position);
                blueScore++;
                Debug.Log("BlueScore");
            }
            if ((redScore >= matchMaxScore || blueScore >= matchMaxScore) && !restarting)
            {
                restarting = true;
                RpcEndMatch();
                StartCoroutine("Restart");
            }
            player.GetComponent<CapsuleCollider>().enabled = enabled;
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
        [ObserversRpc]
        public void RpcRestartMatch()
        {
            
            InputManager.EnableControls();

            foreach (Health bluePlayer in blueTeamHealth){
                bluePlayer.health = 100;
                bluePlayer.gameObject.transform.position = blueSpawn.position;
            }

            foreach (Health redPlayer in redTeamHealth){
                redPlayer.health = 100;
                redPlayer.gameObject.transform.position = redSpawn.position;
            }

            redScore = 0;
            blueScore = 0;

            winScreenText.enabled = false;
        }
        [ObserversRpc]
        public void RpcEndMatch()
        {
            InputManager.DisableControls();
            winScreenText.enabled = true;
            if (redScore >= matchMaxScore)
                winScreenText.text = "Red Won";
            else winScreenText.text = "Blue Won";

        }
        public void updateTeamScore(int oldScore, int newScore, bool asServer)//the paramaters are useless for this function but have to add them
        {
            if(redScoreText!=null)redScoreText.text = redScore.ToString();
            if (blueScoreText != null) blueScoreText.text = blueScore.ToString();
        }
        public void Disconnect(Health Player)
        {
            if (Player.team == 0) redPlayerCount--;
            else bluePlayerCount--;
            Players.Remove(Player.id);
        }
    }
}
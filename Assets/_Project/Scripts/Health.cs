using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Health : NetworkBehaviour
{
    [SyncVar (hook ="updateHealth")] public float health;
    public Text healthText;
    public void getShot(float damage)
    {
        health -= damage;
    }
    public void updateHealth(float oldhealth,float newhealth)
    {
        healthText.text = newhealth.ToString();
    }
}

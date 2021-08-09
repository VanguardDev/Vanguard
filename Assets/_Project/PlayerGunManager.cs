using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;
public class PlayerGunManager : NetworkBehaviour 
{
    private Weapon weapon;
    void Start()
    {
        
        weapon = GetComponentInChildren<Weapon>();
        if (!isLocalPlayer)
        {
            weapon.enabled = false;
            enabled = false;
        }
        //GetComponent<FirstPersonLook>().pilotActionControls.VanguardPilot.Jump.performed += Jump;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0))
            weapon.ShootInputDown();
        if (Input.GetMouseButtonUp(0))
            weapon.ShootInputUp();
    }
}

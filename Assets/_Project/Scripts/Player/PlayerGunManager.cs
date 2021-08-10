using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerGunManager : MonoBehaviour 
{
    private Weapon weapon;
    void Start()
    {
        weapon = GetComponentInChildren<Weapon>();
        //GetComponent<FirstPersonLook>().pilotActionControls.VanguardPilot.Jump.performed += Jump;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0))
            weapon.ShootInputDown();
        if (Input.GetMouseButtonUp(0))
            weapon.ShootInputUp();
    }
}
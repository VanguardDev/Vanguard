using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
public class spawncube : NetworkBehaviour
{
    public GameObject cubeprefab;
    public LayerMask layermaskforraycast;
    public void Start()
    {
        GetComponent<FirstPersonLook>().pilotActionControls.VanguardPilot.Shoot.performed += makecube;
    }
    [Command]
    public void Cmdmakecube(Vector3 pos,Quaternion rot)
    {
        GameObject bulletClone = Instantiate(cubeprefab,pos, rot);
        NetworkServer.Spawn(bulletClone);
    }
    public void makecube(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1)
        {
            RaycastHit rayhit;
            Ray r = new Ray(GetComponentInChildren<Camera>().transform.position, GetComponentInChildren<Camera>().transform.forward);
            if (Physics.Raycast(r, out rayhit,100,layermaskforraycast))Cmdmakecube(rayhit.point+new Vector3(0,cubeprefab.transform.localScale.y/2,0),transform.rotation);
            
        }
        
    }
}

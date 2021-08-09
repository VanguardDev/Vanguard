using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbarkingManager : MonoBehaviour{
    public FirstPersonMove playerPilot;
    public FirstPersonLook playerPilotLook;
    public RobotMovement playerRobot;
    public BigRobotLook playerRobotLook;
    public Camera cam;
    public Transform camHolder;
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void Embark(){
        playerPilot.enabled = false;
        playerPilotLook.SetLookEnabled(false);
        playerPilotLook.enabled = false;
        playerRobot.enabled = true;
        playerRobotLook.enabled = true;

        playerRobotLook.cam = this.cam;
        cam.transform.position = camHolder.position;
        cam.transform.rotation = camHolder.rotation;
        cam.transform.SetParent(camHolder);
        playerRobotLook.SetLookEnabled(true);

        //playerRobot.Sync();
    }

    public void Disembark(){

    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag.Equals("Player")){
            playerPilot = other.gameObject.GetComponent<FirstPersonMove>();
            playerPilotLook = other.gameObject.GetComponent<FirstPersonLook>();
            cam = other.GetComponentInChildren<Camera>();

            other.gameObject.transform.SetParent(this.transform.root);
            other.gameObject.transform.position = this.transform.root.position;

            Embark();
        }
    }
}

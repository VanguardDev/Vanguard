using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.InputSystem;
namespace Vanguard
{
    public class Chat : NetworkBehaviour
    {
        string Message,Name;
        public Text chatText;
        bool onChat = false;
        RectMask2D mask;

        public void Start()
        {
            if (!isLocalPlayer)
            {
                chatText.gameObject.SetActive(false);
            }
            mask = chatText.GetComponentInParent<RectMask2D>();
            Name = GetComponent<Health>().name;
            chatText = NetworkClient.localPlayer.GetComponent<Chat>().chatText;
        }
        public void OnChatInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                print("in input"+ onChat);
                if (!onChat)
                {
                    Cursor.lockState = CursorLockMode.None;
                    mask.enabled = false;
                    onChat = true;
                    InputManager.SwitchActionMap("Chat");
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    mask.enabled = true;
                    onChat = false;
                    InputManager.SwitchActionMap("Vanguard (Pilot)");
                    Debug.Log(Message);
                    if (Message != "") CmdSendMessage(Name + Message+ "\n");
                }
            }
            
        }
        [ClientRpc]
        public void RpcReceiveMessage(string incomingMessage)
        {
                chatText.text += incomingMessage;
           
        }
        [Command]
        public void CmdSendMessage(string outgoingMessage)
        {
            RpcReceiveMessage(outgoingMessage);
        }
        public void OnMessageChange(string newMessage)
        {
            Message = newMessage;
        }
    }
}

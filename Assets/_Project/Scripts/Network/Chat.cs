using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using UnityEngine.UI;
using UnityEngine.InputSystem;
namespace Vanguard
{
    public class Chat : NetworkBehaviour
    {
        string Message,Name;
        public Text chatText;
        bool onChat = false;
        [SerializeField] RectMask2D mask;

        public override void OnStartClient()
        {
            if (!base.IsOwner)
            {
                chatText.gameObject.SetActive(false);
            }
            else
            {
                InputManager.OnChat += OnChatInput;
                Name = GetComponent<Health>().name;
            }
            base.OnStartClient();
            // Set the remote player's chatText to the local chatText, that way incoming chats
            // will be added to the local player's feed
            // chatText = NetworkClient.localPlayer.GetComponent<Chat>().chatText;
        }

        public void OnChatInput()
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
                if (Message != "") CmdSendMessage($"{Name}: {Message}\n");
            }
        }

        [ObserversRpc]
        public void RpcReceiveMessage(string incomingMessage)
        {
                chatText.text += incomingMessage;
        }

        [ServerRpc]
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

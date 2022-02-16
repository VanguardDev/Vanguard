using System;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vanguard
{
    /// <summary>
    /// A wrapper class to decouple in-editor input binding from input consumer classes.  Adds
    ///     static pure C# event-based input actions.  Static members are always from the LocalPlayer
    /// </summary>
    public class InputManager : NetworkBehaviour
    {
        private static InputManager _instance;

        public static Action OnJumpStarted;
        public static Action OnJumpStopped;
        public static Action OnCrouchStarted;
        public static Action OnCrouchStopped;

        public static Action OnShootStarted;
        public static Action OnShootStopped;
        public static Action OnReloadStarted;

        public static Action OnGrenadeStarted;
        public static Action OnGrenadeStopped;

        public static Action OnChangeWeapon;
        public static Action OnPickup;

        public static Action OnChat;

        public static Vector2 LookVector { get; private set; }
        public static Vector2 WalkVector { get; private set; }

        private PlayerInput playerInput;
        public static int wantedGunIndex;
        // public override void OnStartClient()
        // {
        //     base.OnStartClient();
        //     playerInput = GetComponent<PlayerInput>();
        //     playerInput.enabled = true;

        //     if (_instance != null)
        //     {
        //         Debug.LogError("Tried to initialize singleton InputManager, but it already exists!");
        //         Destroy(this);
        //         return;
        //     }

        //     _instance = this;
        // }

        public override void OnOwnershipClient(NetworkConnection prevOwner){
            playerInput = GetComponent<PlayerInput>();
            playerInput.enabled = true;

            if (_instance != null)
            {
                Debug.LogError("Tried to initialize singleton InputManager, but it already exists!");
                Destroy(this);
                return;
            }

            _instance = this;
            base.OnOwnershipClient(prevOwner);
        }

        public static void SwitchActionMap(string ActionMap)
        {
            _instance.playerInput.SwitchCurrentActionMap(ActionMap);
        }

        public static void EnableControls()
        {
            _instance.playerInput.ActivateInput();
        }

        public static void DisableControls()
        {
            _instance.playerInput.DeactivateInput();
        }

        public void OnLookInput(InputAction.CallbackContext context)
        {
            if (IsOwner)
            {
                LookVector = context.ReadValue<Vector2>();
            }
        }

        public void OnWalkInput(InputAction.CallbackContext context)
        {
            if (IsOwner)
            {
                WalkVector = context.ReadValue<Vector2>();
            }
        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
            if (IsOwner)
            {
                InvokeCancelableAction(context, OnJumpStarted, OnJumpStopped);
            }
        }

        public void OnCrouchInput(InputAction.CallbackContext context)
        {
            if (IsOwner)
            {
                InvokeCancelableAction(context, OnCrouchStarted, OnCrouchStopped);
            }
        }

        public void OnShootInput(InputAction.CallbackContext context)
        {
            if (IsOwner)
            {
                InvokeCancelableAction(context, OnShootStarted, OnShootStopped);
            }
        }

        public void OnReloadInput(InputAction.CallbackContext context)
        {
            if (IsOwner)
            {
                OnReloadStarted?.Invoke();
            }
        }

        public void OnGrenadeInput(InputAction.CallbackContext context)
        {
            if (IsOwner)
            {
                InvokeCancelableAction(context, OnGrenadeStarted, OnGrenadeStopped);
            }
        }

        public void OnChatInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnChat?.Invoke();
            }
        }
        public void OnWeaponSwitchInput(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                wantedGunIndex = (int)context.ReadValue<float>();
                OnChangeWeapon?.Invoke();
            }
        }
        public void OnPickUpInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnPickup.Invoke();
            }
        }

        /// <summary>
        /// Invokes <paramref name="performed"/> if the <paramref name="context"/> was performed,
        ///     or invokes <paramref name="canceled"/> if the <paramref name="context"/> was
        ///     canceled.
        /// </summary>
        /// <param name="context">The <see cref="InputAction.CallbackContext"/> of the action</param>
        /// <param name="performed">The action to call if the input was performed</param>
        /// <param name="canceled">The action to call if the input was canceled</param>
        private void InvokeCancelableAction(InputAction.CallbackContext context, Action performed, Action canceled)
        {
            if (context.performed)
            {
                performed?.Invoke();
            }
            else if (context.canceled)
            {
                canceled?.Invoke();
            }
        }
    }
}
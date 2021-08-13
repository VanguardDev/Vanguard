// GENERATED AUTOMATICALLY FROM 'Assets/_Project/Inputs/PilotActionControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Vanguard
{
    public class @PilotActionControls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PilotActionControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PilotActionControls"",
    ""maps"": [
        {
            ""name"": ""Vanguard (Pilot)"",
            ""id"": ""39915908-a7e5-4329-a1be-a0a912c1191d"",
            ""actions"": [
                {
                    ""name"": ""Walk"",
                    ""type"": ""Value"",
                    ""id"": ""9b9b2411-d052-417b-b190-e50df3321223"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""de302b41-e168-42e2-8ebb-f383e9a48398"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""8b036aba-7f1a-4616-80c6-6cac074655a6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""87f45ecc-c58d-438b-bd99-a9d9acbb56dd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse"",
                    ""type"": ""PassThrough"",
                    ""id"": ""595a4f7b-5009-47b4-9c8c-e8c48f639401"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""fca71019-275c-435e-9af8-3e5be89b058b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""c53ddce4-ca49-44ce-9249-312f45000d59"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""7b8de662-8e4b-4fd1-90aa-7561618a8eca"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6142caee-f448-414d-8344-7e493818861a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e5a82c70-2d60-4224-a213-3d10335bd105"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c898e02b-5452-469c-980b-9438ffdcee2b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f31457d2-1f12-4436-be6e-60921beafd2a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""6ca82c85-3943-4259-ae54-c1fcfced5325"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""209e20fc-937a-44be-9a9d-ea197e4779dd"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e9670eb1-0fa5-4245-b9e2-14c5884a34d0"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dbe73006-92b6-42c8-ac34-a644cad56f31"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eacd7a5b-c5f3-40d8-8df0-10f0e31b7464"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f5727b17-7f2d-4f11-b2c5-029e9917e975"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardMouse"",
            ""bindingGroup"": ""KeyboardMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Vanguard (Pilot)
            m_VanguardPilot = asset.FindActionMap("Vanguard (Pilot)", throwIfNotFound: true);
            m_VanguardPilot_Walk = m_VanguardPilot.FindAction("Walk", throwIfNotFound: true);
            m_VanguardPilot_Jump = m_VanguardPilot.FindAction("Jump", throwIfNotFound: true);
            m_VanguardPilot_Crouch = m_VanguardPilot.FindAction("Crouch", throwIfNotFound: true);
            m_VanguardPilot_Sprint = m_VanguardPilot.FindAction("Sprint", throwIfNotFound: true);
            m_VanguardPilot_Mouse = m_VanguardPilot.FindAction("Mouse", throwIfNotFound: true);
            m_VanguardPilot_Shoot = m_VanguardPilot.FindAction("Shoot", throwIfNotFound: true);
            m_VanguardPilot_Reload = m_VanguardPilot.FindAction("Reload", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Vanguard (Pilot)
        private readonly InputActionMap m_VanguardPilot;
        private IVanguardPilotActions m_VanguardPilotActionsCallbackInterface;
        private readonly InputAction m_VanguardPilot_Walk;
        private readonly InputAction m_VanguardPilot_Jump;
        private readonly InputAction m_VanguardPilot_Crouch;
        private readonly InputAction m_VanguardPilot_Sprint;
        private readonly InputAction m_VanguardPilot_Mouse;
        private readonly InputAction m_VanguardPilot_Shoot;
        private readonly InputAction m_VanguardPilot_Reload;
        public struct VanguardPilotActions
        {
            private @PilotActionControls m_Wrapper;
            public VanguardPilotActions(@PilotActionControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Walk => m_Wrapper.m_VanguardPilot_Walk;
            public InputAction @Jump => m_Wrapper.m_VanguardPilot_Jump;
            public InputAction @Crouch => m_Wrapper.m_VanguardPilot_Crouch;
            public InputAction @Sprint => m_Wrapper.m_VanguardPilot_Sprint;
            public InputAction @Mouse => m_Wrapper.m_VanguardPilot_Mouse;
            public InputAction @Shoot => m_Wrapper.m_VanguardPilot_Shoot;
            public InputAction @Reload => m_Wrapper.m_VanguardPilot_Reload;
            public InputActionMap Get() { return m_Wrapper.m_VanguardPilot; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(VanguardPilotActions set) { return set.Get(); }
            public void SetCallbacks(IVanguardPilotActions instance)
            {
                if (m_Wrapper.m_VanguardPilotActionsCallbackInterface != null)
                {
                    @Walk.started -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnWalk;
                    @Walk.performed -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnWalk;
                    @Walk.canceled -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnWalk;
                    @Jump.started -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnJump;
                    @Crouch.started -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnCrouch;
                    @Crouch.performed -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnCrouch;
                    @Crouch.canceled -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnCrouch;
                    @Sprint.started -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnSprint;
                    @Sprint.performed -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnSprint;
                    @Sprint.canceled -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnSprint;
                    @Mouse.started -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnMouse;
                    @Mouse.performed -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnMouse;
                    @Mouse.canceled -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnMouse;
                    @Shoot.started -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnShoot;
                    @Shoot.performed -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnShoot;
                    @Shoot.canceled -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnShoot;
                    @Reload.started -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnReload;
                    @Reload.performed -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnReload;
                    @Reload.canceled -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnReload;
                }
                m_Wrapper.m_VanguardPilotActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Walk.started += instance.OnWalk;
                    @Walk.performed += instance.OnWalk;
                    @Walk.canceled += instance.OnWalk;
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                    @Crouch.started += instance.OnCrouch;
                    @Crouch.performed += instance.OnCrouch;
                    @Crouch.canceled += instance.OnCrouch;
                    @Sprint.started += instance.OnSprint;
                    @Sprint.performed += instance.OnSprint;
                    @Sprint.canceled += instance.OnSprint;
                    @Mouse.started += instance.OnMouse;
                    @Mouse.performed += instance.OnMouse;
                    @Mouse.canceled += instance.OnMouse;
                    @Shoot.started += instance.OnShoot;
                    @Shoot.performed += instance.OnShoot;
                    @Shoot.canceled += instance.OnShoot;
                    @Reload.started += instance.OnReload;
                    @Reload.performed += instance.OnReload;
                    @Reload.canceled += instance.OnReload;
                }
            }
        }
        public VanguardPilotActions @VanguardPilot => new VanguardPilotActions(this);
        private int m_KeyboardMouseSchemeIndex = -1;
        public InputControlScheme KeyboardMouseScheme
        {
            get
            {
                if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardMouse");
                return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
            }
        }
        public interface IVanguardPilotActions
        {
            void OnWalk(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnCrouch(InputAction.CallbackContext context);
            void OnSprint(InputAction.CallbackContext context);
            void OnMouse(InputAction.CallbackContext context);
            void OnShoot(InputAction.CallbackContext context);
            void OnReload(InputAction.CallbackContext context);
        }
    }
}

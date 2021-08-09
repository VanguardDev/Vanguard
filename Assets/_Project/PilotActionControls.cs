// GENERATED AUTOMATICALLY FROM 'Assets/_Project/PilotActionControls.inputactions'

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
                    ""name"": ""Embark"",
                    ""type"": ""Button"",
                    ""id"": ""49d01264-d0c4-419a-8013-427e3faa0647"",
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
                    ""path"": ""<Keyboard>/leftCtrl"",
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
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41a15746-9a85-4681-81eb-dbedab324b7a"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Embark"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""BigRobots"",
            ""id"": ""13b181fa-72d0-410f-97de-2c21ea4934f1"",
            ""actions"": [
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""688e2e47-94ee-4b6d-bb46-6fb221b1d9f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Walk"",
                    ""type"": ""Value"",
                    ""id"": ""61f55c14-4b48-437e-a1fc-86d9a0b211ae"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1757f9c3-db18-4d4f-af31-56acd048ccb2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Disembark"",
                    ""type"": ""Button"",
                    ""id"": ""85dce9fe-e16d-4926-811f-364ba0cb0984"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a48d861d-c502-46a0-9b06-e178ffe8598a"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""5ead7565-eaa1-433b-b7ee-c3a04a8d2e45"",
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
                    ""id"": ""59a9341b-042b-4861-bcdd-3fe7b2080080"",
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
                    ""id"": ""fa6ebdee-1451-431c-b7fc-c8b9b1c00da3"",
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
                    ""id"": ""af1142d2-cd6a-42ba-82e6-a0e6b18d76d4"",
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
                    ""id"": ""4489ab2a-4593-4dbb-b7ac-65c24d67fbf8"",
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
                    ""id"": ""9c3b2090-2c17-40ab-b8f5-b025035bd5cd"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""718154f9-f5a4-40b3-a2e1-ce7f1d19a4b2"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Disembark"",
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
            m_VanguardPilot_Embark = m_VanguardPilot.FindAction("Embark", throwIfNotFound: true);
            // BigRobots
            m_BigRobots = asset.FindActionMap("BigRobots", throwIfNotFound: true);
            m_BigRobots_Dash = m_BigRobots.FindAction("Dash", throwIfNotFound: true);
            m_BigRobots_Walk = m_BigRobots.FindAction("Walk", throwIfNotFound: true);
            m_BigRobots_Mouse = m_BigRobots.FindAction("Mouse", throwIfNotFound: true);
            m_BigRobots_Disembark = m_BigRobots.FindAction("Disembark", throwIfNotFound: true);
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
        private readonly InputAction m_VanguardPilot_Embark;
        public struct VanguardPilotActions
        {
            private @PilotActionControls m_Wrapper;
            public VanguardPilotActions(@PilotActionControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Walk => m_Wrapper.m_VanguardPilot_Walk;
            public InputAction @Jump => m_Wrapper.m_VanguardPilot_Jump;
            public InputAction @Crouch => m_Wrapper.m_VanguardPilot_Crouch;
            public InputAction @Sprint => m_Wrapper.m_VanguardPilot_Sprint;
            public InputAction @Mouse => m_Wrapper.m_VanguardPilot_Mouse;
            public InputAction @Embark => m_Wrapper.m_VanguardPilot_Embark;
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
                    @Embark.started -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnEmbark;
                    @Embark.performed -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnEmbark;
                    @Embark.canceled -= m_Wrapper.m_VanguardPilotActionsCallbackInterface.OnEmbark;
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
                    @Embark.started += instance.OnEmbark;
                    @Embark.performed += instance.OnEmbark;
                    @Embark.canceled += instance.OnEmbark;
                }
            }
        }
        public VanguardPilotActions @VanguardPilot => new VanguardPilotActions(this);

        // BigRobots
        private readonly InputActionMap m_BigRobots;
        private IBigRobotsActions m_BigRobotsActionsCallbackInterface;
        private readonly InputAction m_BigRobots_Dash;
        private readonly InputAction m_BigRobots_Walk;
        private readonly InputAction m_BigRobots_Mouse;
        private readonly InputAction m_BigRobots_Disembark;
        public struct BigRobotsActions
        {
            private @PilotActionControls m_Wrapper;
            public BigRobotsActions(@PilotActionControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Dash => m_Wrapper.m_BigRobots_Dash;
            public InputAction @Walk => m_Wrapper.m_BigRobots_Walk;
            public InputAction @Mouse => m_Wrapper.m_BigRobots_Mouse;
            public InputAction @Disembark => m_Wrapper.m_BigRobots_Disembark;
            public InputActionMap Get() { return m_Wrapper.m_BigRobots; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(BigRobotsActions set) { return set.Get(); }
            public void SetCallbacks(IBigRobotsActions instance)
            {
                if (m_Wrapper.m_BigRobotsActionsCallbackInterface != null)
                {
                    @Dash.started -= m_Wrapper.m_BigRobotsActionsCallbackInterface.OnDash;
                    @Dash.performed -= m_Wrapper.m_BigRobotsActionsCallbackInterface.OnDash;
                    @Dash.canceled -= m_Wrapper.m_BigRobotsActionsCallbackInterface.OnDash;
                    @Walk.started -= m_Wrapper.m_BigRobotsActionsCallbackInterface.OnWalk;
                    @Walk.performed -= m_Wrapper.m_BigRobotsActionsCallbackInterface.OnWalk;
                    @Walk.canceled -= m_Wrapper.m_BigRobotsActionsCallbackInterface.OnWalk;
                    @Mouse.started -= m_Wrapper.m_BigRobotsActionsCallbackInterface.OnMouse;
                    @Mouse.performed -= m_Wrapper.m_BigRobotsActionsCallbackInterface.OnMouse;
                    @Mouse.canceled -= m_Wrapper.m_BigRobotsActionsCallbackInterface.OnMouse;
                    @Disembark.started -= m_Wrapper.m_BigRobotsActionsCallbackInterface.OnDisembark;
                    @Disembark.performed -= m_Wrapper.m_BigRobotsActionsCallbackInterface.OnDisembark;
                    @Disembark.canceled -= m_Wrapper.m_BigRobotsActionsCallbackInterface.OnDisembark;
                }
                m_Wrapper.m_BigRobotsActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Dash.started += instance.OnDash;
                    @Dash.performed += instance.OnDash;
                    @Dash.canceled += instance.OnDash;
                    @Walk.started += instance.OnWalk;
                    @Walk.performed += instance.OnWalk;
                    @Walk.canceled += instance.OnWalk;
                    @Mouse.started += instance.OnMouse;
                    @Mouse.performed += instance.OnMouse;
                    @Mouse.canceled += instance.OnMouse;
                    @Disembark.started += instance.OnDisembark;
                    @Disembark.performed += instance.OnDisembark;
                    @Disembark.canceled += instance.OnDisembark;
                }
            }
        }
        public BigRobotsActions @BigRobots => new BigRobotsActions(this);
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
            void OnEmbark(InputAction.CallbackContext context);
        }
        public interface IBigRobotsActions
        {
            void OnDash(InputAction.CallbackContext context);
            void OnWalk(InputAction.CallbackContext context);
            void OnMouse(InputAction.CallbackContext context);
            void OnDisembark(InputAction.CallbackContext context);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Vanguard;
using UnityEngine;

public class FPInput : MonoBehaviour
{
    public PilotActionControls Actions { get; private set; }
    public Vector2 LookVector { get; private set; }
    public Vector2 WalkVector { get; private set; }

    void Awake() {
        Actions = new PilotActionControls();
    }

    void Update() {
        if (Actions == null)
            Actions = new PilotActionControls();

        LookVector = Actions.VanguardPilot.Mouse.ReadValue<Vector2>();
        WalkVector = Actions.VanguardPilot.Walk.ReadValue<Vector2>();
    }

    private void OnEnable() {
        EnableControls();
    }

    private void OnDisable() {
        DisableControls();
    }

    public void EnableControls() {
        Actions?.Enable();
    }
    
    public void DisableControls() {
        Actions?.Disable();
    }
}

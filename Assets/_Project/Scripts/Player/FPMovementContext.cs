using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPMovementContext : MonoBehaviour
{
    private FPMovementState state;
    public FPMovementState State { 
        get { return state; } 
        set { 
            state = value;
        }
    }

    public FPInput Inputs { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public CapsuleCollider Collider { get; private set; }

    public void Start() {
        var initialState = new FPIdleState();
        initialState.Context = this;

        State = initialState;
        Rigidbody = GetComponent<Rigidbody>();
        Inputs = GetComponent<FPInput>();
        Collider = GetComponent<CapsuleCollider>();
    }

    public void Update() {
        state.StateChangeCheck();
        state.Update();
    }

    public void FixedUpdate() {
        state.PhysicsUpdate();
    }
}
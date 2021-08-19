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
            if (state != null)
                Debug.Log(state.GetType().Name);
        }
    }

    [HideInInspector]
    public FPInput Inputs { get; private set; }

    [HideInInspector]
    public Rigidbody Rigidbody { get; private set; }

    [HideInInspector]
    public CapsuleCollider Collider { get; private set; }

    [HideInInspector]
    public RaycastHit groundHit;

    [SerializeField]
    private float groundCheckDistance = 0.005f;

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

    public bool GroundCheck() {
        return Physics.Raycast(transform.position, Vector3.down, out groundHit, (Collider.height / 2) + groundCheckDistance);
    }
}
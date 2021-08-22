using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPMovementContext : MonoBehaviour
{
    public float speed;
    [SerializeField]
    private float groundCheckDistance = 0.005f;

    public int maxJumpCount = 2;
    public float jumpVelocity = 5;

    [HideInInspector]
    public int jumpCount = 0;

    [SerializeField]
    public float wallrunBoost = 1.3f;
    public float wallrunAwayVelocity = 3f;
    public float maxWallrunBoostVelocity = 11.1f;
    public float wallrunMinVelocity = 3.53333f;

    private FPMovementState state;
    [HideInInspector]
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

    [HideInInspector]
    public RaycastHit wallHit;

    public void Start() {
        var initialState = new FPMovementState.FPIdleState();
        initialState.Context = this;
        initialState.Enter();

        State = initialState;
        Rigidbody = GetComponent<Rigidbody>();
        Inputs = GetComponent<FPInput>();
        Collider = GetComponent<CapsuleCollider>();

        Inputs.Actions.VanguardPilot.Jump.performed += Jump;
    }

    public void Update() {
        state.Update();
        speed = Vector3.Scale(Rigidbody.velocity, new Vector3(1, 0, 1)).magnitude;
    }

    public void FixedUpdate() {
        state.StateChangeCheck();
        state.PhysicsUpdate();
    }

    public void Jump(InputAction.CallbackContext context) {
        if (jumpCount < maxJumpCount) {
            state.Jump();
            state.StateChangeCheck(); 
            jumpCount++;
        }
    }

    public bool GroundCheck() {
        bool retval = Physics.Raycast(transform.position, Vector3.down, out groundHit, ((Collider.height / 2) * transform.localScale.y) + groundCheckDistance);
        return retval;
    }

    public bool WallCheck() {
        bool retval = Physics.Raycast(transform.position, transform.right, out wallHit, (Collider.radius / 2) + 0.6f) ||
                      Physics.Raycast(transform.position, -transform.right, out wallHit, (Collider.radius / 2) + 0.6f);
        return retval && Mathf.Abs(Vector3.Dot(Vector3.Cross(wallHit.normal, Vector3.up), transform.forward)) > 0.3f;
    }
}
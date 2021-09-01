using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPMovementContext : MonoBehaviour
{   
    [Header("Walk Settings")]
    public float walkSpeed = 8f;
    private float groundCheckDistance = 0.1f;
    
    [Header("Jump Settings")]
    public int maxJumpCount = 2;
    public float jumpVelocity = 8;

    [HideInInspector]
    public int jumpCount = 0;

    [Header("Wallrun Settings")]
    public float wallrunBoost = 1.4f;
    public float wallrunBoostThreshold = 12f;
    public float wallrunAwayBoost = 1.1f;
    public float wallrunAwayBoostThreshold = 13f;
    public bool wallrunEnabled = true;

    [Header("Slide Settings")]
    public float slideBoost = 1.2f;
    public float slideBoostThreshold = 17;
    // [HideInInspector]
    // public float wallrunMinVelocity = 3.53333f;

    private FPMovementState state;
    [HideInInspector]
    public FPMovementState State { 
        get { return state; } 
        set { 
            state = value;
           // if (state != null)
                //Debug.Log(state.GetType().Name);
        }
    }

    [HideInInspector]
    public FPInput Inputs { get; private set; }

    [HideInInspector]
    public Rigidbody Rigidbody { get; private set; }

    [HideInInspector]
    public CapsuleCollider Collider { get; private set; }

    [HideInInspector]
    public FirstPersonLook Look { get; private set; }

    [HideInInspector]
    public RaycastHit groundHit;

    [HideInInspector]
    public RaycastHit wallHit;

    [HideInInspector]
    public bool IsCrouching { get; private set; }
    [HideInInspector]
    public bool IsJumping;

    public void Start() {
        var initialState = new FPMovementState.FPIdleState();
        initialState.Context = this;
        initialState.Enter();

        State = initialState;
        Rigidbody = GetComponent<Rigidbody>();
        Inputs = GetComponent<FPInput>();
        Collider = GetComponent<CapsuleCollider>();
        Look = GetComponent<FirstPersonLook>();

        Inputs.Actions.VanguardPilot.Jump.performed += Jump;
        Inputs.Actions.VanguardPilot.Jump.canceled += CancelJump;
        
        Inputs.Actions.VanguardPilot.Crouch.performed += Crouch;
        Inputs.Actions.VanguardPilot.Crouch.canceled += CancelCrouch;
    }

    public void Update() {
        state.Update();
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
            IsJumping = true;
        }
    }

    public void CancelJump(InputAction.CallbackContext context) {
        IsJumping = false;
    }

    public void Crouch(InputAction.CallbackContext context) {
        state.Crouch();
        state.StateChangeCheck(); 
        IsCrouching = true;
    }

    public void CancelCrouch(InputAction.CallbackContext context) {
        IsCrouching = false;
    }

    public bool GroundCheck() {
        bool retval = Physics.Raycast(transform.position, Vector3.down, out groundHit, (Collider.height/2 * transform.localScale.y) + groundCheckDistance);
        return retval;
    }

    public void BreakFromWall(float delay=0.2f) {
        wallrunEnabled = false;
        Invoke("EnableWallrun", delay);
    }

    public void EnableWallrun() {
        wallrunEnabled = true;
    }


    public bool WallCheck() {
        bool retval = Physics.Raycast(transform.position, transform.right, out wallHit, Collider.radius + 0.9f) ||
                      Physics.Raycast(transform.position, -transform.right, out wallHit, Collider.radius + 0.9f);
        if (retval)
            Debug.Log(wallHit.normal.y);
        return retval && wallrunEnabled && Mathf.Abs(wallHit.normal.y) < 0.1f;// && Mathf.Abs(Vector3.Dot(Vector3.Cross(wallHit.normal, Vector3.up), transform.forward)) > 0.3f;
    }

    void OnGUI()
    {
        if (Application.isEditor)
        {
            GUI.Label(new Rect(10, 200, 100, 20), Vector3.Scale(Rigidbody.velocity, new Vector3(1, 0, 1)).magnitude.ToString());
        }
    }
}
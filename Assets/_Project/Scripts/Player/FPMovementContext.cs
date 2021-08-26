using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPMovementContext : MonoBehaviour
{
    public float speed;
    public float walkSpeed = 20f;

    [SerializeField]
    private float groundCheckDistance = 0.005f;

    public int maxJumpCount = 2;
    public float jumpVelocity = 5;

    [HideInInspector]
    public int jumpCount = 0;

    [SerializeField]
    public float wallrunBoost = 1.3f;
    public float wallrunAwayBoost = 1.2f;
    public float wallrunAwayBoostThreshold = 15f;
    public float maxWallrunBoostVelocity = 11.1f;
    // [HideInInspector]
    // public float wallrunMinVelocity = 3.53333f;

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
    public FirstPersonLook Look { get; private set; }

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
        Look = GetComponent<FirstPersonLook>();

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
        bool retval = Physics.Raycast(transform.position, Vector3.down, out groundHit, (Collider.height/2 * transform.localScale.y) + groundCheckDistance);
        return retval;
    }

    public bool WallCheck() {
        bool retval = Physics.Raycast(transform.position, transform.right, out wallHit, Collider.radius + 0.6f) ||
                      Physics.Raycast(transform.position, -transform.right, out wallHit, Collider.radius + 0.6f);
        return retval;// && Mathf.Abs(Vector3.Dot(Vector3.Cross(wallHit.normal, Vector3.up), transform.forward)) > 0.3f;
    }

    void OnGUI()
    {
        if (Application.isEditor)
        {
            GUI.Label(new Rect(10, 200, 100, 20), Vector3.Scale(Rigidbody.velocity, new Vector3(1, 0, 1)).magnitude.ToString());
        }
    }
}
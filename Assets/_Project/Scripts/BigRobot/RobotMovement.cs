using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotMovement : MonoBehaviour{
    [SerializeField] public Vanguard.PilotActionControls input;
    [Header("Walk Settings")]
    public float speed = 5f;
    public float acceleration = 0.1f;

    private Vector3 walkVector;
    private Vector3 immediateWalkVector;

    [Header("Dash Settings")]
    public float DashImpulse = 1000f;
    public float DashCooldown = 5f;
    private bool canDash = true;

    [Header("Components")]
    private Rigidbody rb;
    private CapsuleCollider bodyCollider;
    private FirstPersonLook camManager;

    private Vector2 targetMoveInputVector;
    private Vector3 moveInputVector;

    private Vector2 movementInput;


    private float moveSpeed;


    IEnumerator DashCooldownHandler(){
        canDash = false;
        yield return new WaitForSeconds(DashCooldown);
    }

    private void Awake() {
        input = new Vanguard.PilotActionControls();
        
        input.BigRobots.Walk.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        input.BigRobots.Walk.canceled += ctx => movementInput = Vector2.zero;

        input.BigRobots.Dash.performed += ctx => Dash();
    }

    void Start(){
        rb = GetComponent<Rigidbody>();
        camManager = GetComponent<FirstPersonLook>();
        bodyCollider = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate() {
        MyInput();
    }

    void MyInput(){
        var horizontalMovement = movementInput.x;
        var verticalMovement = movementInput.y;

        

        Vector3 moveDirection = (rb.transform.forward * verticalMovement) + 
        (rb.transform.right * horizontalMovement );

        moveSpeed = Mathf.Lerp(moveSpeed, speed, acceleration * Time.deltaTime);

        rb.AddForce(moveDirection.normalized * speed, ForceMode.Acceleration);
    }

    public void Dash(){
        if(canDash){
            rb.AddForce( rb.velocity * DashImpulse, ForceMode.Impulse);
            StartCoroutine(DashCooldownHandler());
        }
    }

    private void OnEnable() {
        input.Enable();
    }
    private void OnDisable() {
        input.Disable();
    }

    public void Sync(){
        Awake();
    }
}

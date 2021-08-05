using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementState {
    Idle,
    Walking,
    Falling,
    Wallrunning
}

public enum WallrunState {
    None,
    Left,
    Right
}
public class FirstPersonMove : MonoBehaviour
{
    [Header("Walk Settings")]
    public float speed = 5f;
    public float acceleration = 0.1f;

    private bool isGrounded;
    private Vector3 walkVector;
    private Vector3 immediateWalkVector;
    private RaycastHit groundHit;

    [Header("Jump Settings")]
    public float jumpVelocity = 50f;
    public int maxJumpCount = 2;
    public float airstrafeMultiplier = 0.5f;
    private int jumpCount;

    [Header("Wallrun Settings")]
    public WallrunState wallrunState = WallrunState.None;
    public float wallrunJumpVelocity = 3;

    private Vector3 wallNormal;
    private Vector3 wallDirection;
    private float wallrunSpeed;
    private float wallrunTime;
    private bool wallRunEnabled = true;
    private RaycastHit wallHit;

    [Header("Misc")]
    [SerializeField]
    public MovementState movementState = MovementState.Idle;
    public LayerMask environmentMask;
    private Rigidbody rb;
    private FirstPersonLook camManager;

    private Vector2 targetMoveInputVector;
    private Vector3 moveInputVector;
    public void UpdateMoveInput(InputAction.CallbackContext context) {
        targetMoveInputVector = context.ReadValue<Vector2>();
    }

    private bool inputJump;
    public void UpdateJumpInput(InputAction.CallbackContext context) {
        var newValue = context.ReadValue<float>() == 1;

        if (newValue != inputJump) {
            if (newValue == true && jumpCount < maxJumpCount - 1) {
                
                if (wallrunState != WallrunState.None) {
                    jumpCount = -1; // so fucking jank :D
                    Vector3 fwd = transform.forward * wallrunSpeed;
                    fwd.y = jumpVelocity;
                    rb.velocity = fwd + (wallNormal * wallrunJumpVelocity);

                    ExitWallRun();
                }
                else {
                    if (Vector3.Distance(walkVector, Vector3.zero) < 0.5f)
                        rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
                    else
                        rb.velocity = new Vector3(immediateWalkVector.x, jumpVelocity, immediateWalkVector.z);
                }

                jumpCount++;
            }
        }
        inputJump = newValue;
    }

    void Start() {
        rb = GetComponent<Rigidbody>();
        camManager = GetComponent<FirstPersonLook>();
    }

    void Update() {
    }

    void FixedUpdate() {
        moveInputVector = Vector2.Lerp(moveInputVector, targetMoveInputVector, Time.fixedDeltaTime * 1/acceleration);
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out groundHit, .53f, environmentMask);
        WallRunCheck();
        
        Vector3 targetVelocity = rb.velocity;

        walkVector = Vector3.Cross(transform.TransformDirection(new Vector3(moveInputVector.y, 0, -moveInputVector.x)), isGrounded ? groundHit.normal : Vector3.up) * speed;
        immediateWalkVector = Vector3.Cross(transform.TransformDirection(new Vector3(targetMoveInputVector.y, 0, -targetMoveInputVector.x)), isGrounded ? groundHit.normal : Vector3.up) * speed;
        
        if (isGrounded) {
            jumpCount = 0;
            targetVelocity = walkVector;
            targetVelocity.y = rb.velocity.y;
        }
        else if (wallrunState == WallrunState.None) {
            targetVelocity = (rb.velocity + (immediateWalkVector.normalized * airstrafeMultiplier)).normalized * rb.velocity.magnitude;
            targetVelocity.y = rb.velocity.y;
        }
        else {
            jumpCount = 0;
            targetVelocity = -(wallNormal * Vector3.Distance(wallHit.point, transform.position)) + (wallDirection * wallrunSpeed * Mathf.Lerp(1.3f, 1, Mathf.Min(wallrunTime, 1)));
            targetVelocity.y = rb.velocity.y / 2;
        }

        //Debug.DrawLine(transform.position, transform.position + wallDirection, Color.red);
        rb.velocity = targetVelocity;
    }

    float wallrunCheckLength = 0.6f;
    void WallRunCheck() {
        Vector3 wallrunForwardOffset = transform.forward / 4;

        if (wallRunEnabled) {
            RaycastHit rightHit;
            bool rightCheck = (wallrunState == WallrunState.None && Physics.Raycast(transform.position, transform.right - wallrunForwardOffset, out rightHit, wallrunCheckLength, environmentMask)) ||
                        Physics.Raycast(transform.position, transform.right + wallrunForwardOffset, out rightHit, wallrunCheckLength, environmentMask) ||
                        Physics.Raycast(transform.position, transform.right, out rightHit, wallrunCheckLength, environmentMask);
            
            RaycastHit leftHit;
            bool leftCheck = (wallrunState == WallrunState.None && Physics.Raycast(transform.position, -transform.right - wallrunForwardOffset, out leftHit, wallrunCheckLength, environmentMask)) ||
                        Physics.Raycast(transform.position, -transform.right + wallrunForwardOffset, out leftHit, wallrunCheckLength, environmentMask) ||
                        Physics.Raycast(transform.position, -transform.right, out leftHit, wallrunCheckLength, environmentMask);

            if ((rightCheck || leftCheck) && !isGrounded) {
                if (wallrunState == WallrunState.None) {
                    wallrunSpeed = rb.velocity.magnitude;
                    wallrunTime = 0;
                }

                // TODO: add camera lock to normal
                if (leftCheck && Mathf.Abs(Vector3.Dot(leftHit.normal, Vector3.up)) < 0.1f) {
                    if (wallrunState == WallrunState.None)
                        wallrunState = WallrunState.Left;
                    wallNormal = leftHit.normal;
                    wallDirection = Vector3.Cross(wallNormal, Vector3.up);
                    wallHit = leftHit;

                }
                else if (rightCheck && Mathf.Abs(Vector3.Dot(rightHit.normal, Vector3.up)) < 0.1f) {
                    if (wallrunState == WallrunState.None)
                        wallrunState = WallrunState.Right;
                    wallNormal = rightHit.normal;
                    wallDirection = Vector3.Cross(wallNormal, Vector3.up);
                    wallHit = rightHit;
                }
                wallrunTime += Time.fixedDeltaTime;
                wallDirection *= Vector3.Dot(transform.forward, wallDirection);
            }
            else if (!rightCheck && !leftCheck) {
                if (wallrunState != WallrunState.None) {
                    wallrunState = WallrunState.None;
                    ExitWallRun();
                }
                wallNormal = Vector3.zero;
                wallDirection = Vector3.zero;
            }
        } 
        else {
            wallrunState = WallrunState.None;
            wallNormal = Vector3.zero;
            wallDirection = Vector3.zero;
        }
    }

    void EnableWallRun() {
        wallRunEnabled = true;
        camManager.targetDutch = 0;

    }
    void ExitWallRun() {
        wallRunEnabled = false;
        Invoke("EnableWallRun", 0.5f);
    }
}

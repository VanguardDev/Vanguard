using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Vanguard;

public enum CrouchState {
    None,
    Queued,
    Sneak,
    Slide
}

public enum WallrunState {
    None,
    Left,
    Right,
    Climb
}

public struct WallCheck
{
    public WallrunState State;
    public bool Hit;
    public RaycastHit HitInfo;
}

public class FirstPersonMove : NetworkBehaviour
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
    public float baseWallrunSpeed = 4;
    public float wallrunJumpVelocity = 3;

    public float minClimbDistance = 1;
    public AnimationCurve climbVelCurve;

    private Vector3 wallNormal;
    private Vector3 wallDirection;
    private float wallrunSpeed;
    private float wallrunTime;
    private bool wallRunEnabled = true;
    private RaycastHit wallHit;
    private float wallrunCheckLength;

    public Transform climbTransform;


    [Header("Crouch Settings")]
    public CrouchState crouchState = CrouchState.None;
    public float slideVelThresh = 2f;
    public float sneakSpeed = 3f;
    public float slideVelBoost = 1.3f;
    public float baseInitialSlideVel = 4f;
    public float minSlideTime = 0.3f;
    public float maxInitialSlideVel = 8f;
    public float slideJumpVelMult = .7f;
    public float crouchColliderHeight = 1.5f;

    public float crouchTime = 0f;

    [Header("Misc")]
    [SerializeField]
    public LayerMask environmentMask;

    private Rigidbody rb;
    private CapsuleCollider collider;
    private FirstPersonLook camManager;

    private Vector2 targetMoveInputVector;
    private Vector3 moveInputVector;
    private bool inputJump;

    public void Jump(InputAction.CallbackContext context) {
        var newValue = context.ReadValue<float>() == 1;

        if (newValue != inputJump) {
            if (newValue == true && jumpCount < maxJumpCount) {
                if (wallrunState != WallrunState.None) {
                    climbTransform = null;
                    Vector3 fwd = transform.forward * wallrunSpeed;
                    if (wallrunState == WallrunState.Climb)
                        fwd *= 0;

                    fwd.y = jumpVelocity;
                    rb.velocity = fwd + (wallNormal * wallrunJumpVelocity);
                    ExitWallrun();
                }
                else {
                    Vector3 jumpDir = rb.velocity;
                    if (Vector3.Distance(immediateWalkVector, Vector3.zero) > 0.5f && crouchState != CrouchState.Slide)
                        jumpDir = immediateWalkVector / speed * rb.velocity.magnitude;
                    jumpDir.y = jumpVelocity;
                    if (crouchState == CrouchState.Slide)
                        jumpDir.y *= slideJumpVelMult;

                    rb.velocity = jumpDir;
                }

                jumpCount++;
            }
        }
        inputJump = newValue;
    }

    private bool inputCrouch;
    public void Crouch(InputAction.CallbackContext context) {
        var newValue = context.ReadValue<float>() == 1;

        if (newValue != inputCrouch) {
            if (newValue == true) {
                if (wallrunState != WallrunState.None) {
                    if (wallrunState == WallrunState.Climb)
                        rb.velocity = Vector3.zero;
                    else
                        rb.velocity = (wallDirection.normalized * rb.velocity.magnitude) + wallNormal;
                    ExitWallrun();
                }
                if (isGrounded) {
                    StartCrouch();
                } else {
                    crouchState = CrouchState.Queued;
                }
            }
            else {
                crouchState = CrouchState.None;
                camManager.targetHeight = 1;
            }
        }
        inputCrouch = newValue;
    }
    private FPInput input;
    void Start()
    {
        input = GetComponent<FPInput>();
        if (!isLocalPlayer)
        {
            GetComponentInChildren<Canvas>().enabled = false;
            input.DisableControls();
        }
        else
        {
            input.Actions.VanguardPilot.Jump.performed += Jump;
            input.Actions.VanguardPilot.Jump.canceled += Jump;
            input.Actions.VanguardPilot.Crouch.performed += Crouch;
            input.Actions.VanguardPilot.Crouch.canceled += Crouch;
        }

        rb = GetComponent<Rigidbody>();
        camManager = GetComponent<FirstPersonLook>();
        collider = GetComponent<CapsuleCollider>();
        wallrunCheckLength = collider.radius*gameObject.transform.localScale.x*3.3f;
    }

    void FixedUpdate() {
        if (!isLocalPlayer)
        {
            return;
        }

        targetMoveInputVector = input.WalkVector;
        moveInputVector = Vector2.Lerp(moveInputVector, targetMoveInputVector, Time.fixedDeltaTime * 1/acceleration);
        bool newIsGrounded = Physics.Raycast(transform.position, Vector3.down, out groundHit, (transform.localScale.y * collider.height) * 0.75f, environmentMask);
        if (newIsGrounded != isGrounded) {
            if (newIsGrounded)
                OnEnterGround();
            else
                OnExitGround();
        }
        isGrounded = newIsGrounded;
        WallRunCheck();
        
        Vector3 targetVelocity = rb.velocity;

        walkVector = Vector3.Cross(transform.TransformDirection(new Vector3(moveInputVector.y, 0, -moveInputVector.x)), isGrounded ? groundHit.normal : Vector3.up) * speed;
        immediateWalkVector = Vector3.Cross(transform.TransformDirection(new Vector3(targetMoveInputVector.y, 0, -targetMoveInputVector.x)), isGrounded ? groundHit.normal : Vector3.up) * speed;
        
        if (isGrounded) {
            if (crouchState != CrouchState.Slide) {
                targetVelocity = crouchState == CrouchState.Sneak ? (walkVector / speed) * sneakSpeed : walkVector;
                targetVelocity.y = rb.velocity.y;
            }
        }
        else {
            switch (wallrunState) {
                case WallrunState.None:
                    targetVelocity = (rb.velocity + (immediateWalkVector.normalized * airstrafeMultiplier)).normalized * rb.velocity.magnitude;
                    targetVelocity.y = rb.velocity.y;
                    break;
                case WallrunState.Left:
                case WallrunState.Right:
                    targetVelocity = -(wallNormal * Vector3.Distance(wallHit.point, transform.position)) + (wallDirection * wallrunSpeed * Mathf.Lerp(1.3f, 1, Mathf.Min(wallrunTime, 1)));
                    targetVelocity.y = rb.velocity.y / 2;
                    break;
                case WallrunState.Climb:
                    if (wallrunTime < climbVelCurve.keys[climbVelCurve.length - 1].time) {
                        targetVelocity = -wallNormal + (Vector3.up * 3 * climbVelCurve.Evaluate(wallrunTime));
                    } else {
                        wallrunState = WallrunState.None;
                        ExitWallrun();
                    }
                    break;
            }
        }

        //Debug.DrawLine(transform.position, transform.position + wallDirection, Color.red);
        if (crouchState != CrouchState.Slide) {
            rb.velocity = targetVelocity;
        }
        else if (crouchTime > minSlideTime){
            Vector3 velocityXZ = Vector3.Scale(rb.velocity, new Vector3(1, 0, 1));
            if (velocityXZ.magnitude < slideVelThresh)
                crouchState = CrouchState.Sneak;
        }

        if (crouchState == CrouchState.Slide || crouchState == CrouchState.Sneak) {
            camManager.targetHeight = 0.5f;
            crouchTime += Time.fixedDeltaTime;
        }

    }

    void WallRunCheck() {
        Vector3 wallrunForwardOffset = transform.forward / 4;

        if (wallRunEnabled && !isGrounded) {
            // REALLY WANT TO FIND A WAY TO DO THIS WITHOUT ALL THE RAYCASTS >:(
            
            RaycastHit rightHit;
            bool rightCheck = (wallrunState == WallrunState.None && Physics.Raycast(transform.position, transform.right - wallrunForwardOffset, out rightHit, wallrunCheckLength, environmentMask)) ||
                Physics.Raycast(transform.position, transform.right + wallrunForwardOffset, out rightHit, wallrunCheckLength, environmentMask) ||
                Physics.Raycast(transform.position, transform.right, out rightHit, wallrunCheckLength, environmentMask);
            
            RaycastHit leftHit;
            bool leftCheck = (wallrunState == WallrunState.None && Physics.Raycast(transform.position, -transform.right - wallrunForwardOffset, out leftHit, wallrunCheckLength, environmentMask)) ||
                Physics.Raycast(transform.position, -transform.right + wallrunForwardOffset, out leftHit, wallrunCheckLength, environmentMask) ||
                Physics.Raycast(transform.position, -transform.right, out leftHit, wallrunCheckLength, environmentMask);
            
            RaycastHit fwdHit;
            bool fwdCheck = Physics.Raycast(transform.position, transform.forward, out fwdHit, wallrunCheckLength, environmentMask) || 
                (wallrunState != WallrunState.None && Physics.Raycast(transform.position - (Vector3.up * ((transform.localScale.y * collider.height) - collider.radius)), transform.forward, out fwdHit, wallrunCheckLength-0.1f, environmentMask));

            List<WallCheck> checks = new List<WallCheck>() {
                new WallCheck() { State = WallrunState.Left, Hit = leftCheck, HitInfo = leftHit },
                new WallCheck() { State = WallrunState.Right, Hit = rightCheck, HitInfo = rightHit },
                new WallCheck() { State = WallrunState.Climb, Hit = fwdCheck, HitInfo = fwdHit },
            };

            if (checks.Any(check => check.Hit)) {
                if (wallrunState == WallrunState.None) {
                    wallrunSpeed = Mathf.Max(rb.velocity.magnitude, baseWallrunSpeed);
                    wallrunTime = 0;
                }
                
                WallrunState oldState = wallrunState;

                // TODO: add camera lock to normal
                foreach (WallCheck check in checks) {
                    if (check.Hit && Mathf.Abs(Vector3.Dot(check.HitInfo.normal, Vector3.up)) < 0.1f) {
                        if (wallrunState == WallrunState.None)
                            wallrunState = check.State;
                        wallNormal = check.HitInfo.normal;
                        wallDirection = Vector3.Cross(wallNormal, Vector3.up);
                        wallHit = check.HitInfo;
                    }
                }
                if (wallrunState != WallrunState.Climb) {
                    wallDirection = Vector3.Cross(wallNormal, Vector3.up);
                    camManager.targetDutch = -15 * Vector3.Dot(transform.forward, wallDirection);
                    wallDirection *= Vector3.Dot(transform.forward, wallDirection);

                    OnEnterWallrun();
                }
                else {
                    if (oldState != WallrunState.None || (climbTransform == null && oldState == WallrunState.None)) {
                        //camManager.SetLookEnabled(false);
                        //Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.z), Color.red);
                        climbTransform = wallHit.transform;
                        camManager.xRotation = Mathf.Lerp(camManager.xRotation, 10, Time.fixedDeltaTime * 10);
                        camManager.cam.transform.rotation = Quaternion.Euler(new Vector3(camManager.xRotation, camManager.yRotation, 0));
                    }
                    else {
                        wallrunState = WallrunState.None;
                        ExitWallrun();
                    }
                }

                wallrunTime += Time.fixedDeltaTime;
            }
            else {
                if (wallrunState != WallrunState.None) {
                    wallrunState = WallrunState.None;
                    ExitWallrun();
                }
            }
        } 
        else {
            if (wallrunState != WallrunState.None) {
                wallrunState = WallrunState.None;
                ExitWallrun();
            }
        }

        if (wallrunState == WallrunState.None)
            camManager.targetDutch = 0;
    }

    void OnEnterGround() {
        jumpCount = 0;
        climbTransform = null;
        if (crouchState == CrouchState.Queued) {
            StartCrouch();
        }
    }

    void OnExitGround() {
        if (crouchState != CrouchState.None) {
            crouchState = CrouchState.Queued;
            camManager.targetHeight = 1;
        }
    }

    void EnableWallrun() {
        wallRunEnabled = true;
    }

    void OnEnterWallrun() {
        jumpCount = 0;
        //camManager.SetLookEnabled(false);
    }

    void ExitWallrun() {
        wallRunEnabled = false;
        camManager.SetLookEnabled(true);
        Invoke("EnableWallrun", 0.5f);
        //camManager.SetLookEnabled(true);
    }

    void StartCrouch() {
        if (crouchState == CrouchState.Queued) {
            StartSlide();
        } 
        else if (crouchState == CrouchState.None) {
            Vector3 velocityXZ = Vector3.Scale(rb.velocity, new Vector3(1, 0, 1));
            if (velocityXZ.magnitude > slideVelThresh)
                StartSlide();
            else
                crouchState = CrouchState.Sneak;
        }
    }

    void StartSlide() {
        crouchTime = 0;
        crouchState = CrouchState.Slide;
        //rb.velocity = Vector3.Cross(Quaternion.Euler(0, 90, 0) * transform.forward, groundHit.normal).normalized * Mathf.Clamp(rb.velocity.magnitude * slideVelBoost, baseInitialSlideVel, maxInitialSlideVel);
        rb.velocity = Vector3.Cross(
            Vector3.Scale(
                Quaternion.Euler(0, 90, 0) * rb.velocity, 
                new Vector3(1, 0, 1)
            ), groundHit.normal
        ).normalized * Mathf.Clamp(
            Vector3.Scale(rb.velocity, new Vector3(1, 0, 1)).magnitude * slideVelBoost, 
            baseInitialSlideVel, 
            maxInitialSlideVel
        );
    }

    /*void OnGUI()
    {
        if (Application.isEditor)
        {
            GUI.Label(new Rect(10, 40, 100, 20), transform.position.ToString());
            GUI.Label(new Rect(10, 60, 100, 20), rb.velocity.ToString());
        }
    }*/

}

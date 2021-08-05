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

    private Rigidbody rb;
    
    [SerializeField]
    public MovementState movementState = MovementState.Idle;

    private Vector2 targetMoveInputVector;
    private Vector3 moveInputVector;
    public void UpdateMoveInput(InputAction.CallbackContext context)
    {
        targetMoveInputVector = context.ReadValue<Vector2>();
    }

    private bool inputJump;
    public void UpdateJumpInput(InputAction.CallbackContext context)
    {
        var newValue = context.ReadValue<float>() == 1;

        if (newValue != inputJump) {
            if (newValue == true && jumpCount < maxJumpCount - 1) {

                if (Vector3.Distance(walkVector, Vector3.zero) < 0.05f) {
                    Debug.Log("Jump With Default rb momentum");
                    rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
                } else {
                    Debug.Log("Jump With input momentum");
                    rb.velocity = new Vector3(immediateWalkVector.x, jumpVelocity, immediateWalkVector.z);
                }
                jumpCount++;
            }
        }
        inputJump = newValue;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        moveInputVector = Vector2.Lerp(moveInputVector, targetMoveInputVector, Time.fixedDeltaTime * 1/acceleration);
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out groundHit, .53f);

        Vector3 targetVelocity = rb.velocity;

        walkVector = Vector3.Cross(transform.TransformDirection(new Vector3(moveInputVector.y, 0, -moveInputVector.x)), isGrounded ? groundHit.normal : Vector3.up) * speed;
        immediateWalkVector = Vector3.Cross(transform.TransformDirection(new Vector3(targetMoveInputVector.y, 0, -targetMoveInputVector.x)), isGrounded ? groundHit.normal : Vector3.up) * speed;
        
        if (isGrounded) {
            jumpCount = 0;
            targetVelocity = walkVector;

        } else {
            targetVelocity = (rb.velocity + (immediateWalkVector.normalized * airstrafeMultiplier)).normalized * rb.velocity.magnitude;
            //targetVelocity = walkVector;
        }
        targetVelocity.y = rb.velocity.y;
        rb.velocity = targetVelocity;
    }
}

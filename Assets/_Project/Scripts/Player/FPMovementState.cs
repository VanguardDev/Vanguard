using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FPMovementState
{
    protected FPMovementContext context;
    public FPMovementContext Context { 
        get { return context; } 
        set {
            context = value;
        }
    }

    public virtual void Enter() {}
    public virtual void Update() {}
    public virtual void PhysicsUpdate() {}
    public virtual void Exit() {}
    public virtual void Jump() {
        context.Rigidbody.velocity = new Vector3(
            context.Rigidbody.velocity.x, 
            context.jumpVelocity, 
            context.Rigidbody.velocity.z
        );
    }

    public virtual void Crouch() {}

    public virtual void StateChangeCheck() {
        if (context.State != this)
            Exit();
    }

    public class FPIdleState : FPMovementState {
        bool jumping = false;
        public FPIdleState(FPMovementState prevState = null) {
            if (prevState != null)
                context = prevState.Context;
            Enter();
                
        }
        
        public override void Enter() {
            if (context != null)
                context.jumpCount = 0;
        }

        public override void PhysicsUpdate() {
            if (!jumping)
                context.Rigidbody.velocity = Vector3.up * Mathf.Min(context.Rigidbody.velocity.y, 0);
        }

        public override void Jump() {
            jumping = true;
            context.Rigidbody.velocity = new Vector3(
                context.Rigidbody.velocity.x, 
                context.jumpVelocity, 
                context.Rigidbody.velocity.z
            );
        }

        public override void StateChangeCheck() {
            if (context.GroundCheck()) {
                if (context.Inputs.WalkVector.magnitude > 0)
                    context.State = new FPWalkState(this);
            }
            else {
                context.State = new FPFallState(this);
            }
            base.StateChangeCheck();
        }
    }

    public class FPFallState : FPMovementState {
        Vector3 initialLateralVelocity;
        Vector3 prevInputDirection;
        public FPFallState(FPMovementState prevState = null) {
            if (prevState != null) {
                context = prevState.Context;
                initialLateralVelocity = Vector3.Scale(context.Rigidbody.velocity, new Vector3(1, 0, 1));
            }
        }

        public override void Jump() {
            Vector3 lateralVelocity = Vector3.Scale(context.Rigidbody.velocity, new Vector3(1, 0, 1));
            Vector3 inputDirection = (context.transform.TransformDirection(
                new Vector3(context.Inputs.WalkVector.x, 0, context.Inputs.WalkVector.y).normalized
            ) + (lateralVelocity.magnitude > 0.05f ? lateralVelocity : Vector3.zero).normalized*0.3f).normalized * lateralVelocity.magnitude;
            context.Rigidbody.velocity = new Vector3(
                inputDirection.x, 
                context.Rigidbody.velocity.y, 
                inputDirection.z
            );
            base.Jump();
        }

        public override void PhysicsUpdate() {
            Vector3 inputDirection = prevInputDirection;
            if (new Vector3(context.Inputs.WalkVector.x, 0, context.Inputs.WalkVector.y).magnitude > 0.1f)
                inputDirection = new Vector3(context.Inputs.WalkVector.x, 0, context.Inputs.WalkVector.y);

            // Vector3 fallVelocity = (context.transform.TransformDirection(inputDirection) * 5).normalized * initialLateralVelocity.magnitude;
            // Debug.Log(fallVelocity);
            // fallVelocity.y = context.Rigidbody.velocity.y;
            // context.Rigidbody.velocity = fallVelocity;

            prevInputDirection = inputDirection;
        }

        public override void StateChangeCheck() {
            if (context.GroundCheck()) {
                if (context.IsCrouching)
                    context.State = new FPSlideState(this);
                else {
                    if (context.Inputs.WalkVector.magnitude < 0.02f)
                        context.State = new FPIdleState(this);
                    else
                        context.State = new FPWalkState(this);
                }
            } else {
                if (context.WallCheck()) {
                    context.State = new FPWallrunState(this);
                }
            }
            base.StateChangeCheck();
        }
    }
    
    public class FPWallrunState : FPMovementState {
        Vector3 wallDir;
        Vector3 initialVelocity;
        float initialLateralSpeed;

        public FPWallrunState(FPMovementState prevState = null) {
            if (prevState != null) {
                context = prevState.Context;
                initialVelocity = context.Rigidbody.velocity;
                initialLateralSpeed = Mathf.Max(Vector3.Scale(initialVelocity, new Vector3(1, 0, 1)).magnitude, 8.3f);

                if (initialLateralSpeed < context.maxWallrunBoostVelocity)
                    initialLateralSpeed *= context.wallrunBoost;
                context.jumpCount = 0;
                context.Rigidbody.useGravity = false;
            }
        }

        public override void PhysicsUpdate() {
            if (!context.IsJumping && !context.IsCrouching) {
                context.Rigidbody.AddForce(-Vector3.up * context.Rigidbody.mass * 4);

                Vector3 lateralVelocity = Vector3.Scale(context.Rigidbody.velocity, new Vector3(1, 0, 1));
                wallDir = Vector3.Cross(context.wallHit.normal, Vector3.up);

                Vector3 wallrunVelocity = (wallDir * initialLateralSpeed * Mathf.Sign(Vector3.Dot(wallDir, context.transform.forward))) - context.wallHit.normal;
                wallrunVelocity.y = context.Rigidbody.velocity.y;
                context.Rigidbody.velocity = wallrunVelocity;
                Debug.Log(Vector3.Dot(wallDir, context.transform.forward));
                context.Look.targetDutch = -15 * Vector3.Dot(wallDir, context.transform.forward);
            }
        }

        public override void Jump() {
            context.Rigidbody.velocity = (context.transform.forward + context.wallHit.normal).normalized * initialLateralSpeed * (Vector3.Scale(initialVelocity, new Vector3(1, 0, 1)).magnitude > context.wallrunAwayBoostThreshold ? 1 : context.wallrunAwayBoost);// * Vector3.Scale(initialVelocity, new Vector3(1, 0, 1)).magnitude;
            base.Jump();
        }

        public override void Crouch() {
            context.Rigidbody.velocity = (context.transform.forward + context.wallHit.normal).normalized;
        }

        public override void Exit() {
            context.Rigidbody.useGravity = true;
            context.Look.targetDutch = 0;
        }

        public override void StateChangeCheck() {
            if (context.GroundCheck()) {
                if (context.Inputs.WalkVector.magnitude < 0.02f)
                    context.State = new FPIdleState(this);
                else
                    context.State = new FPWalkState(this);
            } else if (!context.WallCheck() || Mathf.Abs(Vector3.Dot(wallDir, context.transform.forward)) < 0.3f) {
                context.State = new FPFallState(this);
            }
            base.StateChangeCheck();
        }
    }

    public class FPWalkState : FPMovementState {
        Vector3 prevVelocity;
        public FPWalkState(FPMovementState prevState = null) {
            if (prevState != null) {
                context = prevState.Context;
                context.jumpCount = 0;
            }
            prevVelocity = context.Rigidbody.velocity;
        }
        
        public override void PhysicsUpdate() {
            Vector3 walkVector = context.transform.TransformDirection(
                Vector3.Cross(new Vector3(context.Inputs.WalkVector.y, 0, -context.Inputs.WalkVector.x), context.groundHit.normal)
            ) * context.walkSpeed;
            prevVelocity = walkVector;
            walkVector.y = context.Rigidbody.velocity.y;
            context.Rigidbody.velocity = Vector3.Lerp(context.Rigidbody.velocity, walkVector, Time.deltaTime * 10f);
        }

        public override void StateChangeCheck() {
            if (context.GroundCheck()) {
                if (prevVelocity.magnitude < 0.02f)
                    context.State = new FPIdleState(this);
                else if (context.IsCrouching)
                    context.State = new FPSlideState(this);
            }
            else {
                context.State = new FPFallState(this);
            }

            base.StateChangeCheck();
        }
    }

    public class FPSlideState : FPMovementState {
        public FPSlideState(FPMovementState prevState = null) {
            if (prevState != null) {
                context = prevState.Context;
                context.jumpCount = 0;
            }
            Debug.Log("Slide");
            context.Rigidbody.velocity = Vector3.Cross(new Vector3(context.Rigidbody.velocity.z, 0, -context.Rigidbody.velocity.x).normalized, context.groundHit.normal) * Vector3.Scale(context.Rigidbody.velocity, new Vector3(1, 0, 1)).magnitude;
        }
        
        public override void PhysicsUpdate() {
            // walkVector.y = context.Rigidbody.velocity.y;
            // context.Rigidbody.velocity = walkVector;
            // prevWalkInputVector = context.Inputs.WalkVector;
        }

        public override void StateChangeCheck() {
            if (context.GroundCheck()) {
                if (!context.IsCrouching)
                    context.State = new FPWalkState(this);
            }
            else {
                context.State = new FPFallState(this);
            }

            base.StateChangeCheck();
        }
    }
}

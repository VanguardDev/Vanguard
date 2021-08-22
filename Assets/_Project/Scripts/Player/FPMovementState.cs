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
        public FPFallState(FPMovementState prevState = null) {
            if (prevState != null)
                context = prevState.Context;
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

        public override void StateChangeCheck() {
            if (context.GroundCheck()) {
                if (context.Inputs.WalkVector.magnitude < 0.02f)
                    context.State = new FPIdleState(this);
                else
                    context.State = new FPWalkState(this);
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
        bool jumping;
        float initialLateralSpeed;
        public FPWallrunState(FPMovementState prevState = null) {
            if (prevState != null) {
                context = prevState.Context;
                initialVelocity = context.Rigidbody.velocity;
                initialLateralSpeed = Vector3.Scale(initialVelocity, new Vector3(1, 0, 1)).magnitude;

                if (initialLateralSpeed < context.maxWallrunBoostVelocity)
                    initialLateralSpeed *= context.wallrunBoost;
                context.jumpCount = 0;
            }
        }

        public override void PhysicsUpdate() {
            if (!jumping) {
                Vector3 lateralVelocity = Vector3.Scale(context.Rigidbody.velocity, new Vector3(1, 0, 1));
                wallDir = Vector3.Cross(context.wallHit.normal, Vector3.up);
                context.Rigidbody.velocity = (wallDir * Mathf.Max(initialLateralSpeed, context.wallrunMinVelocity) * Vector3.Dot(wallDir, context.transform.forward)) - context.wallHit.normal*2;
                Debug.Log(Vector3.Dot(wallDir, context.transform.forward));
            }
        }

        public override void Jump() {
            jumping = true;
            context.Rigidbody.velocity = (context.transform.forward + context.wallHit.normal).normalized * initialLateralSpeed;
            base.Jump();
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
        Vector2 prevWalkInputVector;
        public FPWalkState(FPMovementState prevState = null) {
            if (prevState != null) {
                context = prevState.Context;
                context.jumpCount = 0;
            }
            prevWalkInputVector = context.Inputs.WalkVector;
        }
        
        public override void PhysicsUpdate() {
            Vector2 walkInputVector = Vector2.Lerp(prevWalkInputVector, context.Inputs.WalkVector, Time.deltaTime * 10f);
            Vector3 walkVector = context.transform.TransformDirection(
                Vector3.Cross(new Vector3(walkInputVector.y, 0, -walkInputVector.x), context.groundHit.normal)
            ) * 4;
            walkVector.y = context.Rigidbody.velocity.y;
            context.Rigidbody.velocity = walkVector;
            prevWalkInputVector = context.Inputs.WalkVector;
        }

        public override void StateChangeCheck() {
            if (context.GroundCheck()) {
                if (context.Inputs.WalkVector.magnitude < 0.02f)
                    context.State = new FPIdleState(this);
            }
            else {
                context.State = new FPFallState(this);
            }

            base.StateChangeCheck();
        }
    }
}

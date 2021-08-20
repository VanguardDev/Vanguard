using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FPMovementState
{
    protected FPMovementContext context;
    public FPMovementContext Context { get { return context; } set { context = value; } }

    public virtual void Enter() {}
    public virtual void Update() {}
    public virtual void PhysicsUpdate() {}
    public virtual void Exit() {}

    public virtual void StateChangeCheck() {
        if (context.State != this)
            Exit();
    }

    public class FPIdleState : FPMovementState {
        public FPIdleState(FPMovementState prevState = null) {
            if (prevState != null)
                this.context = prevState.Context;
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
                this.context = prevState.Context;
        }

        public override void StateChangeCheck() {
            if (context.GroundCheck()) {
                if (context.Inputs.WalkVector.magnitude < 0.02f)
                    context.State = new FPIdleState(this);
                else
                    context.State = new FPWalkState(this);
            }
            base.StateChangeCheck();
        }
    }

    public class FPWalkState : FPMovementState {
        Vector2 prevWalkInputVector;
        public FPWalkState(FPMovementState prevState = null) {
            if (prevState != null)
                this.context = prevState.Context;
            prevWalkInputVector = context.Inputs.WalkVector;
        }
        
        public override void PhysicsUpdate() {
            Vector2 walkInputVector = Vector2.Lerp(prevWalkInputVector, context.Inputs.WalkVector, Time.deltaTime * 10f);
            Vector3 walkVector = context.transform.TransformDirection(
                Vector3.Cross(new Vector3(context.Inputs.WalkVector.y, 0, -context.Inputs.WalkVector.x), context.groundHit.normal)
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

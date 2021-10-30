using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanguard
{
    public abstract class FPMovementState
    {
        protected FPMovementContext context;
        public FPMovementContext Context
        {
            get { return context; }
            set
            {
                context = value;
            }
        }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void PhysicsUpdate() { }
        public virtual void Exit() { }
        public virtual void Jump()
        {
            context.Rigidbody.velocity = new Vector3(
                context.Rigidbody.velocity.x,
                context.jumpVelocity,
                context.Rigidbody.velocity.z
            );
        }

        public virtual void Crouch() { }

        public virtual void StateChangeCheck()
        {
            if (context.State != this)
                Exit();
        }

        /// <summary>
        /// Freezes all but downwards vertical velocity. Transitions: WALK, FALL
        /// </summary>
        public class FPIdleState : FPMovementState
        {
            bool jumping = false;
            public FPIdleState(FPMovementState prevState = null)
            {
                if (prevState != null)
                    context = prevState.Context;
                Enter();
            }

            public override void Enter()
            {
                if (context != null)
                {
                    context.jumpCount = 0;
                    context.animationManager.ChangeState("idle");
                }
                
            }

            public override void PhysicsUpdate()
            {
                if (!jumping)
                    context.Rigidbody.velocity = Vector3.up * Mathf.Min(context.Rigidbody.velocity.y, 0);
            }

            public override void Jump()
            {
                jumping = true;
                base.Jump();
            }

            public override void StateChangeCheck()
            {
                if (context.GroundCheck())
                {
                    if (InputManager.WalkVector.magnitude > 0)
                        context.State = new FPWalkState(this);
                }
                else
                {
                    context.State = new FPFallState(this);
                }
                base.StateChangeCheck();
            }
        }

        /// <summary>
        /// Airstrafing and managing jump direction when in air. Transitions: SLIDE, IDLE, WALK, WALLRUN
        /// </summary>
        public class FPFallState : FPMovementState
        {
            Vector3 initialLateralVelocity;
            Vector3 prevInputDirection;
            public FPFallState(FPMovementState prevState = null)
            {
                if (prevState != null)
                {
                    context = prevState.Context;
                    initialLateralVelocity = Vector3.Scale(context.Rigidbody.velocity, new Vector3(1, 0, 1));
                    context.animationManager.ChangeState("falling");
                }
            }

            public override void Jump()
            {
                Vector3 lateralVelocity = Vector3.Scale(context.Rigidbody.velocity, new Vector3(1, 0, 1));
                Vector3 inputDirection = (context.transform.TransformDirection(
                    new Vector3(InputManager.WalkVector.x, 0, InputManager.WalkVector.y).normalized
                ) + (lateralVelocity.magnitude > 0.05f ? lateralVelocity : Vector3.zero).normalized * 0.3f).normalized * lateralVelocity.magnitude;
                context.Rigidbody.velocity = new Vector3(
                    inputDirection.x,
                    context.Rigidbody.velocity.y,
                    inputDirection.z
                );
                base.Jump();
            }

            public override void PhysicsUpdate()
            {
                context.Rigidbody.AddForce(-Vector3.up * context.Rigidbody.mass * 20);
                Vector3 inputVector = context.transform.TransformDirection(new Vector3(InputManager.WalkVector.x, 0, InputManager.WalkVector.y)).normalized;
                Vector3 vc = inputVector * 0.6f;

                Vector3 newVel = Vector3.ClampMagnitude(Vector3.Scale(context.Rigidbody.velocity, new Vector3(1, 0, 1)) + vc, Mathf.Max(initialLateralVelocity.magnitude, 1.4f));
                newVel.y = context.Rigidbody.velocity.y;
                context.Rigidbody.velocity = newVel;
            }

            public override void StateChangeCheck()
            {
                if (context.GroundCheck())
                {
                    if (context.IsCrouching)
                        context.State = new FPSlideState(this);
                    else
                    {
                        if (InputManager.WalkVector.magnitude < 0.02f)
                            context.State = new FPIdleState(this);
                        else
                            context.State = new FPWalkState(this);
                    }
                }
                else
                {
                    if (context.WallCheck())
                    {
                        context.State = new FPWallrunState(this);
                    }
                }
                base.StateChangeCheck();
            }
        }

        /// <summary>
        /// Sticks player to wall and applies velocity along wall. Also reduces gravity and tilts cam. Transitions: IDLE, WALK, FALL
        /// </summary>
        public class FPWallrunState : FPMovementState
        {
            Vector3 initialVelocity;
            float initialLateralSpeed;
            bool jumping = false;
            public FPWallrunState(FPMovementState prevState = null)
            {
                if (prevState != null)
                {
                    context = prevState.Context;
                    initialVelocity = context.Rigidbody.velocity;
                    initialLateralSpeed = Mathf.Max(Vector3.Scale(initialVelocity, new Vector3(1, 0, 1)).magnitude, 8.3f);

                    if (initialLateralSpeed < context.wallrunBoostThreshold)
                        initialLateralSpeed *= context.wallrunBoost;
                    context.jumpCount = 0;
                    context.animationManager.ChangeState("wallrunning");
                }
            }

            public override void PhysicsUpdate()
            {
                if (!jumping)
                {
                    // context.Rigidbody.useGravity = false;
                    context.Rigidbody.AddForce(-Vector3.up * context.Rigidbody.mass * 4);

                    Vector3 lateralVelocity = Vector3.Scale(context.Rigidbody.velocity, new Vector3(1, 0, 1));
                    Vector3 wallDir = Vector3.Cross(context.wallHit.normal, Vector3.up);

                    Vector3 wallrunVelocity = (wallDir * initialLateralSpeed * Mathf.Sign(Vector3.Dot(wallDir, context.transform.forward))) - (context.wallHit.normal * 3 * Vector3.Distance(context.transform.position, context.wallHit.point));
                    wallrunVelocity.y = context.Rigidbody.velocity.y;
                    context.Rigidbody.velocity = wallrunVelocity;
                    //Debug.Log(Vector3.Dot(wallDir, context.transform.forward));
                    context.Look.targetDutch = -15 * Vector3.Dot(wallDir, context.transform.forward);
                }
            }

            public override void Jump()
            {
                context.Rigidbody.velocity = (context.transform.forward + context.wallHit.normal).normalized * initialLateralSpeed * (Vector3.Scale(initialVelocity, new Vector3(1, 0, 1)).magnitude > context.wallrunAwayBoostThreshold ? 1 : context.wallrunAwayBoost);// * Vector3.Scale(initialVelocity, new Vector3(1, 0, 1)).magnitude;
                jumping = true;
                base.Jump();
                context.BreakFromWall(0.2f);
            }

            public override void Crouch()
            {
                context.Rigidbody.velocity += context.wallHit.normal * 2;// * Vector3.Scale(initialVelocity, new Vector3(1, 0, 1)).magnitude;
                jumping = true;
                context.BreakFromWall(1f);
            }

            public override void Exit()
            {
                // context.Rigidbody.useGravity = true;
                context.Look.targetDutch = 0;
            }

            public override void StateChangeCheck()
            {
                if (context.GroundCheck())
                {
                    if (InputManager.WalkVector.magnitude < 0.02f)
                        context.State = new FPIdleState(this);
                    else
                        context.State = new FPWalkState(this);
                }
                else if (!context.WallCheck() || Mathf.Abs(Vector3.Dot(Vector3.Cross(context.wallHit.normal, Vector3.up), context.transform.forward)) < 0.3f)
                {
                    context.State = new FPFallState(this);
                }
                base.StateChangeCheck();
            }
        }

        /// <summary>
        /// Moves the player on input. Transitions: IDLE, SLIDE, FALL 
        /// </summary>
        public class FPWalkState : FPMovementState
        {
            Vector3 walkVector;
            public FPWalkState(FPMovementState prevState = null)
            {
                if (prevState != null)
                {
                    context = prevState.Context;
                    context.jumpCount = 0;
                    context.animationManager.ChangeState("walking");
                }
            }

            public override void PhysicsUpdate()
            {
                walkVector = context.transform.TransformDirection(
                    Vector3.Cross(new Vector3(InputManager.WalkVector.y, 0, -InputManager.WalkVector.x), context.groundHit.normal)
                ) * context.walkSpeed;
                walkVector.y = context.Rigidbody.velocity.y;
                context.Rigidbody.velocity = Vector3.Lerp(context.Rigidbody.velocity, walkVector, Time.deltaTime * 10f);
            }

            public override void StateChangeCheck()
            {
                if (context.GroundCheck())
                {
                    if (walkVector.magnitude < 0.02f)
                        context.State = new FPIdleState(this);
                    else if (context.IsCrouching)
                        context.State = new FPSlideState(this);
                }
                else
                {
                    context.State = new FPFallState(this);
                }

                base.StateChangeCheck();
            }
        }

        /// <summary>
        /// Adds force if under threshold, otherwise do nothing. Transitions: WALK, FALL
        /// </summary>
        public class FPSlideState : FPMovementState
        {
            float slideTime = 0;
            public FPSlideState(FPMovementState prevState = null)
            {
                if (prevState != null)
                {
                    context = prevState.Context;
                    context.animationManager.ChangeState("sliding");
                    context.jumpCount = 0;
                }
                Debug.Log("Slide");

                context.Rigidbody.velocity = Vector3.Cross(new Vector3(context.Rigidbody.velocity.z, 0, -context.Rigidbody.velocity.x).normalized, context.groundHit.normal) *
                    Vector3.Scale(context.Rigidbody.velocity, new Vector3(1, 0, 1)).magnitude;
                if (context.Rigidbody.velocity.magnitude < context.slideBoostThreshold)
                    context.Rigidbody.velocity *= context.slideBoost;
            }

            public override void PhysicsUpdate()
            {
                context.Look.targetHeight = 0.4f;
                slideTime += Time.fixedDeltaTime;
            }

            public override void Exit()
            {
                context.Look.targetHeight = 1f;
            }

            public override void StateChangeCheck()
            {
                if (context.GroundCheck())
                {
                    if (!context.IsCrouching)
                        context.State = new FPWalkState(this);
                }
                else
                {
                    context.State = new FPFallState(this);
                }

                base.StateChangeCheck();
            }
        }
    }
}
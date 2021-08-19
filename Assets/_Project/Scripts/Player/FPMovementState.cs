using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FPMovementState
{
    protected FPMovementContext context;
    public FPMovementContext Context { get { return context; } set { context = value; } }

    public abstract void Enter();
    public abstract void Update();
    public abstract void PhysicsUpdate();
    public abstract void Exit();

    public virtual void StateChangeCheck() {
        if (context.State != this)
            Exit();
    }
}

public class FPIdleState : FPMovementState {

    public FPIdleState(FPMovementState prevState = null) {
        if (prevState != null)
            this.context = prevState.Context;
        
    }
    
    public override void Enter() {}
    public override void Update() {}
    public override void PhysicsUpdate() {}
    public override void Exit() {}
}
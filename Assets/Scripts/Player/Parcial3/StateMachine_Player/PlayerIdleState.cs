using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {}
    public override void EnterState(){
        Debug.Log("Entering Idle State");
        Ctx.speed = 0.0f;
        Ctx.animator.SetTrigger("move");
        Ctx.animator.SetFloat("speed", Ctx.speed);
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;

        Ctx.currentDodgeMovementX = 0;
        Ctx.currentDodgeMovementZ = 0;
        
    }

    public override void UpdateState(){
        CheckSwitchStates();
    }

    public override void ExitState(){}

    public override void CheckSwitchStates(){
        if(Ctx.isMovementPressed) {
            SwitchStates(Factory.Walk());
        }
        else if(!Ctx.isMovementPressed)
        {
            SwitchStates(Factory.Idle());
        }
    }

    public override void InitializeSubState(){
        
    }
}

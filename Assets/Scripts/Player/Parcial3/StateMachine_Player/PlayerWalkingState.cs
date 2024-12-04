using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public PlayerWalkingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {}
    public override void EnterState(){
        Ctx.speed = 0.5f;
        Ctx.animator.SetTrigger("move");
        Ctx.animator.SetFloat("speed", Ctx.speed);
        
    }

    public override void UpdateState(){
        CheckSwitchStates();
        Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x *  Ctx.walkingMultiplier;
        Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * Ctx.walkingMultiplier;
        Ctx.speed = 0.5f;
        Ctx.animator.SetTrigger("move");
        Ctx.animator.SetFloat("speed", Ctx.speed);
    }

    public override void ExitState(){}

    public override void CheckSwitchStates(){
        if(!Ctx.isMovementPressed) {
            SwitchStates(Factory.Idle());
        }
        else if(Ctx.isDodgeTap)
        {
            SwitchStates(Factory.Dodge());
        }
    }

    public override void InitializeSubState(){}
}

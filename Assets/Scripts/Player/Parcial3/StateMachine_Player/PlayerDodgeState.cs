using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    public PlayerDodgeState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {}
    public override void EnterState(){
        Ctx.speed = 1.0f;
        Ctx.animator.SetTrigger("Dodge");
        Ctx.animator.SetFloat("speed", Ctx.speed);
        
    }

    public override void UpdateState(){
        CheckSwitchStates();

        Ctx.currentDodgeMovementX = Ctx.CurrentMovementInput.x;
        Ctx.currentDodgeMovementZ = Ctx.CurrentMovementInput.y;
        Ctx.speed = 1.0f;
        if(!Ctx._DodgeBlock)
        {
            Ctx.animator.SetTrigger("Dodge");
            Ctx.animator.SetFloat("speed", Ctx.speed);
        }
    }

    public override void ExitState(){}

    public override void CheckSwitchStates(){
        if (!Ctx.isDodgeTap)
        {   
            SetSubStates(Factory.Walk());
        }
    }

    public override void InitializeSubState(){
        
    }

}

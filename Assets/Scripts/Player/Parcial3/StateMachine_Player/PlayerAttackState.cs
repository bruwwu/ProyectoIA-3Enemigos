using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {}
    public override void EnterState(){
        Debug.Log("Entering Attack State");
        Ctx.animator.applyRootMotion = true;
        Ctx._Attack = false;
        Ctx._TimePassed = 0.0f;
        Ctx.animator.SetTrigger("Attack");
        Ctx.animator.SetFloat("speed", 0.0f);

    }

    public override void UpdateState(){
        CheckSwitchStates();

        Ctx._TimePassed += Time.deltaTime;

        // Verificar si hay al menos un clip en la capa de animación especificada
        if (Ctx.animator.GetCurrentAnimatorClipInfo(0).Length > 0)
        {
            Ctx._ClipLength = Ctx.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            Ctx._ClipSpeed = Ctx.animator.GetCurrentAnimatorStateInfo(0).speed;
            Debug.Log("Ewe melee combat");
        }
        else
        {
            Debug.LogWarning("No animation clips found in the specified layer.");
        }

        if(Ctx._AttackAction)
        {
            Ctx._Attack = true;
        }
        else
        {
            Ctx.animator.SetTrigger("move");
            Ctx._Attack = false;
        }

    }

    public override void ExitState(){
        Ctx.animator.applyRootMotion = false;
    }

    public override void CheckSwitchStates(){

        if(Ctx._TimePassed >= Ctx._ClipLength / Ctx._ClipSpeed && Ctx._Attack)
        {
            SwitchStates(Factory.Attack());
        }
        if(Ctx._TimePassed >= Ctx._ClipLength / Ctx._ClipSpeed)
        {
            Ctx.animator.SetTrigger("move");
            Ctx._AttackAction = false;
            SwitchStates(Factory.Idle());
        }

        if(!Ctx._Attack)
        {
            Ctx._AttackAction = false;
            SwitchStates(Factory.Idle());
        }
        else if(!Ctx._Attack && Ctx.isMovementPressed)
        {
            Ctx.animator.SetTrigger("move");
            Ctx._AttackAction = false;
            SwitchStates(Factory.Walk());
            
        }
        else if (Ctx.isMovementPressed)
        {
            Ctx.animator.SetTrigger("move");
            Ctx._AttackAction = false;
            SwitchStates(Factory.Walk());
        }
        else
        {
            SwitchStates(Factory.Attack());
        }
    }

    public override void InitializeSubState(){
        
    }
}

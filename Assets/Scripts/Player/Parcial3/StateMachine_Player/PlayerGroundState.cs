using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {
        isRootState = true;
        InitializeSubState();
    }
    public override void EnterState(){
        ApplyGravity();
    }

    public override void UpdateState(){
        ApplyGravity();
        CheckSwitchStates();
    }

    public override void ExitState(){}

    public override void CheckSwitchStates(){
        // Verifica si hay movimiento
        if (Ctx.isMovementPressed)
        {
            SetSubStates(Factory.Walk()); // 1 para correr, 0.5 para caminar
        }
        else
        {
            SetSubStates(Factory.Idle());
        }

        // Establece el valor de speed en el animator
        
        // Maneja el dodge
        if (Ctx.isDodgeTap && Ctx.isMovementPressed)
        {   
            SetSubStates(Factory.Dodge());
        }

        if(Ctx._Attack && !Ctx.isMovementPressed)
        {
            SetSubStates(Factory.Attack());
        }
    }

    public override void InitializeSubState(){

    }

    private void ApplyGravity() { 
    if (Ctx.characterController.isGrounded) 
    { 
        Ctx.CurrentMovementY = Ctx.GroundedGravity; Ctx.CurrentDodgeMovementY = Ctx.GroundedGravity; 
    } 
    else { 
        Ctx.CurrentMovementY += Ctx.Gravity * Time.deltaTime; Ctx.CurrentDodgeMovementY += Ctx.Gravity * Time.deltaTime; 
    } // Actualiza el movimiento aplicado en el eje Y Ctx.AppliedMovementY = Ctx.CurrentMovementY; 
    }
}



using UnityEngine;

public class BossChaseState : BossBaseState
{
    public BossChaseState(BossFSM boss, BossStateFactory stateFactory) : base(boss, stateFactory) { }

    public override void EnterState()
    {
        boss.rb.velocity = Vector3.zero;
        

        if (boss.naomiAni != null)
        {
            boss.naomiAni.SetTrigger("Walking");
            boss.speed = 1;
            boss.naomiAni.SetFloat("Speed", boss.speed);
        }
    }

  public override void UpdateState()
{
    CheckSwitchStates();
    // Establecer el destino al jugador
    boss.navMeshAgent.SetDestination(boss.player.position);
    
}


    public override void ExitState(){}

   public override void CheckSwitchStates()
    {
        // Priorizar volver a Idle si está fuera de ambos rangos
        if (!boss.IsPlayerInRange(boss.jumpSphereRadious))
        {
            boss.SwitchState(Factory.Idle());
        }
        else if (boss.IsPlayerInRange(boss.jumpSphereRadious) && boss.isJumping)
        {
            boss.SwitchState(Factory.Jump());
        }
        else if (boss.IsPlayerInRange(boss.detectedSphereRadious))
        {
            boss.SwitchState(Factory.Chase());
        }
    }



    public override void InitializeSubState()
    {
        // No hay subestados para Chase en este caso
    }
}

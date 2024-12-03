public class BossChaseState : BossBaseState
{
    public BossChaseState(BossFSM boss, BossStateFactory stateFactory) : base(boss, stateFactory) { }

    public override void EnterState()
    {
        // Inicia el movimiento hacia el jugador
        boss.navMeshAgent.isStopped = false;
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

    // Cambiar al estado Jump si el jugador está en rango de salto
    if (!boss.IsPlayerInRange(boss.detectedSphereRadious) && boss.IsPlayerInRange(boss.jumpSphereRadious) && boss.isJumping)
    {
        boss.SwitchState(Factory.Jump());
    }
}


    public override void ExitState(){}

    public override void CheckSwitchStates()
    {
        // Validar las transiciones según condiciones adicionales si las hay
        if (!boss.IsPlayerInRange(boss.detectedSphereRadious))
        {
           // boss.SwitchState(Factory.MeteorCast());
        }
        else if (!boss.IsPlayerInRange(boss.jumpSphereRadious))
        {
            boss.SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubState()
    {
        // No hay subestados para Chase en este caso
    }
}

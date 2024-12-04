public class BossIdleState : BossBaseState
{
    public BossIdleState(BossFSM boss, BossStateFactory stateFactory) : base(boss, stateFactory) { }

    public override void EnterState()
    {
        

        if (boss.naomiAni != null)
        {
            boss.naomiAni.SetTrigger("Walking"); // Cambiado a una animación de reposo
            boss.speed = 0; // Velocidad cero en estado Idle
            boss.naomiAni.SetFloat("Speed", boss.speed);
        }
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        // Cambiar al estado de Chase si el jugador está en rango de detección
        
    }

    public override void ExitState(){}

    public override void CheckSwitchStates()
    {
        if (boss.IsPlayerInRange(boss.detectedSphereRadious))
        {
            boss.SwitchState(Factory.Chase());
        }
        // Cambiar al estado de Jump si el jugador está en rango de salto
        else if (boss.IsPlayerInRange(boss.jumpSphereRadious) && !boss.isJumping)
        {
            boss.isJumping = true;
            boss.SwitchState(Factory.Jump());
        }
    }

    public override void InitializeSubState()
    {
        // No hay subestados para Idle
    }
}

public class BossJumpState : BossBaseState
{
    public BossJumpState(BossFSM boss, BossStateFactory stateFactory) : base(boss, stateFactory) { }

    public override void EnterState()
    {

        // Inicia el salto
        boss.DoJump();

        // Desactiva temporalmente la capacidad de saltar
        boss.isJumping = true;

        // Activar animación de salto
        if (boss.naomiAni != null)
        {
            boss.naomiAni.SetTrigger("Jumping");
            boss.speed = 2;
            boss.naomiAni.SetFloat("Speed", boss.speed);
        }
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        // Verificar si el salto ha terminado
        if (!boss.isJumping)
        {
            CheckSwitchStates();
        }
    }

    public override void ExitState(){}

    public override void CheckSwitchStates()
    {
        // Cambiar al estado Chase si el jugador está en rango de detección
        if (boss.IsPlayerInRange(boss.detectedSphereRadious))
        {
            boss.SwitchState(Factory.Chase());
        }
    }

    public override void InitializeSubState()
    {
        // No hay subestados en Jump
    }
}

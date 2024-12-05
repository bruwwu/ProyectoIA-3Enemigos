public class BossShootState : BossBaseState
{
    public BossShootState(BossFSM boss, BossStateFactory stateFactory) : base(boss, stateFactory) { }

    public override void EnterState()
    {
        boss.isLockingIn = true; 
        boss.StartCoroutine(boss.lockIn());
    }

    public override void UpdateState()
    {
        CheckSwitchStates(); // Validar transición a otros estados si aplica
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        // Priorizar estados en base a las condiciones
        if (!boss.IsPlayerInRange(boss.jumpSphereRadious))
        {
            boss.isLockingIn = false;
            boss.SwitchState(Factory.Idle());
        }
        else if (boss.IsPlayerInRange(boss.jumpSphereRadious) && boss.isJumping)
        {
            boss.isLockingIn = false;
            boss.SwitchState(Factory.Jump());
        }
        else if (boss.IsPlayerInRange(boss.detectedSphereRadious))
        {
            boss.isLockingIn = false;
            boss.SwitchState(Factory.Chase());
        }
    }

    public override void InitializeSubState() { }


}

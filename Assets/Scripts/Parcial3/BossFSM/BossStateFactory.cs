public class BossStateFactory
{
    private BossFSM boss; // Referencia al FSM del jefe

    public BossStateFactory(BossFSM bossFSM)
    {
        boss = bossFSM; // Asignar la referencia del jefe al campo privado
    }

    // Métodos para crear instancias de los estados
    public BossBaseState Idle()
    {
        return new BossIdleState(boss, this);
    }

    public BossBaseState Chase()
    {
        return new BossChaseState(boss, this);
    }

    public BossBaseState Jump()
    {
        return new BossJumpState(boss, this);
    }

     public BossBaseState Wipe()
    {
        return new BossWipeState(boss, this);
    }

/*    public BossBaseState MeteorCast()
    {
        return new BossMeteorCastState(boss, this);
    }

    public BossBaseState LockIn()
    {
        return new BossLockInState(boss, this);
    }*/
}

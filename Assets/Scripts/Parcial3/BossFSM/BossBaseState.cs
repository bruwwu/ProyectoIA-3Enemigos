public abstract class BossBaseState
{
    public bool _isRootState = false;
    public BossStateFactory _factory;
    public BossBaseState _currentSubState;
    public BossBaseState _currentSuperState;
    public BossFSM boss;

    protected bool IsRootState { set { _isRootState = value; } }
    protected BossFSM Ctx { get { return boss; } }
    protected BossStateFactory Factory { get { return _factory; } }

    public BossBaseState(BossFSM currentContext, BossStateFactory bossStateFactory)
    {
        boss = currentContext;
        _factory = bossStateFactory;
    }

    // Métodos abstractos que cada estado implementará
    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitializeSubState();

    // Actualización de estados y subestados
    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    // Cambiar entre estados
    protected void SwitchStates(BossBaseState newState)
    {
        ExitState(); // Salir del estado actual
        newState.EnterState(); // Entrar al nuevo estado

        if (_isRootState)
        {
            boss.SwitchState(newState); // Actualizar el estado raíz en el contexto del jefe
        }
    }

    // Asignar un estado superior
    protected void SetSuperState(BossBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    // Asignar un estado subordinado
    protected void SetSubState(BossBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}

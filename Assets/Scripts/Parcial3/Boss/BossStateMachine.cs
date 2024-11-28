using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using DebugManager;
using Utilities;
using Unity.AI.Navigation;

public abstract class BossStateMachine
{
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubstate();

    void UpdateStates(){}
    void SwitchState(){}
    void SetSuperState(){}
    void SetSubState(){}
}

using System.Collections;
using UnityEngine;

public class BossWipeState : BossBaseState
{
    private float wipeDuration = 1.2f;

    public BossWipeState(BossFSM boss, BossStateFactory stateFactory) : base(boss, stateFactory) { }

    public override void EnterState()
    {
        boss.navMeshAgent.isStopped = true;

        if (boss.naomiAni != null)
        {
            boss.naomiAni.SetTrigger("FastCast");
        }

        boss.StartCoroutine(PerformWipe());
    }

    public override void UpdateState()
    {
    }

    public override void ExitState(){}

    public override void CheckSwitchStates()
    {
        if (boss.IsPlayerInRange(boss.detectedSphereRadious))
        {
            boss.SwitchState(Factory.Chase());
        }
        else
        {
            boss.SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubState() { }

    private IEnumerator PerformWipe()
    {
        yield return new WaitForSeconds(wipeDuration);

        Collider[] hitColliders = Physics.OverlapSphere(boss.transform.position, boss.wipeRadius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                boss.bossDmgValues.ApplyWipeDamage(boss.wipeDamage);
                Debug.Log("Wipe ejecutado. Jugador dañado.");
            }
        }
    }
}

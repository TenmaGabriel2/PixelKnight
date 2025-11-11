
using UnityEngine;

public class MonsterAttackState : MonsterState
{
    public MonsterAttackState(Monster monster, MonsterStateMachine stateMachine, string animatorParamName, E_AnimatorParamType animatorParamType) : base(monster, stateMachine, animatorParamName, animatorParamType)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (monster.isAttackOver)
        {
            stateMachine.ChangeState(monster.idleState);
        }
    }
}
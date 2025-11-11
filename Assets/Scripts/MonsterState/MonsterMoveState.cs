using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveState : MonsterState
{
    public MonsterMoveState(Monster monster, MonsterStateMachine stateMachine, string animatorParamName, E_AnimatorParamType animatorParamType) : base(monster, stateMachine, animatorParamName, animatorParamType)
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
        if (monster.targetDirection != Vector2.zero)
        {
            //如果后面加怪物数据类 就要换成数据类的变量
            monster.rb.velocity = monster.targetDirection * MonsterDataManager.Instance.monsterData.speed;
        }
        else
        {
            stateMachine.ChangeState(monster.idleState);
        }
    }
}
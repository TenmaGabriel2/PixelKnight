using UnityEngine;

public class MonsterIdleState : MonsterState
{
    public MonsterIdleState(Monster monster, MonsterStateMachine stateMachine, string animatorParamName, E_AnimatorParamType animatorParamType) : base(monster, stateMachine, animatorParamName, animatorParamType)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //防止重复启用协程
        monster.isPatroling = true;
        monster.StartCoroutine(monster.Patrol());

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
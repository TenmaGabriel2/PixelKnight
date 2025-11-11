using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : PlayerState
{
    public PlayerHurt(Player player, PlayerStateMachine stateMachine, string animatorParamName, E_AnimatorParamType animatorParamType) : base(player, stateMachine, animatorParamName, animatorParamType)
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
        if (player.isHurtOver)
            stateMachine.ChangeState(player.idleState);
    }
}

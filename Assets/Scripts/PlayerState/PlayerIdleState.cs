using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animatorParamName, E_AnimatorParamType animatorParamType) : base(player, stateMachine, animatorParamName, animatorParamType)
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
        
        if((Mathf.Abs(xInput)+Mathf.Abs(yInput)) > 0)
           stateMachine.ChangeState(player.moveState);
    }
}

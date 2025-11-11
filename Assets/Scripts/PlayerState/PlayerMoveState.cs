using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animatorParamName, E_AnimatorParamType animatorParamType) : base(player, stateMachine, animatorParamName, animatorParamType)
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
        if (player.isDead)
        {
            player.rb.velocity = Vector2.zero;
        }
        else if(SkillDataManager.Instance.skill.dashTime<SkillDataManager.Instance.skill.dashDuration)
        {
            //³å´Ì 
            player.SetVelocity(xInput * SkillDataManager.Instance.skill.dashSpeed, yInput * SkillDataManager.Instance.skill.dashSpeed);
        }
        else
        {
            player.SetVelocity(xInput * PlayerDataManager.Instance.GetCurrentPlayerData().speed, yInput * PlayerDataManager.Instance.GetCurrentPlayerData().speed);
            player.SetFilp(xInput);
        }

        if (Mathf.Abs(xInput)+Mathf.Abs(yInput) ==0)       
            stateMachine.ChangeState(player.idleState);



    }



}

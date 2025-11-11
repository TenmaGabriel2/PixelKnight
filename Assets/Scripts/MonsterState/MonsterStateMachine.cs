using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine

{
    public MonsterState currentState { get;private set; }

    public void Initialize(MonsterState startState)
    {
        currentState = startState;
        currentState.Enter();
    }
    public void ChangeState(MonsterState newState)
    {
        //·ÀÖ¹×´Ì¬ÖØ¸´ÇÐ»» 
        if (currentState == newState)
            return;

        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

}

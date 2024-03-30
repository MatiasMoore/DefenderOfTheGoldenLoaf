using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleMovingFrogAI : MovingAiPrimitive
{
    public override void DebugDrawGizmos()
    {
        
    }

    public override string GetStateName()
    {
        return "Idle";
    }

    public override void StartState()
    {

    }

    public override void Stop()
    {
        
    }

    public override void UpdateState()
    {
        //TODO: если что-то можно взять из таргета
        if (GetMovingStateManager().movingAIStateParam.target == null)
        {
            GetMovingStateManager().ChooseNewTarget();
            GetMovingStateManager().SwitchToState("MoveToTarget");
        } else
        {
            GetMovingStateManager().SwitchToState("MoveToTarget");
        }


    }
}

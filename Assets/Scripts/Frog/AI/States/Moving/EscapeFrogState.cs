using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeFrogState : MovingAiPrimitive
{
    [SerializeField]
    private float _minDistanceToTarget;

    public override void DebugDrawGizmos()
    {
        
    }

    public override string GetStateName()
    {
        return "Escape";
    }

    public override void StartState()
    {
        
    }

    public override void Stop()
    {
        GetMovingStateManager().movingAIStateParam.objectMovement.Stop();
    }

    public override void UpdateState()
    {
        GetMovingStateManager().movingAIStateParam.objectMovement.GoToPointOnNavMesh(GetMovingStateManager().movingAIStateParam.escapeTarget.position);

        if (Vector3.Distance(GetMovingStateManager().movingAIStateParam.escapeTarget.position, GetMovingStateManager().movingAIStateParam.objectMovement.transform.position) < 1f)
        {
            GetMovingStateManager().ChooseNewTarget();
            GetMovingStateManager().SwitchToState("MoveToTarget");
        }
    }
}

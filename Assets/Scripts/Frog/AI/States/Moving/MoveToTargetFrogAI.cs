using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetFrogAI : MovingAiPrimitive
{
    [SerializeField]
    private float _minDistanceToTarget;
    public override void DebugDrawGizmos()
    {
        DebugDraw.DrawSphere(transform.position, _minDistanceToTarget, Color.red);
    }

    private void OnDrawGizmosSelected()
    {
        DebugDrawGizmos();
    }

    public override string GetStateName()
    {
        return "MoveToTarget";
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
        
        if (GetMovingStateManager().movingAIStateParam.target == null)
        {
            GetMovingStateManager().SwitchToState("Idle");
            return;
        }

        if (Vector3.Distance(GetMovingStateManager().movingAIStateParam.target.position, GetMovingStateManager().movingAIStateParam.objectMovement.transform.position) < _minDistanceToTarget)
        {
            GetMovingStateManager().Pickup();
            GetMovingStateManager().SwitchToState("Escape");
            return;
        }

        GetMovingStateManager().movingAIStateParam.objectMovement.GoToPointOnNavMesh(GetMovingStateManager().movingAIStateParam.target.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeFrogState : MovingAiPrimitive
{
    [SerializeField]
    private float _minDistanceToTarget;
    [SerializeField]
    private float _jumpCooldown = 1f;
    [SerializeField]
    private float _jumpTime = 0.4f;

    private Timer _timer;
    private enum JumpState
    {
        Jumping,
        NotJumping
    }
    private JumpState _state;
    public override void DebugDrawGizmos()
    {
        
    }

    public override string GetStateName()
    {
        return "Escape";
    }

    public override void StartState()
    {
        _timer = new Timer(_jumpTime);
        _state = JumpState.Jumping;
        _timer.OnTimerDone += StopJump;
        _timer.StartTimer();
    }

    public override void Stop()
    {
        GetMovingStateManager().movingAIStateParam.objectMovement.Stop();
    }

    public override void UpdateState()
    {
        _timer.Tick();

        if (Vector3.Distance(GetMovingStateManager().movingAIStateParam.escapeTarget.position, GetMovingStateManager().movingAIStateParam.objectMovement.transform.position) < 1f)
        {
            GetMovingStateManager().ChooseNewTarget();
            GetMovingStateManager().Escaped();
            GetMovingStateManager().SwitchToState("MoveToTarget");
        }

        switch (_state)
        {
            case JumpState.Jumping:
                Jumping();
                break;
            case JumpState.NotJumping:
                NotJumping();
                break;
        }
    }

    private void StartJump()
    {
        _timer.OnTimerDone -= StartJump;
        _state = JumpState.Jumping;
        _timer = new Timer(_jumpTime);
        _timer.StartTimer();
        _timer.OnTimerDone += StopJump;

    }

    private void StopJump()
    {
        _timer.OnTimerDone -= StopJump;
        _state = JumpState.NotJumping;
        _timer = new Timer(_jumpCooldown);
        _timer.StartTimer();
        _timer.OnTimerDone += StartJump;
    }

    private void Jumping()
    {
        GetMovingStateManager().movingAIStateParam.objectMovement.GoToPointOnNavMesh(GetMovingStateManager().movingAIStateParam.escapeTarget.position);
    }

    private void NotJumping()
    {
        GetMovingStateManager().movingAIStateParam.objectMovement.Stop();
    }
}

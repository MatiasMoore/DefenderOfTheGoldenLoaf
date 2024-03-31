using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetFrogAI : MovingAiPrimitive
{
    [SerializeField]
    private float _minDistanceToTarget;
    [SerializeField]
    private float _jumpCooldown;
    [SerializeField]
    private float _jumpTime = 1f;

    private Timer _timer;
    [SerializeField]
    private Animator _animator;

    private string _isJump = "isJump";
    private string _velX = "velX";
    private string _velY = "velY";
    private string _jumptimeAnimator = "jumpTime";
    private enum JumpState
    {
        Jumping,
        NotJumping
    }

    private JumpState _state;
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
        if (_timer == null)
        {
            float randomJumpTime = Random.Range(0, _jumpTime);
            _timer = new Timer(randomJumpTime);
        }

        StartJump();
    }

    public override void Stop()
    {
        GetMovingStateManager().movingAIStateParam.objectMovement.Stop();
    }

    public override void UpdateState()
    {
        _timer.Tick();
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
        _animator.SetBool(_isJump, true);
        _animator.SetFloat(_jumptimeAnimator, _jumpTime);
    }

    private void StopJump()
    {
        _timer.OnTimerDone -= StopJump;
        _state = JumpState.NotJumping;
        _timer = new Timer(_jumpCooldown);
        _timer.StartTimer();
        _timer.OnTimerDone += StartJump;
        _animator.SetBool(_isJump, false);
    }

    private void Jumping()
    {
        GetMovingStateManager().movingAIStateParam.objectMovement.GoToPointOnNavMesh(GetMovingStateManager().movingAIStateParam.target.position);
        _animator.SetFloat(_velX, GetMovingStateManager().movingAIStateParam.objectMovement.GetVelocity().x);
        _animator.SetFloat(_velY, GetMovingStateManager().movingAIStateParam.objectMovement.GetVelocity().y);
    }

    private void NotJumping()
    {
        GetMovingStateManager().movingAIStateParam.objectMovement.Stop();
    }
}

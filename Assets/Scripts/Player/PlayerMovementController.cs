using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectMovement))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private ObjectMovement _objectMovement;

    [SerializeField]
    private Vector2 _direction;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Animator _handAnimator;

    private const string _moveSpeed = "MoveSpeed";
    private const string _velX = "velX";
    private const string _velY = "velY";
    private const string _oldDirX = "oldDirX";
    private const string _oldDirY = "oldDirY";
    
    public void Init(PlayerControls inputSystem)
    { 
        _objectMovement = GetComponent<ObjectMovement>();
        _objectMovement.Init();
    }

    private void Update()
    {
        if (PlayerControls.Instance != null)
        {   
            _animator.SetFloat(_velX, _objectMovement.GetVelocity().x);
            _animator.SetFloat(_velY, _objectMovement.GetVelocity().y);
            _handAnimator.SetFloat(_velY, _objectMovement.GetVelocity().y);
            _handAnimator.SetFloat(_velX, _objectMovement.GetVelocity().x);

            if (PlayerControls.Movement != Vector2.zero)
            {
                _animator.SetFloat(_oldDirX, PlayerControls.Movement.x);
                _animator.SetFloat(_oldDirY, PlayerControls.Movement.y);
                _handAnimator.SetFloat(_oldDirX, PlayerControls.Movement.x);
                _handAnimator.SetFloat(_oldDirY, PlayerControls.Movement.y);
            }
            _animator.SetFloat(_velY, PlayerControls.Movement.y);
            _objectMovement.SetDirection(new Vector2(PlayerControls.Movement.x, PlayerControls.Movement.y));
            UpdateDebug();
        } 
    }

    private void UpdateDebug()
    {
        _direction = PlayerControls.Movement;
    }

}

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

    private const string _moveSpeed = "MoveSpeed";
    private const string _cursorX = "CursorX";
    private const string _cursorY = "CursorY";
    
    public void Init(PlayerControls inputSystem)
    { 
        _objectMovement = GetComponent<ObjectMovement>();
        _objectMovement.Init();
    }

    private void Update()
    {
        if (PlayerControls.Instance != null)
        {         
            _objectMovement.SetDirection(new Vector2(PlayerControls.Movement.x, PlayerControls.Movement.y));
            UpdateDebug();
        } 
    }

    private void UpdateDebug()
    {
        _direction = PlayerControls.Movement;
    }

}

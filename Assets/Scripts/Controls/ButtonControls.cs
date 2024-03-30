using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class ButtonControls : MonoBehaviour
{
    [SerializeField]
    private string _pressActionName;

    [SerializeField]
    private const float timeToRegisterTouchHold = 0.2f;

    [SerializeField]
    private bool _printDebugMessages = false;    

    //Class with the control map
    private PlayerInput _playerInput;

    //Main actions
    private InputAction _pressAction;

    public event UnityAction pressDown;
    public event UnityAction pressUp;
    private Coroutine _pressHoldCoroutine;
    private float _pressHoldTime;
    public event UnityAction pressHold;

    private PlayerControls _playerControls;

    public void Init(PlayerControls controls)
    {
        _playerControls = controls;

        _playerInput = GetComponent<PlayerInput>();
        _pressAction = _playerInput.actions[_pressActionName];

        _pressAction.performed += FireTouchDownEvent;
        _pressAction.canceled += FireTouchUpEvent;
    }

    public void Disable()
    {
        _pressAction.performed -= FireTouchDownEvent;
        _pressAction.canceled -= FireTouchUpEvent;
        StopAllCoroutines();
    }

    private void FireTouchDownEvent(InputAction.CallbackContext context)
    {
        if (!_playerControls.IsCursorOverUIObject())
        {
            if (_printDebugMessages)
                Debug.Log("Touched!");
            pressDown?.Invoke();
            _pressHoldCoroutine = StartCoroutine(FireTouchHoldEvent());
        }
    }

    private void FireTouchUpEvent(InputAction.CallbackContext context)
    {
        if (_pressHoldCoroutine != null)
        {
            if (_printDebugMessages)
                Debug.Log("Released!");
            pressUp?.Invoke();
            if (_pressHoldCoroutine != null)
            {
                StopCoroutine(_pressHoldCoroutine);
                _pressHoldCoroutine = null;
            }
        }
    }

    private IEnumerator FireTouchHoldEvent()
    {
        _pressHoldTime = 0;
        while (true)
        {
            if (_pressHoldTime >= timeToRegisterTouchHold)
            {
                if (_printDebugMessages)
                    Debug.Log("Held!");
                pressHold?.Invoke();
            }

            _pressHoldTime += Time.deltaTime;
            yield return null;
        }
    }
}
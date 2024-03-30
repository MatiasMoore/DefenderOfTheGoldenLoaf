using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerControls : MonoBehaviour
{
    //Class with the control map
    private PlayerInput _playerInput;

    //Main actions
    private InputAction _pressPositionAction;

    [SerializeField]
    private string _positionActionName = "CursorPosition";

    [SerializeField]
    public ButtonControls _mainClick;
    [SerializeField]
    public ButtonControls _alternativeClick;
    [SerializeField]
    public ButtonControls _scrollUp;
    [SerializeField]
    public ButtonControls _scrollDown;

    private InputAction _moveAction;
    public static Vector2 Movement;

    private List<ButtonControls> _allControls;

    public static PlayerControls Instance { get; private set; }

    public void Init()
    {
        if (Instance != null) return;

        _playerInput = GetComponent<PlayerInput>();
        _pressPositionAction = _playerInput.actions[_positionActionName];
        _allControls = GetComponents<ButtonControls>().ToList();
        _moveAction = _playerInput.actions["Move"];
        foreach (var control in _allControls)
        {
            control.Init(this);
        }

        Instance = this;
    }

    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        foreach (var control in _allControls)
        {
            control.Disable();
        }
        StopAllCoroutines();
    }

    public bool IsCursorOverUIObject()
    {
        if (EventSystem.current == null) 
            return false;

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = getTouchScreenPosition();

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public Vector2 getTouchScreenPosition()
    {
        return _pressPositionAction.ReadValue<Vector2>();
    }

    public Vector2 getTouchWorldPosition2d()
    {
        Vector3 screenCoords = getTouchScreenPosition();
        screenCoords.z = 5;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenCoords);
        worldPos.z = 5;
        return (Vector2)worldPos;
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class AIStateManager : MonoBehaviour
{
    [SerializeField]
    private bool _reload;

    [SerializeField]
    private List<AIStatePrimitive> _statesList;

    private AIStatePrimitive _currentState;

    [SerializeField]
    protected string _currentStateString;

    [SerializeField]
    protected bool _isUpdate = true;

    private Dictionary<string, AIStatePrimitive> _states = new Dictionary<string, AIStatePrimitive>();

    public void SwitchToState(string stateString)
    {
        if (_states.TryGetValue(stateString, out AIStatePrimitive state))
        {
            if (_currentState != null)
            {
                _currentState.Stop();
            }
            _currentStateString = stateString;
            _currentState = state;
            _currentState.StartState();
        }
        else
        {
            throw new System.Exception($"No state could be found for {stateString}");
        }

    }

    protected virtual void OnEnable()
    {
        InitStates();
        SwitchToState(_currentStateString);
    }

    private void InitStates()
    {
        if (_currentState != null)
        {
            _currentState.Stop();
        }

        _states.Clear();

        foreach (var state in _statesList)
        {
            state.SetStateManager(this);
            _states.Add(state.GetStateName(), state);
        }
    }

    private void FixedUpdate()
    {
        if (!_isUpdate)
        {
            return;
        }

        _currentState.UpdateState();
    }

    private void OnValidate()
    {
        if (_reload)
        {
            _reload = false;
            InitStates();
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            if (_currentState != null)
            {
                _currentState.DebugDrawGizmos();
            }        
        }
        else
        {
            foreach (var state in _statesList)
            {
                state.DebugDrawGizmos();
            }
        }
    }

}

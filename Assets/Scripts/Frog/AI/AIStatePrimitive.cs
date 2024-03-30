using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AIStatePrimitive : MonoBehaviour
{
    protected AIStateManager _stateManager;

    public virtual void SetStateManager(AIStateManager manager)
    {
        _stateManager = manager;
    }

    abstract public void StartState();

    abstract public void Stop();

    abstract public void UpdateState();

    abstract public void DebugDrawGizmos();

    abstract public string GetStateName();

}

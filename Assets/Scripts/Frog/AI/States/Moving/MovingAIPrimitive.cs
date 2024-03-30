using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingAiPrimitive : AIStatePrimitive
{
    public MovingAIStateManager GetMovingStateManager()
    {
        return (MovingAIStateManager)_stateManager;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovingAIStateManager : AIStateManager
{
    [Serializable]
    public struct MovingAIStateParams
    {
        public List<Transform> possibleTargets;
        public Transform target;
        public ObjectMovement objectMovement;
        public Transform escapeTarget;
    }

    public MovingAIStateParams movingAIStateParam;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (movingAIStateParam.objectMovement == null)
        {
            movingAIStateParam.objectMovement = GetComponent<ObjectMovement>();
        }

        if (movingAIStateParam.objectMovement == null)
        {
            Debug.LogError($"ObjectMovement is not set for {gameObject.name}");
        }
        movingAIStateParam.objectMovement.Init();
        movingAIStateParam.objectMovement.SetWalkType(ObjectMovement.WalkType.ByPoint);
    }

    [ContextMenu("Choose new target")]
    public void ChooseNewTarget()
    {
        //choose not the same random target

        List<Transform> possibleTargets = new List<Transform>(movingAIStateParam.possibleTargets);
        possibleTargets.Remove(movingAIStateParam.target);
        movingAIStateParam.target = possibleTargets[Random.Range(0, possibleTargets.Count)];
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class MovingAIStateManager : AIStateManager
{

    [SerializeField]
    public event UnityAction OnPush;
    [SerializeField]
    public event UnityAction OnEscape;
    [SerializeField]
    public event UnityAction OnPickup;

    [Serializable]
    public struct Target
    {
        public int weight;
        public Transform target;
    }

    [Serializable]
    public struct MovingAIStateParams
    {
        public List<Target> possibleTargets;
        [HideInInspector]
        public Transform target;
        [HideInInspector]
        public ObjectMovement objectMovement;
        public Transform escapeTarget;
    }

    public MovingAIStateParams movingAIStateParam;

    public void Escaped()
    {
        OnEscape?.Invoke();
    }

    public void Pickup()
    {
        OnPickup?.Invoke();
    }

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
        int weightSum = 0;
        foreach (var target in movingAIStateParam.possibleTargets)
        {
            weightSum += target.weight;
        }

        int randomValue = Random.Range(0, weightSum);
        foreach (var target in movingAIStateParam.possibleTargets)
        {
            randomValue -= target.weight;
            if (randomValue <= 0)
            {
                movingAIStateParam.target = target.target;
                return;
            }
        }
    }

    public void StopForSeconds(float seconds)
    {
        OnPush.Invoke();
        _isUpdate = false;
        movingAIStateParam.objectMovement.SetIsUpdate(false);
        StartCoroutine(SetIsUpdateWithDelay(true, seconds));
        
    }

    private IEnumerator SetIsUpdateWithDelay(bool state, float delay)
    {
        yield return new WaitForSeconds(delay);
        _isUpdate = state;
        movingAIStateParam.objectMovement.SetIsUpdate(state);
    }

}

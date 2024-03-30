using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealManager : MonoBehaviour
{
    [SerializeField]
    private MovingAIStateManager _movingAIStateManager;

    [SerializeField]
    private float _stealRadius;

    [SerializeField]
    private Inventory _inventory;

    private void Start()
    {
        _movingAIStateManager.OnPush += Drop;
        _movingAIStateManager.OnEscape += DestroyElement;
    }

    private void Update()
    {
        if (_movingAIStateManager.movingAIStateParam.target != null)
        {
            if (Vector2.Distance(transform.position, _movingAIStateManager.movingAIStateParam.target.position) < _stealRadius)
            {
                TryToSteal();
            }
        }
    }

    private void TryToSteal()
    {
        _inventory.PickupItemAtPos(_movingAIStateManager.movingAIStateParam.target.position);
    }

    private void Drop()
    {
        _inventory.DropItem();
    }

    private void DestroyElement()
    {
        _inventory.DestroyItem();
    }

    private void OnDrawGizmosSelected()
    {
        DebugDraw.DrawSphere(transform.position, _stealRadius, Color.red);
    }
}

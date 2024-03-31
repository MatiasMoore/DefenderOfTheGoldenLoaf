using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CheckoutTable : MonoBehaviour
{
    [SerializeField]
    private Transform _attachPoint;

    public static UnityAction<CheckoutTable, Recipe> CheckedOutRecipe;

    private Plate _currentPlate;

    public bool IsFree() => _currentPlate == null;

    public void SetPlateOnTable(Plate plate)
    {
        _currentPlate = plate;
        _currentPlate.OnPickedUp += ForgetPlate;
        _currentPlate.transform.parent = _attachPoint;
        _currentPlate.transform.localPosition = plate.GetAttachOffset();
        Checkout();
    }

    public void ForgetPlate()
    {
        _currentPlate.OnPickedUp -= ForgetPlate;
        _currentPlate = null;
    }

    public void ClearAndDeletePlate()
    {
        Destroy(_currentPlate.gameObject);
        _currentPlate = null;
    }

    public void Checkout()
    {
        if (_currentPlate != null && _currentPlate.IsFinished())
            CheckedOutRecipe?.Invoke(this, _currentPlate.GetFinishedRecipe());
    }
}

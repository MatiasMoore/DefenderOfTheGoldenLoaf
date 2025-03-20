using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CheckoutTable : TablePrimitive
{
    public static UnityAction<CheckoutTable, Recipe> CheckedOutRecipe;

    public override void ItemPickedUp()
    {
    }

    public override void ItemPlaced()
    {
        Checkout();
    }

    private void Checkout()
    {
        if (_currentItem != null)
        {
            if (_currentItem is Plate)
            {
                var currentPlate = (Plate)_currentItem;
                CheckedOutRecipe?.Invoke(this, currentPlate.GetFinishedRecipe());
            }
        }

    }
}

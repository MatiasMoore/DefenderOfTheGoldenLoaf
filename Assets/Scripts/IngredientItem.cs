using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientItem : InventoryItem
{
    [SerializeField]
    private IngredientWithInstruction _ingredient;

    public override void Dropped()
    {
    }

    public override Vector3 GetAttachOffset()
    {
        return Vector3.zero;
    }

    public override void PickedUp()
    {
        OnPickedUp?.Invoke();
    }

    public IngredientWithInstruction GetIngredient() => _ingredient;

    public override bool UseAtPos(Vector2 pos)
    {
        var collider = Physics2D.OverlapPoint(pos, 1 << LayerMask.NameToLayer("Plate"));
        if (collider != null && collider.TryGetComponent<Combinator>(out Combinator table)) 
        {
            Debug.Log("Trying to add to plate!");
            if (table.TryToAddIngredient(this))
            {
                Debug.Log("Added to plate!");
                ForgetAbout?.Invoke();
                return true;
            }
        }

        return false;
    }
}

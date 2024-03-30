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
    }

    public override bool UseAtPos(Vector2 pos)
    {
        var collider = Physics2D.OverlapPoint(pos);
        if (collider != null && collider.TryGetComponent<RecipeTable>(out RecipeTable table)) 
        {
            if (table.TryToAddIngredient(_ingredient))
            {
                Destroyed?.Invoke();
                return true;
            }
        }

        return false;
    }
}

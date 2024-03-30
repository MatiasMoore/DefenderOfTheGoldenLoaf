using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecipeTable : MonoBehaviour
{
    [SerializeField]
    private Dish _currentDish;

    private UnityAction<RecipeTable, Dish> FinishedDish;

    public void SetCurrentDish(Dish newDish)
    {
        _currentDish = newDish;
    }

    public bool TryToAddInstruction(IngredientWithInstruction ingredient)
    {
        bool success = _currentDish.TryToAddIngredient(ingredient);
        if (success)
            FinishedDish?.Invoke(this, _currentDish);

        return success;
    }
}

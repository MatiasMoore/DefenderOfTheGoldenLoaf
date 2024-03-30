using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecipeTable : MonoBehaviour
{
    [SerializeField]
    private Recipe _currentRecipe;

    private UnityAction<RecipeTable, Recipe> FinishedRecipe;

    public void SetCurrentRecipe(Recipe newRecipe)
    {
        _currentRecipe = newRecipe;
    }

    public bool TryToAddInstruction(IngredientWithInstruction instruction)
    {
        bool success = _currentRecipe.AddFinishedIntruction(instruction);
        if (success)
            FinishedRecipe?.Invoke(this, _currentRecipe);

        return success;
    }
}

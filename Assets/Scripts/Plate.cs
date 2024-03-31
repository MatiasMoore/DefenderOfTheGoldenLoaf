using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Plate : InventoryItem
{
    [SerializeField]
    private List<Recipe> _recipes;

    [SerializeField]
    private Recipe _failedRecipe;

    private List<IngredientWithInstruction> _addedIngredients = new ();
    private Recipe _finishedRecipe = null;

    private UnityAction<Plate, Recipe> FinishedRecipe;

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

    public bool TryToAddIngredient(IngredientWithInstruction ingredient)
    {
        if (_finishedRecipe != null)
            return false;

        AddIngredient(ingredient);

        return true;
    }

    public override bool UseAtPos(Vector2 pos)
    {
        return false;
    }

    private void AddIngredient(IngredientWithInstruction ingredient)
    {
        _addedIngredients.Add(ingredient);
        CheckIfDishIsReady();
    }

    private void CheckIfDishIsReady()
    {
        foreach (var recipe in _recipes)
        {
            var required = recipe.GetRequiredIngredients();
            if (required.Count != _addedIngredients.Count)
                continue;

            bool allMatch = true;
            for (int i = 0; i < required.Count; i++)
            {
                var reqIngredient = required[i];
                var addedIngredient = _addedIngredients[i];
                if (!reqIngredient.IsEquivalentTo(addedIngredient))
                    allMatch = false;
            }
            if (allMatch)
            {
                FinishRecipe(recipe);
                return;
            }
        }

        if (_addedIngredients.Count >= 3)
        {
            FinishRecipe(_failedRecipe);
            return;
        }
    }

    private void FinishRecipe(Recipe recipe)
    {
        _finishedRecipe = recipe;
        FinishedRecipe?.Invoke(this, _finishedRecipe);
        var obj = Instantiate(_finishedRecipe.GetGameObj(), transform.position, Quaternion.identity, this.transform);
    }
}

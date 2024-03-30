using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dish : MonoBehaviour
{
    [SerializeField]
    private Recipe _recipe;

    public UnityAction Finished;

    private List<IngredientWithInstruction> _neededIngredients = new ();

    private bool _isFinished = false;

    private void Awake()
    {
        _neededIngredients = _recipe.GetRequiredIngredients();
    }

    public bool TryToAddIngredient(IngredientWithInstruction ingredient)
    {
        if (!IsIngredientNeeded(ingredient))
            return false;

        RemoveNeededIngredient(ingredient);

        if (_neededIngredients.Count == 0)
            FinishDish();

        return true;
    }

    private void FinishDish()
    {
        _isFinished = true;
        Finished?.Invoke();
    }

    private bool IsIngredientNeeded(IngredientWithInstruction ingredient)
    {
        foreach (var needed in _neededIngredients)
        {
            if (needed.GetIngredientType() == ingredient.GetIngredientType()
                && needed.GetIngredientInstruction() == ingredient.GetIngredientInstruction())
                return true;
        }

        return false;
    }

    private void RemoveNeededIngredient(IngredientWithInstruction ingredient)
    {
        foreach (var needed in _neededIngredients)
        {
            if (needed.GetIngredientType() == ingredient.GetIngredientType()
                && needed.GetIngredientInstruction() == ingredient.GetIngredientInstruction())
            {
                _neededIngredients.Remove(needed);
            }
        }
    }

    public bool IsFinished() => _isFinished;
}

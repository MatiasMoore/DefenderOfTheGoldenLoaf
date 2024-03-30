using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecipeTable : MonoBehaviour
{
    [SerializeField]
    private Dish _currentDish;

    private UnityAction<RecipeTable, Dish> FinishedDish;

    public bool IsFree() => _currentDish == null;

    public void SetCurrentDish(Dish newDish)
    {
        _currentDish = newDish;
        _currentDish.Finished += OnDishFinish;
    }

    public bool TryToAddIngredient(IngredientWithInstruction ingredient)
    {
        if (_currentDish == null)
            return false;

        return _currentDish.TryToAddIngredient(ingredient);
    }

    private void OnDishFinish()
    {
        FinishedDish?.Invoke(this, _currentDish);
        var obj = Instantiate(_currentDish.GetRecipe().GetGameObj(), transform.position, Quaternion.identity, this.transform);
    }
}

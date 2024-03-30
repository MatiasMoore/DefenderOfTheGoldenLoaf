using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Order
{
    public static UnityAction<Order> OrderFinished;

    private Dish _dish;

    private RecipeTable _recipeTable;

    private VisualElement _visualElem;

    public Order(Dish dish, RecipeTable recipeTable, VisualElement elem)
    {
        _dish = dish;
        _recipeTable = recipeTable;
        _visualElem = elem;
        _dish.Finished += FinishOrder;
    }

    public Dish GetDish() => _dish;

    public VisualElement GetVisualElement() => _visualElem;

    public Recipe GetRecipe() => _dish.GetRecipe();

    public RecipeTable GetRecipeTable() => _recipeTable;

    private void FinishOrder()
    {
        OrderFinished?.Invoke(this);
    }
}

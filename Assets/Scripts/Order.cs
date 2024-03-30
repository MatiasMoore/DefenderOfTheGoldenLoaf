using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Order
{
    public static UnityAction<Order> OrderFinished;

    private Dish _dish;

    private RecipeTable _recipeTable;

    public Order(Dish dish, RecipeTable recipeTable)
    {
        _dish = dish;
        _recipeTable = recipeTable;
        _dish.Finished += FinishOrder;
    }

    public Dish GetDish() => _dish;

    public Recipe GetRecipe() => _dish.GetRecipe();

    public RecipeTable GetRecipeTable() => _recipeTable;

    private void FinishOrder()
    {
        OrderFinished?.Invoke(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Order
{
    public static UnityAction<Order> OrderFinished;

    private Recipe _recipe;

    private VisualElement _visualElem;

    public Order(Recipe recipe, VisualElement elem)
    {
        _recipe = recipe;
        _visualElem = elem;
    }

    public Recipe GetRecipe() => _recipe;

    public VisualElement GetVisualElement() => _visualElem;

    public void FinishOrder()
    {
        OrderFinished?.Invoke(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Order
{
    private Recipe _recipe;

    private VisualElement _visualElem;

    private float _timeToFinish;

    public Order(Recipe recipe, VisualElement elem, float timeToFinish)
    {
        _recipe = recipe;
        _visualElem = elem;
        _timeToFinish = timeToFinish;
    }

    public float GetTimeLeftToFinish() => _timeToFinish;

    public void ReduceTimeLeftBy(float delta)
    {
        _timeToFinish = Mathf.Clamp(_timeToFinish - delta, 0, float.MaxValue);
    }

    public Recipe GetRecipe() => _recipe;

    public VisualElement GetVisualElement() => _visualElem;
}

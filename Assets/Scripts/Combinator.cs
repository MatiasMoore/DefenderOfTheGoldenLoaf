using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Combinator : InventoryItem
{
    [SerializeField]
    private Transform _attach;

    [SerializeField]
    private List<Recipe> _recipes;

    [SerializeField]
    private int _maxIngredientCount = 3;

    [SerializeField]
    private Recipe _failedRecipe;

    [SerializeField]
    private float _delay = 1;

    private InventoryItem _item;
    private Coroutine _finishing = null;
    private Recipe _finishedRecipe = null;

    private UnityAction<Combinator, Recipe> FinishedRecipe;

    public override void Dropped()
    {
    }

    public override Vector3 GetAttachOffset()
    {
        return Vector3.zero;
    }

    public override void PickedUp()
    {
        OnPickedUp?.Invoke();
    }

    public Recipe GetFinishedRecipe()
    {
        if (_attach.childCount == 1)
        {
            if (_attach.GetChild(0).TryGetComponent<IngredientItem>(out IngredientItem item))
            {
                return item.GetRecipe();
            }
        }
        return null;
    }

    public bool IsFinished() => _finishedRecipe != null;

    public bool TryToAddIngredient(IngredientItem ingredientItem)
    {
        if (_finishedRecipe != null && _finishing != null)
            return false;

        AddIngredient(ingredientItem);

        return true;
    }

    public override bool UseAtPos(Vector2 pos)
    {
        var collider = Physics2D.OverlapPoint(pos, 1 << LayerMask.NameToLayer("Checkout"));
        if (collider != null && collider.TryGetComponent<TablePrimitive>(out TablePrimitive table))
        {
            if (table.IsFree())
            {
                ForgetAbout?.Invoke();
                table.SetItemOnTable(this);
                return true;
            }
        }
        return false;
    }

    private void AddIngredient(IngredientItem ingredientItem)
    {
        ingredientItem.transform.parent = _attach;
        ingredientItem.transform.localPosition = Vector3.zero;
        CheckIfDishIsReady();
    }

    private void CheckIfDishIsReady()
    {
        var ingredientItems = _attach.GetComponentsInChildren<IngredientItem>().ToList();
        var addedIngredients = new List<IngredientWithInstruction>();
        foreach (var item in  ingredientItems)
        {
            addedIngredients.Add(item.GetIngredient());
        }

        foreach (var recipe in _recipes)
        {
            var required = recipe.GetRequiredIngredients();
            if (required.Count != addedIngredients.Count)
                continue;

            bool allMatch = true;
            for (int i = 0; i < required.Count; i++)
            {
                var reqIngredient = required[i];
                var addedIngredient = addedIngredients[i];
                if (!reqIngredient.IsEquivalentTo(addedIngredient))
                    allMatch = false;
            }
            if (allMatch)
            {
                FinishRecipe(recipe);
                return;
            }
        }

        if (addedIngredients.Count >= _maxIngredientCount)
        {
            FinishRecipe(_failedRecipe);
            return;
        }
    }

    private void FinishRecipe(Recipe recipe)
    {
        _finishing = StartCoroutine(FinishRecipeWithDelay(_delay, recipe));
    }

    private IEnumerator FinishRecipeWithDelay(float delay, Recipe recipe)
    {
        float time = 0;
        while (time < delay)
        {
            time += Time.deltaTime;
            yield return null;
        }

        _finishedRecipe = recipe;
        FinishedRecipe?.Invoke(this, _finishedRecipe);
        for (int i = 0; i < _attach.childCount; i++)
        {
            Destroy(_attach.GetChild(i).gameObject);
        }
        var obj = Instantiate(_finishedRecipe.GetGameObj(), transform.position, Quaternion.identity, _attach);
        if (obj.TryGetComponent<InventoryItem>(out InventoryItem item))
        {
            _item = item;
            _item.OnPickedUp += Clear;
        }
        _finishing = null;
    }

    private void Clear()
    {
        _finishedRecipe = null;
        _item.OnPickedUp -= Clear;
        _item = null;
    }
}

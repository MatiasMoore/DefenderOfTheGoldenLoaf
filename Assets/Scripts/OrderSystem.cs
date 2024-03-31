using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class OrderSystem : MonoBehaviour
{
    [SerializeField]
    private int _maxOrderCount = 3;

    [SerializeField]
    private List<Recipe> _recipes;

    //[SerializeField]
    //private List<Plate> _tables;

    [SerializeField]
    private HUDController _controller;

    [SerializeField]
    private GameObject _recipeTimerPrefab;

    private List<Order> _orders = new ();

    private void Awake()
    {
        Order.OrderFinished += OrderFinished;
    }

    [ContextMenu("Create Order")]
    public void CreateOrder()
    {
        if (_orders.Count < _maxOrderCount)
        {
            var recipe = _recipes[Random.Range(0, _recipes.Count)];
            var elem = _controller.AddRecipeElement(recipe);
            Instantiate(_recipeTimerPrefab).GetComponent<RecipeTimer>().SetupObject(elem, 3f); // ÇÄÅÑÜ ÄÎËÆÍÎ ÇÀÄÀÂÀÒÜÑß ÂĞÅÌß ÍÀ ÏĞÈÃÎÒÎÂËÅÍÈÅ
            var order = new Order(recipe, elem);
            AddOrder(order);
        }
    }

    private void AddOrder(Order order)
    {
        _orders.Add(order);   
    }

    private void RemoveOrder(Order order) 
    {
        _orders.Remove(order);
        Debug.Log("Removing order!");
        _controller.RemoveRecipeElement(order.GetVisualElement());
    }

    private void OrderFinished(Order order)
    {
        Debug.Log("Removing order!");
        RemoveOrder(order);
    }
}

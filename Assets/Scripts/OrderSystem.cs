using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    [SerializeField]
    private List<Recipe> _recipes;

    [SerializeField]
    private List<RecipeTable> _tables;

    [SerializeField]
    private HUDController _controller;

    private List<Order> _orders = new ();

    private void Awake()
    {
        Order.OrderFinished += OrderFinished;
    }

    [ContextMenu("Create Order")]
    public void CreateOrder()
    {
        foreach (RecipeTable table in _tables)
        {
            if (table.IsFree())
            {
                var dish = new Dish(_recipes[0]);
                var order = new Order(dish, table);
                AddOrder(order);
                table.SetCurrentDish(dish);
            }
        }
    }

    private void AddOrder(Order order)
    {
        _orders.Add(order);
        _controller.AddRecipeElement(order.GetRecipe());
    }

    private void RemoveOrder(Order order) 
    {
        _orders.Remove(order);
        Debug.Log("Removing order!");
    }

    private void OrderFinished(Order order)
    {
        Debug.Log("Removing order!");
        RemoveOrder(order);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class OrderSystem : MonoBehaviour
{
    [SerializeField]
    private int _maxOrderCount = 3;

    [SerializeField]
    private int _ordersToWin = 5;

    [SerializeField]
    private float _timePerOrder = 20f;

    [SerializeField]
    private float _timeBetweenOrders = 10f;

    [SerializeField]
    private List<Recipe> _recipes;

    [SerializeField]
    private HUDController _controller;

    [SerializeField]
    private GameObject _recipeTimerPrefab;

    private int _finishedOrderCount = 0;
    private List<Order> _orders = new ();

    private void Awake()
    {
        CheckoutTable.CheckedOutRecipe += CheckedOutRecipe;
    }

    private void Start()
    {
        CreateOrdersOneByOne(_ordersToWin, _timeBetweenOrders);
    }

    private void CreateOrdersOneByOne(int count, float delayBetween)
    {
        for (int i = 0; i < count; i++)
        {
            StartCoroutine(CreateOrderWithDelay(i*delayBetween));
        }
    }

    private IEnumerator CreateOrderWithDelay(float delay)
    {
        float time = 0;
        while (time < delay)
        {
            time += Time.deltaTime;
            yield return null;
        }

        CreateOrder();
    }

    private void Update()
    {
        foreach (var order in _orders)
        {
            order.ReduceTimeLeftBy(Time.deltaTime);
            if (order.GetTimeLeftToFinish() <= 0)
            {
                FailOrder(order);
                return;
            }
        }
    }

    private void CheckedOutRecipe(CheckoutTable table, Recipe recipe)
    {
        foreach (var order in _orders)
        {
            if (order.GetRecipe() == recipe)
            {
                table.ClearAndDeleteItem();
                FinishOrder(order);
                return;
            }
        }
    }

    [ContextMenu("Create Order")]
    public void CreateOrder()
    {
        if (_orders.Count < _maxOrderCount)
        {
            var recipe = _recipes[Random.Range(0, _recipes.Count)];
            var elem = _controller.AddRecipeElement(recipe);
            Instantiate(_recipeTimerPrefab).GetComponent<RecipeTimer>().SetupObject(elem, _timePerOrder); // «ƒ≈—‹ ƒŒÀ∆ÕŒ «¿ƒ¿¬¿“‹—ﬂ ¬–≈Ãﬂ Õ¿ œ–»√Œ“Œ¬À≈Õ»≈
            var order = new Order(recipe, elem, _timePerOrder);
            AddOrder(order);
        }
        else
        {
            LoseGame();
        }
    }

    private void AddOrder(Order order)
    {
        _orders.Add(order);
    }

    private void RemoveOrder(Order order) 
    {
        _orders.Remove(order);
        _controller.RemoveRecipeElement(order.GetVisualElement());
    }

    private void UpdateProgressBar()
    {
        float progress = (float)_finishedOrderCount / (float)_ordersToWin;
        _controller.ChangeLevelProgressBarValue(progress);
    }

    private void FinishOrder(Order order)
    {
        RemoveOrder(order);
        _finishedOrderCount++;
        UpdateProgressBar();
        if (_finishedOrderCount == _ordersToWin)
            WinGame();
    }

    private void FailOrder(Order order)
    {
        RemoveOrder(order);
        LoseGame();
    }

    private void ClearOrders()
    {
        foreach (var order in _orders)
        {
            _controller.RemoveRecipeElement(order.GetVisualElement());
        }
        _orders.Clear();
    }

    private void LoseGame()
    {
        Debug.Log("You lost!");
        StopAllCoroutines();
        ClearOrders();
        _controller.SetLosePanelActivity(true);
    }

    private void WinGame()
    {
        Debug.Log("You won!");
        StopAllCoroutines();
        ClearOrders();
        _controller.SetWinPanelActivity(true);
    }
}

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
    private float _reduceTimePerOrderOnShuffle = 5f;

    [SerializeField]
    private float _timeBetweenOrders = 10f;

    [SerializeField]
    private int _bonusSpawnChance = 50;

    [SerializeField]
    private List<GameObject> _bonusItems = new();

    [SerializeField]
    private List<Recipe> _recipes;

    [SerializeField]
    private HUDController _controller;

    [SerializeField]
    private GameObject _recipeTimerPrefab;

    private int _finishedOrderCount = 0;
    private List<Order> _orders = new ();

    private void OnEnable()
    {
        CheckoutTable.CheckedOutRecipe += CheckedOutRecipe;
        MenuItem.MenuDestroyed += ShuffleMenu;
    }

    private void OnDisable()
    {
        CheckoutTable.CheckedOutRecipe -= CheckedOutRecipe;
        MenuItem.MenuDestroyed -= ShuffleMenu;
    }

    private void Start()
    {
        CreateOrdersOneByOne(_ordersToWin, _timeBetweenOrders);
    }

    [ContextMenu("Shuffle")]
    private void ShuffleMenu()
    {
        Debug.Log("Shuffling");
        ClearOrders();
        //StopAllCoroutines();
        _timePerOrder = Mathf.Clamp(_timePerOrder - _reduceTimePerOrderOnShuffle, 0, int.MaxValue);
        CreateOrdersOneByOne(_ordersToWin - _finishedOrderCount, _timeBetweenOrders);
        AudioPlayer.Instance.PlaySFX(AudioPlayer.SFX.shuffle);
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
                if (Random.Range(1, 101) >= _bonusSpawnChance)
                    SpawnRandomBonusAtTable(table);
                return;
            }
        }
    }

    private void SpawnRandomBonusAtTable(TablePrimitive table)
    {
        if (_bonusItems.Count == 0)
            return;

        var bonus = _bonusItems[Random.Range(0, _bonusItems.Count)];
        var bonusObj = Instantiate(bonus, Vector3.zero, Quaternion.identity);
        if (!bonusObj.TryGetComponent<InventoryItem>(out InventoryItem item))
            throw new System.Exception("Bonus must be inventory item!");

        table.SetItemOnTable(item);
    }

    [ContextMenu("Create Order")]
    public void CreateOrder()
    {
        if (_orders.Count < _maxOrderCount)
        {
            var recipe = _recipes[Random.Range(0, _recipes.Count)];
            var elem = _controller.AddRecipeElement(recipe);
            Instantiate(_recipeTimerPrefab).GetComponent<RecipeTimer>().SetupObject(elem, _timePerOrder); // ����� ������ ���������� ����� �� �������������
            var order = new Order(recipe, elem, _timePerOrder);
            AddOrder(order);
            AudioPlayer.Instance.PlaySFX(AudioPlayer.SFX.orderStart);
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
        else
            AudioPlayer.Instance.PlaySFX(AudioPlayer.SFX.orderFinish);
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
        AudioPlayer.Instance.PlaySFX(AudioPlayer.SFX.lose);
    }

    private void WinGame()
    {
        Debug.Log("You won!");
        StopAllCoroutines();
        ClearOrders();
        _controller.SetWinPanelActivity(true);
        AudioPlayer.Instance.PlaySFX(AudioPlayer.SFX.win);
    }
}

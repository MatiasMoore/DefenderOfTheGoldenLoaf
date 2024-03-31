using UnityEngine.Events;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
abstract public class TablePrimitive : MonoBehaviour
{
    [SerializeField]
    protected Transform _attachPoint;

    protected InventoryItem _currentItem;

    public bool IsFree() => _currentItem == null;

    public void SetItemOnTable(InventoryItem item)
    {
        _currentItem = item;
        _currentItem.OnPickedUp += ForgetItem;
        _currentItem.transform.parent = _attachPoint;
        _currentItem.transform.localPosition = item.GetAttachOffset();
        ItemPlaced();
    }

    public void ForgetItem()
    {
        _currentItem.OnPickedUp -= ForgetItem;
        ItemPickedUp();
        _currentItem = null;
    }

    public void ClearAndDeleteItem()
    {
        Destroy(_currentItem.gameObject);
        _currentItem = null;
    }

    abstract public void ItemPlaced();

    abstract public void ItemPickedUp();
}
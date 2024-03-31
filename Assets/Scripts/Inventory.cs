using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Transform _holdAttach;

    [SerializeField]
    private float _maxInteractDistance = 4;

    private InventoryItem _currentItem;

    private PlayerControls _playerControls;

    public void Init(PlayerControls playerControls)
    {
        _playerControls = playerControls;
        _playerControls._mainClick.pressDown += MainAction;
        _playerControls._alternativeClick.pressDown += AlternativeAction;
    }

    private void MainAction()
    {
        Debug.Log("Main action!");

        if (_currentItem == null)
            return;

        UseItemAtPos(_playerControls.getTouchWorldPosition2d());
    }

    private void AlternativeAction()
    {
        Debug.Log("Alternative action!");

        if ( _currentItem == null)
        {
            PickupItemAtPos(_playerControls.getTouchWorldPosition2d());
        }
        else
        {
            DropItem();
        }
    }

    private void UseItemAtPos(Vector2 pos)
    {
        if (Vector2.Distance(pos, (Vector2)transform.position) > _maxInteractDistance)
            return;

        _currentItem.UseAtPos(pos);
    }

    public void PickupItemAtPos(Vector2 pos)
    {
        if (Vector2.Distance(pos, (Vector2)transform.position) > _maxInteractDistance)
            return;

        bool success = PickupPlate(pos);
        if (!success)
        {
            success = PickupAnyPickup(pos);
        }
        if (success)
            AudioPlayer.Instance.PlaySFX(AudioPlayer.SFX.pickup);
    }

    private bool PickupPlate(Vector2 pos)
    {
        var collider = Physics2D.OverlapPoint(pos, 1 << LayerMask.NameToLayer("Plate"));
        if (collider != null && collider.TryGetComponent<InventoryItem>(out InventoryItem newItem))
        {
            if (!(newItem is Plate))
                return false;

            _currentItem = newItem;
            _currentItem.transform.parent = _holdAttach;
            _currentItem.transform.localPosition = _currentItem.GetAttachOffset();
            _currentItem.Destroyed += DestroyItem;
            _currentItem.ForgetAbout += ForgetItem;
            _currentItem.PickedUp();
            return true;
        }

        return false;
    }

    private bool PickupAnyPickup(Vector2 pos)
    {
        var collider = Physics2D.OverlapPoint(pos, 1 << LayerMask.NameToLayer("Pickup"));
        if (collider != null && collider.TryGetComponent<InventoryItem>(out InventoryItem newItem))
        {
            _currentItem = newItem;
            _currentItem.transform.parent = _holdAttach;
            _currentItem.transform.localPosition = _currentItem.GetAttachOffset();
            _currentItem.Destroyed += DestroyItem;
            _currentItem.ForgetAbout += ForgetItem;
            _currentItem.PickedUp();
            return true;
        }

        return false;
    }

    public void DropItem()
    {
        if (_currentItem == null)
            return;
        _currentItem.transform.parent = null;
        _currentItem.Dropped();
        _currentItem = null;
        AudioPlayer.Instance.PlaySFX(AudioPlayer.SFX.pickup);
    }

    public void ForgetItem()
    {
        _currentItem = null;
    }

    public void DestroyItem()
    {
        if (_currentItem == null)
            return;
        Debug.Log($"Destroy item {_currentItem.gameObject.name}");
        Destroy(_currentItem.gameObject);
        _currentItem = null;
    }
}

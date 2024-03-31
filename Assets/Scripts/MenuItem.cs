using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuItem : InventoryItem
{
    public static UnityAction MenuDestroyed;
    public override void Dropped()
    {
    }

    public override Vector3 GetAttachOffset()
    {
        return Vector3.zero;
    }

    public override void PickedUp()
    {
    }

    public override bool UseAtPos(Vector2 pos)
    {
        return false;
    }

    public override void WillBeDestroyed()
    {
        MenuDestroyed?.Invoke();
    }
}

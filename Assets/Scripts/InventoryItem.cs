using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

abstract public class InventoryItem : MonoBehaviour
{
    public UnityAction Destroyed;

    abstract public bool UseAtPos(Vector2 pos);

    abstract public Vector3 GetAttachOffset();

    abstract public void PickedUp();

    abstract public void Dropped();
}

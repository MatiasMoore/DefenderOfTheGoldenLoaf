using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BasicTable : TablePrimitive
{
    public override void ItemPickedUp()
    {
    }

    public override void ItemPlaced()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBucket : Consumable
{
    [SerializeField]
    public GameObject _slowArea;
    [SerializeField]
    public float _slowTime;
    [SerializeField]
    public Vector2 _size;

    [SerializeField]
    public Vector2 _attachOffset;

    private SlowArea _slowAreaComponent;

    private Timer _cooldown;
    [SerializeField]
    public float _cooldownTime;
    [SerializeField]
    public int _usesAmount;

    private bool _isCooldown;


    public override void Dropped()
    {
        
    }

    public override Vector3 GetAttachOffset()
    {
        return _attachOffset;
    }

    public override void PickedUp()
    {
        
    }

    public override bool UseAtPos(Vector2 pos)
    {
        if (_isCooldown)
        {
            return false;
        }
        
        if (_usesAmount <= 0)
        {
            return false;
        }

        if (!CanBePlaced(pos))
        {
            return false;
        }

        Debug.Log("Water bucket used at " + pos);

        var slowArea = Instantiate(_slowArea, new Vector3(pos.x, pos.y, -1), Quaternion.identity);
        slowArea.transform.localScale = _size;
        _slowAreaComponent = slowArea.GetComponent<SlowArea>();
        _slowAreaComponent.ActivateForSeconds(_slowTime);
        
        _usesAmount--;

        _cooldown = new Timer(_cooldownTime);
        _cooldown.OnTimerDone += StopCooldown;
        _cooldown.StartTimer();

        _isCooldown = true;
        
        if (_usesAmount <= 0)
        {
            Destroyed?.Invoke();
        }

        return true;
    }

    private void Update()
    {
        if (_cooldown == null)
        {
            return;
        }
        _cooldown.Tick();
    }

    private void StopCooldown()
    {
        _isCooldown = false;
    }

    private bool CanBePlaced(Vector2 pos)
    {
        var colliders = Physics2D.OverlapAreaAll(pos - _size / 2, pos + _size / 2);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("Wall"))
            {
                return false;
            }
        }
        return true;
    }

    public override void WillBeDestroyed()
    {
    }
}

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

    [SerializeField]
    public float _cooldownTime;
    [SerializeField]
    public int _usesAmount;

    [SerializeField]
    private List<AudioPlayer.SFX> _soundEffects = new ();

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
        OnPickedUp?.Invoke();
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

        StartCoroutine(Cooldown());

        _isCooldown = true;

        var sfx = _soundEffects[Random.Range(0, _soundEffects.Count)];
        AudioPlayer.Instance.PlaySFX(sfx);

        if (_usesAmount <= 0)
        {
            Destroyed?.Invoke();
        }

        return true;
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldownTime);
        _isCooldown = false;
        yield break;
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

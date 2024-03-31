using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    [SerializeField]
    private AmmoBar _capacityNumbers;

    [SerializeField]
    private Transform _spawn;

    [SerializeField]
    private GameObject _itemToCreate;

    [SerializeField]
    private int _maxCapacity = 10;

    [SerializeField]
    private int _itemsPerCycle = 2;

    [SerializeField]
    private int _currentItems = 0;

    [SerializeField]
    private int _cycleCountLimit = 10;

    [SerializeField]
    private bool _infiniteCycles = true;

    [SerializeField]
    private float _cycleDuration = 2;

    private bool _isCreating = false;

    private void Start()
    {
        StartCreating();
    }

    private void Update()
    {
        if (_spawn.childCount == 0 && _currentItems > 0)
        {
            _currentItems--;
            Instantiate(_itemToCreate, this.transform.position + new Vector3(0, 0, -1), Quaternion.identity, _spawn);
        }

        _capacityNumbers.UpdateAmmo(_currentItems + _spawn.childCount,  _maxCapacity);
    }

    public void StartCreating()
    {
        StopCreating();
        StartCoroutine(CreateIngredientsCoroutine());
        _isCreating = true;
    }

    public void StopCreating()
    {
        StopAllCoroutines();
        _isCreating = false;
    }

    public bool IsCreating() => _isCreating;

    public int GetIngredientsPerCycle() => _itemsPerCycle;

    public void SetIngredientsPerCycle(int newAmount) => _itemsPerCycle = newAmount;

    public float GetCycleDuration() => _cycleDuration;

    public void SetCycleDuration(float newDuration) => _cycleDuration = newDuration;

    private IEnumerator CreateIngredientsCoroutine()
    {
        float time = 0;
        int cyclesFinished = 0;
        while (_infiniteCycles || cyclesFinished < _cycleCountLimit)
        {
            while (_currentItems + _spawn.childCount >= _maxCapacity)
            {
                yield return null;
            }

            if (time >= _cycleDuration)
            {
                _currentItems = Mathf.Clamp(_currentItems + _itemsPerCycle, 0, _maxCapacity);
                cyclesFinished++;
                time = 0;
            }

            _capacityNumbers.UpdateReloadTime(time, _cycleDuration);
            time += Time.deltaTime;
            yield return null;
        }
        _isCreating = false;
    }
}

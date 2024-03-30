using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientStorage : MonoBehaviour
{
    [SerializeField]
    private Transform _spawn;

    [SerializeField]
    private GameObject _ingredientToCreate;

    [SerializeField]
    private int _maxCapacity = 10;

    [SerializeField]
    private int _ingredientsPerCycle = 2;

    [SerializeField]
    private int _currentIngredients = 0;

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
        if (_spawn.childCount == 0 && _currentIngredients > 0)
        {
            _currentIngredients--;
            Instantiate(_ingredientToCreate, this.transform.position + new Vector3(0, 0, -1), Quaternion.identity, _spawn);
        }
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

    public int GetIngredientsPerCycle() => _ingredientsPerCycle;

    public void SetIngredientsPerCycle(int newAmount) => _ingredientsPerCycle = newAmount;

    public float GetCycleDuration() => _cycleDuration;

    public void SetCycleDuration(float newDuration) => _cycleDuration = newDuration;

    private IEnumerator CreateIngredientsCoroutine()
    {
        float time = 0;
        int cyclesFinished = 0;
        while (_infiniteCycles || cyclesFinished < _cycleCountLimit)
        {
            if (_currentIngredients >= _maxCapacity)
                yield return null;

            if (time >= _cycleDuration)
            {
                _currentIngredients = Mathf.Clamp(_currentIngredients + _ingredientsPerCycle, 0, _maxCapacity);
                cyclesFinished++;
                time = 0;
            }

            time += Time.deltaTime;
            yield return null;
        }
        _isCreating = false;
    }
}

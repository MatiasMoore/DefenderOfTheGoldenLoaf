using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stamina : MonoBehaviour
{
    public event UnityAction<int> StaminaChanged;

    [SerializeField]
    private int _maxStamina = 100;

    [SerializeField]
    private int _staminaPerCycle = 2;

    [SerializeField]
    private int _currentStamina = 0;

    [SerializeField]
    private bool _infiniteCycles = true;

    [SerializeField]
    private float _cycleDuration = 0.2f;

    [SerializeField]
    private float _staminaDelay = 1;

    private bool _isCreating = false;
    private bool _isStaminaDelay = false;

    [SerializeField]

    private AmmoBar _stabinaBar;
    private void Start()
    {
        StartCreating();
        StaminaChanged += UpdateStamina;
    }
    private void UpdateStamina(int stamina)
    {
        _stabinaBar.UpdateReloadTime(_currentStamina, _maxStamina);
    }

    public int GetCurrentStamina() => _currentStamina;

    public int GetMaxStamina() => _maxStamina;

    public bool DecreaseStamina(int amount)
    {
        if (_currentStamina - amount >= 0)
        {
            _currentStamina -= amount;
            _isStaminaDelay = true;
            StartCoroutine(StaminaDelay());
            StaminaChanged?.Invoke(_currentStamina);
            return true;
        }
        return false;
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

    public int GetIngredientsPerCycle() => _staminaPerCycle;

    public void SetIngredientsPerCycle(int newAmount) => _staminaPerCycle = newAmount;

    public float GetCycleDuration() => _cycleDuration;

    public void SetCycleDuration(float newDuration) => _cycleDuration = newDuration;

    private IEnumerator CreateIngredientsCoroutine()
    {
        float time = 0;
        int cyclesFinished = 0;
        while (_infiniteCycles)
        {

            while (_isStaminaDelay)
            {
                yield return null;
            }

            StopCoroutine(StaminaDelay());

            if (time >= _cycleDuration)
            {
                _currentStamina = Mathf.Clamp(_currentStamina + _staminaPerCycle, 0, _maxStamina);
                StaminaChanged?.Invoke(_currentStamina);
                cyclesFinished++;
                time = 0;
            }

            time += Time.deltaTime;
            yield return null;
        }

        _isCreating = false;
    }

    private IEnumerator StaminaDelay()
    {
        yield return new WaitForSeconds(_staminaDelay);
        _isStaminaDelay = false;
    }
}

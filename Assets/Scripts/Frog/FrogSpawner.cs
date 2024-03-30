using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform _spawnPoint;
    [SerializeField]
    private int _maxFrogs;
    [SerializeField]
    private float _spawnTime;
    [SerializeField]
    private GameObject _frogPrefab;

    [SerializeField]
    private MovingAIStateManager.MovingAIStateParams _params;

    private List<GameObject> _frogs;

    private int _currentFrogs;
    private Timer _timer;

    private bool _isInit = false;
    private bool _isFrogSpawning = false;

    public void Init()
    {
        _timer = new Timer(_spawnTime);
        _isInit = true;
        _frogs = new List<GameObject>();
        OnEnable();
        StartSpawning();
    }

    [ContextMenu("Spawn frog")]
    private void SpawnFrog()
    {
        Debug.Log("Spawn frog");
        if (_currentFrogs < _maxFrogs)
        {
            _frogPrefab.GetComponent<MovingAIStateManager>().movingAIStateParam = _params;
            _frogs.Add(Instantiate(_frogPrefab, transform.position, Quaternion.identity));
            _currentFrogs++;
            if (_isFrogSpawning)
            {
                _timer.StartTimer();
            }     
        }
    }

    [ContextMenu("Start spawn frogs")]
    public void StartSpawning()
    {
        _timer.StartTimer();
        _isFrogSpawning = true;
    }

    private void Update()
    {
        if (!_isInit)
            return;
        _timer.Tick();
    }

    [ContextMenu("Stop spawn frogs")]
    public void StopSpawning()
    {
        _timer.StopTimer();
        _isFrogSpawning = false;
    }

    private void OnEnable()
    {
        if (_isInit)
        {
            _timer.OnTimerDone += SpawnFrog;
        }
    }

    private void OnDisable()
    {
        if (_isInit)
        {
            _timer.OnTimerDone -= SpawnFrog;
        }
    }
}

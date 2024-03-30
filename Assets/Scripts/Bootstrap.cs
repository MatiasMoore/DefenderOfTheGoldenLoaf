using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField]
    private PlayerControls _playerControls;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private FrogSpawner _frogSpawner;

    private void Start()
    {
        if (_playerControls != null)
        {
            _playerControls.Init();
        } else
        {
            Debug.LogWarning("Player controls not found");
        }

        if (_player != null)
        {
            _player.Init();
        } else
        {
            Debug.LogWarning("Player not found");
        }        
        
        if (_frogSpawner != null)
        {
            _frogSpawner.Init();
        } else
        {
            Debug.LogError("Frog spawner not found");
        }

    }
}

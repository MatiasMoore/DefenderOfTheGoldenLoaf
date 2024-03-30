using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBootsTrap : MonoBehaviour
{
    [SerializeField]
    private PlayerControls _playerControls;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private Inventory _inventory;

    [SerializeField]
    private FrogSpawner _frogSpawner;

    private void Start()
    {
        if (_playerControls != null)
        {
            _playerControls.Init();
        } else
        {
            Debug.LogError("Player controls not found");
        }

        if (_player != null)
        {
            _player.Init();
        } else
        {
            Debug.LogError("Player not found");
        }        

        if (_inventory != null)
        {
            _inventory.Init(_playerControls);
        } else
        {
            Debug.LogError("Inventory not found");
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

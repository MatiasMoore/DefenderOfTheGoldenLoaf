using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField]
    private PlayerControls _playerControls;
    private void Start()
    {
        if (_playerControls != null)
        {
            _playerControls.Init();
        } else
        {
            Debug.LogWarning("Player controls not found");
        }
    }
}

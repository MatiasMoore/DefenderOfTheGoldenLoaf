using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void Init()
    {
        PlayerMovementController playerMovementController = GetComponent<PlayerMovementController>();
        playerMovementController.Init(PlayerControls.Instance);
        PlayerKickController playerKickController = GetComponent<PlayerKickController>();
        playerKickController.Init();
    }
}

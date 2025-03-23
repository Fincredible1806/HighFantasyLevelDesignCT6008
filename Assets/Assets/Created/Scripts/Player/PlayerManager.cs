using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerLocomotion playerLoco;
    CameraManager camManager;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        camManager = FindObjectOfType<CameraManager>();
        playerLoco = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        inputManager.HandleAllInput();
    }

    private void FixedUpdate()
    {
        playerLoco.HandleAllMovement();
    }

    private void LateUpdate()
    {
        camManager.AllCamMovement();
    }

}

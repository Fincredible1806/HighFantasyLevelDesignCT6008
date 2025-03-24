using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerLocomotion playerLoco;
    Animator animator;
    CameraManager camManager;

    public bool isInteracting;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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

        isInteracting = animator.GetBool("isLocked");
    }

}

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
    public bool isUsingRootMotion;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        camManager = FindObjectOfType<CameraManager>();
        playerLoco = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        if(playerLoco.canPlayCharacter)
        {
            inputManager.HandleAllInput();
        }
        
    }

    private void FixedUpdate()
    {
        if (playerLoco.canPlayCharacter)
        {
            playerLoco.HandleAllMovement();
        }
    }

    private void LateUpdate()
    {
        if (playerLoco.canPlayCharacter)
        {
            camManager.AllCamMovement();

            isInteracting = animator.GetBool("isLocked");
            isUsingRootMotion = animator.GetBool("isUsingRootMotion");
            playerLoco.isJumping = animator.GetBool("isJumping");
            animator.SetBool("isGrounded", playerLoco.isGrounded);
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerAnimationManager animationManager;

    public Vector2 moveInput;
    public Vector2 camInput;

    public float camInputX;
    public float camInputY;

    private float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    private void Awake()
    {
        animationManager = GetComponent<PlayerAnimationManager>();
    }
    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => moveInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => camInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInput()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = moveInput.y;
        horizontalInput = moveInput.x;


        camInputY = camInput.y;
        camInputX = camInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animationManager.UpdateAnimatorValues(0, moveAmount);
    }
}

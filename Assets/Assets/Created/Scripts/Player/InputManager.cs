using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Users;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion locomotion;
    PlayerAnimationManager animationManager;

    public Vector2 moveInput;
    public Vector2 camInput;

    public float camInputX;
    public float camInputY;
    [Range(0.001f, 1f)]
    [SerializeField] private float sensetivityX;
    [Range(0.001f, 1f)]
    [SerializeField] private float sensetivityY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool sprintInput;

    private void Awake()
    {
        animationManager = GetComponent<PlayerAnimationManager>();
        locomotion = GetComponent<PlayerLocomotion>();
    }
    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => moveInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => camInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Sprinting.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprinting.canceled += i => sprintInput = false;



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
        HandleSprintInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = moveInput.y;
        horizontalInput = moveInput.x;


        camInputY = camInput.y * sensetivityY;
        camInputX = camInput.x * sensetivityX;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animationManager.UpdateAnimatorValues(0, moveAmount, locomotion.isSprinting);
    }

    private void HandleSprintInput()
    {
        if(sprintInput && moveAmount > 0.5f)
        {
            locomotion.isSprinting = true;
        }
        else
        {
            locomotion.isSprinting= false;
        }
    }
}

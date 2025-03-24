using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    Vector3 moveDir;
    Transform cameraObject;
    Rigidbody playerRb;
    PlayerManager manager;
    PlayerAnimationManager animManager;

    [Header("Falling")]
    public float airTime;
    public float fallingSpeed;
    public float leapVelocity;
    public float rayCastHeightOffset = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Move flags")]
    public bool isGrounded;
    public bool isFalling;
    public bool isSprinting;

    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 2f;
    public float runSpeed= 5;
    [SerializeField] private float sprintSpeed = 7f;
    public float rotSpeed = 15;

    private void Awake()
    {
        manager = GetComponent<PlayerManager>();
        animManager = GetComponent<PlayerAnimationManager>();
        inputManager = GetComponent<InputManager>();
        playerRb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleFalling();

        if (manager.isInteracting)
        {
            return;
        }
        HandleMovemet();
        RotateHandler();

    }
    private void HandleMovemet()
    {

        moveDir = new Vector3(cameraObject.forward.x, 0f, cameraObject.forward.z) * inputManager.verticalInput;
        moveDir += cameraObject.right * inputManager.horizontalInput;
        moveDir.Normalize();
        moveDir.y = 0;

        if(isSprinting)
        {
            moveDir *= sprintSpeed;

        }

        else 
        {
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDir *= runSpeed;
            }
            else
            {
                moveDir *= walkSpeed;
            }
        }

        Vector3 movementVelocity = moveDir;
        playerRb.velocity = movementVelocity;
    }

    private void RotateHandler()
    {
        Vector3 targetDir = Vector3.zero;

        targetDir = cameraObject.forward * inputManager.verticalInput;
        targetDir += cameraObject.right * inputManager.horizontalInput;
        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }

        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

        transform.rotation = playerRot;
    }

    private void HandleFalling()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;
        if (!isGrounded)
        {
            if(!manager.isInteracting)
            {
                animManager.PlayTargetAnimation("Falling", true);   
            }

            airTime += Time.deltaTime;

            playerRb.AddForce(transform.forward * leapVelocity);
            playerRb.AddForce(airTime * fallingSpeed * -Vector3.up);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, 0.5f, groundLayer))
        {
            if (!isGrounded && !manager.isInteracting)
            {
                animManager.PlayTargetAnimation("Land", true);
            }

            airTime = 0;
            isFalling = false;
            animManager.FallBool(isFalling);
            isGrounded = true;
            manager.isInteracting = false;
        }

        else
        {
            isFalling = true;
            isGrounded = false;
            animManager.FallBool(isFalling);
        }

    }
}

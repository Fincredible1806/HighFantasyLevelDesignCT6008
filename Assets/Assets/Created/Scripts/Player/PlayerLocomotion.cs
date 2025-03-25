using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    Vector3 moveDir;
    Transform cameraObject;
    public Rigidbody playerRb;
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
    public bool isSprinting;
    public bool isJumping;

    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 2f;
    public float runSpeed= 5;
    [SerializeField] private float sprintSpeed = 7f;
    public float rotSpeed = 15;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 3;
    [SerializeField] private float gravIntensity = -15;
    [SerializeField] private float jumpCoolDown = 2f;
    [SerializeField] private float jumpTimePassed = 1f;

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

        if (manager.isInteracting || manager.isUsingRootMotion)
        {
            return;
        }
        HandleMovemet();
        RotateHandler();

    }
    private void HandleMovemet()
    {
        if(isJumping)
        {
            return;
        }
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
        if (isJumping)
        {
            return; 
        }

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
        Vector3 targetPos;
        rayCastOrigin.y += rayCastHeightOffset;
        targetPos = transform.position;

        if (!isGrounded && !isJumping) 
        {
            if(!manager.isInteracting)
            {
                animManager.PlayTargetAnimation("Falling", true);   
            }

            animManager.playerAnimator.SetBool("isUsingRootMotion", false);
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

            Vector3 rayCastHitPoint = hit.point;
            targetPos.y = rayCastHitPoint.y;

            airTime = 0;
            animManager.FallBool(false);
            isGrounded = true;
            manager.isInteracting = false;
        }

        else
        {
            isGrounded = false;
            animManager.FallBool(true);
        }

        if(isGrounded && !isJumping)
        {
            if(manager.isInteracting || inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime / 0.1f);
            }

            else
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime / 0.1f);
            }

        }

    }

    public void HandleJump()
    {
        if (isGrounded && jumpTimePassed >= jumpCoolDown)
        {
            jumpTimePassed = 0;
            animManager.playerAnimator.SetBool("isJumping", true);
            animManager.PlayTargetAnimation("Jumping", false);

            float jumpVelocity = Mathf.Sqrt(-2 * gravIntensity * jumpHeight);
            Vector3 playerVelocity = new(moveDir.x, jumpVelocity, moveDir.z);
            playerRb.velocity = playerVelocity;
        }
    }

    public void HandleDodge()
    {
        if(manager.isInteracting)
        {
            return;
        }

        animManager.PlayTargetAnimation("Dodge", true, true);

        //Toggle Invulvn

    }

    private void FixedUpdate()
    {
        jumpTimePassed = Mathf.Clamp(jumpTimePassed + Time.deltaTime, 0, 2.5f);
    }
}

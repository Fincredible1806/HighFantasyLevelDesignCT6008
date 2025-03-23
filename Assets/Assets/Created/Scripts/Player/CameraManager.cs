using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    InputManager inpManager;
    public Transform targetTransform; //Obj to follow
    private Transform cameraTransform; // Transform of actual camera
    private Transform camPivot; //Obj for Pivot
    private float defaultPos;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPos;

    [SerializeField] LayerMask collisionLayers;
    [SerializeField] string camPivotName;

    [Header("Numerical Variables")]
    [SerializeField] float camCollisionOffset = 0.2f; // Cam jump distance for objects colliding with
    [SerializeField] float camCollisionRadius = 0.2f;
    [SerializeField] float minCollisionOffset = 0.2f;
    [SerializeField] float camLookSpeed = 2;
    [SerializeField] float camPivotSpeed = 2;
    [SerializeField] float minPivotAngle, maxPivotAngle;

    public float lookAngle; // Up and Down
    public float pivotAngle; // Left and Right
    public float camFollowSpeed = 0.2f;
    private void Awake()
    {
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        inpManager = FindObjectOfType<InputManager>();
        camPivot = GameObject.Find(camPivotName).transform;
        cameraTransform = Camera.main.transform;
        defaultPos = cameraTransform.localPosition.z;
    }

    public void AllCamMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCollision();
    }

    private void FollowTarget()
    {
        Vector3 targetPos = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, camFollowSpeed);

        transform.position = targetPos;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRot;

       lookAngle = lookAngle + (inpManager.camInputX * camLookSpeed);
       pivotAngle = pivotAngle - (inpManager.camInputY * camPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRot = Quaternion.Euler(rotation);
        transform.rotation = targetRot;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRot = Quaternion.Euler(rotation);
        camPivot.localRotation = targetRot;
    }

    private void HandleCollision()
    {
        float targetPos = defaultPos;
        RaycastHit hit;
        Vector3 direction;
        direction = cameraTransform.position - camPivot.position;
        direction.Normalize();

        if (Physics.SphereCast
            (camPivot.transform.position, camCollisionRadius, direction, out hit, Mathf.Abs(targetPos), collisionLayers))
        {
            float distance;
            distance = Vector3.Distance(camPivot.position, hit.point);
            targetPos =- (distance - camCollisionOffset);
        }

        if (Mathf.Abs(targetPos) < minCollisionOffset)
        {
            targetPos -= minCollisionOffset;
        }

        cameraVectorPos.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPos, 0.2f);
        cameraTransform.localPosition = cameraVectorPos;
    }
}

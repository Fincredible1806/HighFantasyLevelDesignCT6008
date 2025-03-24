using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public Animator playerAnimator;
    int horizontal;
    int vertical;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void PlayTargetAnimation(string targetAnim, bool isLocked)
    {
        playerAnimator.SetBool("isLocked", isLocked);
        playerAnimator.CrossFade(targetAnim, 0.2f);
    }

    public void UpdateAnimatorValues(float horizontalMove, float verticalMove, bool isSprinting)
    {
        //Animation Snapping 
        float snappedHorizontal;
        float snappedVertical;

        #region Snapped Horizontal
        if (horizontalMove > 0 && horizontalMove < 0.55f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (horizontalMove > 0.55f)
        {
            snappedHorizontal = 1f;
        }
        else if (horizontalMove < 0f && horizontalMove > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalMove < -0.55f)
        {
            snappedHorizontal = -1f;
        }
        else
        {
            snappedHorizontal = 0f;
        }
        #endregion
        #region Snapped Vertical
        if (verticalMove > 0 && verticalMove < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMove > 0.55f)
        {
            snappedVertical = 1f;
        }
        else if (verticalMove < 0f && verticalMove > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMove < -0.55f)
        {
            snappedVertical = -1f;
        }
        else
        {
            snappedVertical = 0f;
        }
        #endregion

        if (isSprinting)
        {
            snappedVertical = 2;
            snappedHorizontal = horizontalMove;
        }

        playerAnimator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        playerAnimator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    public void FallBool(bool faller)
    {
        playerAnimator.SetBool("isFalling", faller);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class QTEevent : MonoBehaviour
{
    [SerializeField] GameObject QTEcanvas;
    [SerializeField] TextMeshProUGUI timeLeftText;
    public bool isHappening = false;
    private bool hasPressed;
    float timeToPress;
    float timePassed;
    [SerializeField] KeyCode keyToPress;
    [SerializeField] KeyCode controllerButtonToPress;
    [SerializeField] Animator animator;
    AnimationClip loseClip;
    AnimationClip winClip;
    TimelineAsset winAsset;


    private void Update()
    {
        if (isHappening && !hasPressed)
        {

            timePassed += Time.deltaTime;
            timeLeftText.text = (timeToPress - timePassed).ToString();
            if(Input.GetKeyDown(keyToPress) || Input.GetKeyDown(controllerButtonToPress))
            {
                hasPressed = true;
            }
            if(timePassed >= timeToPress)
            {
                isHappening = false;
                animator.Play(loseClip.name);
            }
        }

        else if(hasPressed && isHappening)
        {
            EventTrigger();
        }
    }
    public void EventTrigger()
    {
        animator.Play(winClip.name);
    }
}

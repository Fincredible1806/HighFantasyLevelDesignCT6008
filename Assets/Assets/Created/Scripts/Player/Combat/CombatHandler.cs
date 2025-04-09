using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    private Animator animator;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int numOfClicks = 0;
    float lastClickTime = 0f;
    float maxComboDelay = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


}

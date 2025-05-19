using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/Normal attack")]
public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController animatorOverride;
    public float damage;
    public bool isStagger;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public List<AttackSO> combo;
    float lastClickTime;
    float lastComboEnd;
    int comboCounter;
    float cooldown = 0.2f;
    float attackWindow = 0.6f;
    bool attackStagger;
    bool canAttack = true;
    
    [SerializeField] Animator animator;
    [SerializeField] CombatHandler handler;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && canAttack)
        {
            Attack();
        }
        ExitAttack();
    }

    private void Attack()
    {
        if (Time.time - lastComboEnd > cooldown && comboCounter <= combo.Count)
        {
            CancelInvoke("EndCombo");

            if(Time.time - lastClickTime >= attackWindow)
            {
                animator.runtimeAnimatorController = combo[comboCounter].animatorOverride;
                animator.CrossFadeInFixedTime("Attack", 0.5f);
                handler.damage = combo[comboCounter].damage;
                attackStagger = combo[comboCounter].isStagger;
                comboCounter++;
                lastClickTime = Time.time;

                if(comboCounter + 1 > combo.Count)
                {
                    comboCounter = 0;  
                    EndCombo();
                }
            }
        }
    }

    void ExitAttack()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("EndCombo", 1);
        }
    }

    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
    }

}

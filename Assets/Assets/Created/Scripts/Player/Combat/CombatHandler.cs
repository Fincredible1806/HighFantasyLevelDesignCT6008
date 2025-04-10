using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    public float damage;

    BoxCollider triggerBox;

    private void Start()
    {
        triggerBox = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<EnemyLife>();
        if(enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }

    public void EnableTriggerBox()
    {
        triggerBox.enabled = true;
    }

    public void DisableTriggerBox()
    {
        triggerBox.enabled = false;
    }
}

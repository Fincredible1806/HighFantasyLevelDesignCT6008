using System.Collections;
using System.Collections.Generic;
using UnityChan;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHIt : MonoBehaviour
{
    [SerializeField] float knockbackForce;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.transform.position += transform.forward * Time.deltaTime * knockbackForce;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] List<Transform> wayPoints = new List<Transform>();
    [SerializeField] GameObject wayPointHolder;
    [Header("Variables")]
    [SerializeField] float playerDistance;
    [SerializeField] float attackRange;
    [SerializeField] float sightRange;
    [SerializeField] float edgeOffset;
    bool inAttackRange;
    bool inSightRange;
    [SerializeField] Vector3 sightOffset;
    [SerializeField] float walkSpeed;
    [SerializeField] float chaseSpeed;
    NavMeshAgent navMeshAgent;
    bool isPatrolling;
    Transform player;
    Animator animator;
    public LayerMask playerLayer;
    bool isAttacking = false;
    bool alreadyAttacked = false;
    [SerializeField] float timeBetweenAttacks;

    float health;
    [SerializeField] float maxHealth;
    [SerializeField] bool isFleeting = false;
    bool hasFleeted = false;
    [SerializeField] float fleetingTime;
    float fleetPassed;
    [SerializeField] Transform fleetLocation;


    private void Awake()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        walkSpeed = navMeshAgent.speed;
        chaseSpeed = (float)(walkSpeed * 1.5);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isFleeting)
        {
            inSightRange = Physics.CheckSphere(transform.position + sightOffset, sightRange, playerLayer);
            inAttackRange = Physics.CheckSphere(transform.position + sightOffset, attackRange, playerLayer);
            if (!inAttackRange)
            {
                animator.SetBool("isAttacking", false);
            }
            ChaseManagemet();
            PatrolManagement();

        }

        if(isFleeting)
        {
            fleetPassed += Time.deltaTime;
            navMeshAgent.SetDestination(fleetLocation.position);
            if(fleetPassed >= fleetingTime)
            {
                isFleeting = false;
            }
        }

    }

    private void PatrolManagement()
    {
        isPatrolling = animator.GetBool("isPatrolling");
        if (isPatrolling)
        {
            navMeshAgent.speed = walkSpeed;
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                navMeshAgent.SetDestination(wayPoints[UnityEngine.Random.Range(0, wayPoints.Count)].position);
            }
        }
        else if (!isPatrolling && animator.GetBool("isChasing") == false)
        {
            navMeshAgent.SetDestination(transform.position);
        }
    }

    void ChaseManagemet()
    {
        navMeshAgent.speed = chaseSpeed;
        if(!inSightRange && !inAttackRange)
        {
            animator.SetBool("isWaiting", false);
            animator.SetBool("isChasing", false);
        }

        if (inSightRange && !inAttackRange)
        {
            animator.SetBool("isPatrolling", false);
            animator.SetBool("isChasing", true);
            animator.SetBool("isWaiting", false);
            navMeshAgent.SetDestination(player.position);
        }

        if (inSightRange && inAttackRange)
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isPatrolling", false);
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        navMeshAgent.SetDestination(transform.position);
        transform.LookAt(player.position);

        if (!alreadyAttacked)
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isWaiting", false);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;

    }

    public void TakeDamage(float damage)
    {
        health = health - damage;
        if(health == maxHealth/2 && !hasFleeted)
        {
            isFleeting = true;
            hasFleeted = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position+ sightOffset, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + sightOffset, attackRange);
    }
}

﻿using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform Player;

    public LayerMask WhatisGround, WhatIsPlayer, obstacle;

    // patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPopintRange;

    // attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, PlayerInAttackRange;
   
    
    
    
    public GameObject Projectile;

    void Awake()
    {
        Player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //Chcle for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);

        #region When Duraing the chase, Player hide behind the obstacle then chase mode switch to patrol
      
        if (playerInSightRange)
            playerInSightRange = !Physics.Linecast(transform.position, Player.transform.position, obstacle);

        #endregion

        if (!playerInSightRange && !PlayerInAttackRange) Patroling();
        if (playerInSightRange && !PlayerInAttackRange) ChasePlayer();
        if (PlayerInAttackRange && playerInSightRange) AttackPlayer();
    }

    void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.sqrMagnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // calculate random point in range
        float randomZ = UnityEngine.Random.Range(-walkPopintRange, walkPopintRange);
        float randomX = UnityEngine.Random.Range(-walkPopintRange, walkPopintRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, WhatisGround))
            walkPointSet = true;
    }

    void ChasePlayer()
    {
        agent.SetDestination(Player.position);
    }

    void AttackPlayer()
    {
        // Make sure enemy doesnt move
        agent.SetDestination(transform.position);

        transform.LookAt(Player);

        if(!alreadyAttacked)
        {
            #region Attack Code
            Rigidbody rb = Instantiate(Projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            #endregion

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

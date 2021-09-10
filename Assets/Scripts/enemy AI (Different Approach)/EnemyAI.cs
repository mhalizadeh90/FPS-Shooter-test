using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform Player;

    public LayerMask WhatisGround, WhatIsPlayer, obstacle;
    public float GroundRaycastDistance = 0.1f;

    // patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPopintRange;
    public float TimeForChangeWalkPoint = 5;
    float nextChangeWalkPoint = 0;
    // attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, PlayerInAttackRange;


    public bool isGunner;
    public GameObject Projectile;

    public ParticleSystem LeftGunParticle;
    public ParticleSystem RightGunParticle;
    public ParticleSystem KnifeParticle;

    public AudioSource ShootSFXPlayer;
    public float AttackDamage;

    [Range(0, 1)] public float MissShotPercentage;
    private const float shootOffsetToPlayer = 5;

    public AIBrain aIBrains;

    Health EnemyHealth;

    void Awake()
    {
        Player = GameObject.FindWithTag("Player").transform;
        EnemyHealth = GetComponent<Health>();
        //agent = GetComponent<NavMeshAgent>();
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

        if (distanceToWalkPoint.sqrMagnitude < 5 || nextChangeWalkPoint < Time.time)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // calculate random point in range

        float randomZ = UnityEngine.Random.Range(-walkPopintRange, walkPopintRange);
        float randomX = UnityEngine.Random.Range(-walkPopintRange, walkPopintRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, GroundRaycastDistance, WhatisGround))
        {
            walkPointSet = true;
            nextChangeWalkPoint = TimeForChangeWalkPoint + Time.time;
        }
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
            //Rigidbody rb = Instantiate(Projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            //==========
            //TODO: REFACTOR
            // for gunner
            if(isGunner)
            {
                LeftGunParticle.Play();
                RightGunParticle.Play();
               
                RaycastHit hit;

                Vector3 aimDirection = (Player.transform.position - transform.position).normalized;
                
                aimDirection.y += UnityEngine.Random.Range(0, MissShotPercentage);
                aimDirection.x += UnityEngine.Random.Range(0, MissShotPercentage);
                aimDirection.z += UnityEngine.Random.Range(0, MissShotPercentage);

                bool isPlayerGetShot = Physics.Raycast(transform.position, aimDirection, out hit, attackRange + shootOffsetToPlayer, WhatIsPlayer);

                if (isPlayerGetShot)
                {
                    Debug.Log("Damage: "+ hit.transform.name +" ["+AttackDamage+"]");
                    PlayerHealth target = hit.transform.GetComponent<PlayerHealth>();
                    target?.TakeDamage(AttackDamage);
                }

                //TODO: RAYCAST FOR SHOOT AND THEN DAMAGE IF HITT THE PLAYER
            }
            else
            {
                KnifeParticle.Play();
                //TODO: RAYCAST FOR KNIFE AND THEN DAMAGE IF HITT THE PLAYER
                RaycastHit hit;

                Vector3 aimDirection = (Player.transform.position - transform.position).normalized;

                aimDirection.y += UnityEngine.Random.Range(0, MissShotPercentage);
                aimDirection.x += UnityEngine.Random.Range(0, MissShotPercentage);
                aimDirection.z += UnityEngine.Random.Range(0, MissShotPercentage);

                bool isPlayerGetShot = Physics.Raycast(transform.position, aimDirection, out hit, attackRange + shootOffsetToPlayer, WhatIsPlayer);

                if (isPlayerGetShot)
                {
                    Debug.Log("Damage: " + hit.transform.name + " [" + AttackDamage + "]");
                    PlayerHealth target = hit.transform.GetComponent<PlayerHealth>();
                    target?.TakeDamage(AttackDamage);
                }
            }

            ShootSFXPlayer.Play();

            #endregion

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void ActivateAIBrainBasedOnDificultyWave(int Wave)
    {
        sightRange = aIBrains.SightRange + (aIBrains.SightRange* aIBrains.SightRangeUpdateStep * Wave);
        attackRange = aIBrains.AttackRange + (aIBrains.AttackRange * aIBrains.AttackRangeUpdateStep * Wave);
        MissShotPercentage = Mathf.Clamp(aIBrains.MissShotPercentage + (aIBrains.MissShotPercentage * aIBrains.MissShotPercentageUpdateStep * Wave),0,1);
        agent.acceleration = aIBrains.MaxAcceleration + (aIBrains.MaxAcceleration * aIBrains.MaxAccelerationUpdateStep * Wave);
        agent.speed = aIBrains.MaxSpeed + (aIBrains.MaxSpeed * aIBrains.MaxSpeedUpdateStep * Wave);
        EnemyHealth.health = aIBrains.MaxHealth + (aIBrains.MaxHealth * aIBrains.MaxHealthUpdateStep * Wave);
        AttackDamage = aIBrains.AttackDamage + (aIBrains.AttackDamage * aIBrains.AttackDamageUpdateStep * Wave);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

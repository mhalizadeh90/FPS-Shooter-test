using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    #region Fields

    [SerializeField] AIBrain aiBrain;
    public NavMeshAgent agent;


    [SerializeField] LayerMask WhatisGround, WhatIsPlayer, obstacle;
    [SerializeField] float GroundRaycastDistance = 0.1f;

    // patroling
    Vector3 walkPoint;
    bool walkPointSet;
    [SerializeField] float patrolingDistanceRange;
    [SerializeField] float MaxDurationOfEachPatrolingCycle = 5;
    float nextTimeForChangingWalkPoint = 0;

    // attacking
    public float timeBetweenAttacks;
    protected bool isAlreadyInAttackedState;
    float AttackDamage;
    float missShotPercentage;
    protected const float attackRangeOffset = 5;

    //States
    float sightRange, attackRange;
    bool playerInSightRange, PlayerInAttackRange;

    Transform playerPosition;
    protected PlayerHealth playerHealth;
    Health EnemyHealth;
    
    [SerializeField] protected AudioSource AttackAudioPlayer;

    #endregion

    void Awake()
    {
        playerPosition = GameObject.FindObjectOfType<PlayerMovement>().transform;
        playerHealth = playerPosition.GetComponent<PlayerHealth>();
        EnemyHealth = GetComponent<Health>();
    }

    void Update()
    {
        CheckForSightRange();
        CheckForAttackRange();

        if (!playerInSightRange && !PlayerInAttackRange) Patroling();
        if (playerInSightRange && !PlayerInAttackRange) ChasePlayer();
        if (PlayerInAttackRange && playerInSightRange) Attack();
    }

    private void CheckForSightRange()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        if (playerInSightRange) playerInSightRange = !Physics.Linecast(transform.position, playerPosition.transform.position, obstacle);
    }

    private void CheckForAttackRange()
    {
        PlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);
    }

    void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.sqrMagnitude < 5 || nextTimeForChangingWalkPoint < Time.time)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // calculate random point in range

        float randomZ = UnityEngine.Random.Range(-patrolingDistanceRange, patrolingDistanceRange);
        float randomX = UnityEngine.Random.Range(-patrolingDistanceRange, patrolingDistanceRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, GroundRaycastDistance, WhatisGround))
        {
            walkPointSet = true;
            nextTimeForChangingWalkPoint = MaxDurationOfEachPatrolingCycle + Time.time;
        }
    }

    protected void StandStillAndLookAtPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(playerPosition);
    }

    protected void AimAndAttack()
    {
        Vector3 aimDirection = GetAimDirection();
        if (isPlayerGetShot(aimDirection)) playerHealth?.TakeDamage(AttackDamage);
    }

    protected void SetNextAttackTimeBasedOnFireRate()
    {
        isAlreadyInAttackedState = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    void ChasePlayer()
    {
        agent.SetDestination(playerPosition.position);
    }

    public virtual void Attack()
    {
        //ENEMY ATTACK
    }

    protected Vector3 GetAimDirection()
    {
        Vector3 aimDirection = (playerPosition.transform.position - transform.position).normalized;

        aimDirection.y += UnityEngine.Random.Range(0, missShotPercentage);
        aimDirection.x += UnityEngine.Random.Range(0, missShotPercentage);
        aimDirection.z += UnityEngine.Random.Range(0, missShotPercentage);

        return aimDirection;
    }

    protected bool isPlayerGetShot(Vector3 ShootDirection)
    {
        RaycastHit hit;
        return(Physics.Raycast(transform.position, ShootDirection, out hit, attackRange + attackRangeOffset, WhatIsPlayer));
    }

    //void AttackPlayer()
    //{
    //    // Make sure enemy doesnt move
    //    agent.SetDestination(transform.position);

    //    transform.LookAt(Player);

    //    if(!alreadyAttacked)
    //    {
    //        #region Attack Code
    //        //TODO: REFACTOR
    //        // for gunner
    //        if(isGunner)
    //        {
    //            LeftGunParticle.Play();
    //            RightGunParticle.Play();

    //            RaycastHit hit;

    //            Vector3 aimDirection = (Player.transform.position - transform.position).normalized;

    //            aimDirection.y += UnityEngine.Random.Range(0, MissShotPercentage);
    //            aimDirection.x += UnityEngine.Random.Range(0, MissShotPercentage);
    //            aimDirection.z += UnityEngine.Random.Range(0, MissShotPercentage);

    //            bool isPlayerGetShot = Physics.Raycast(transform.position, aimDirection, out hit, attackRange + shootOffsetToPlayer, WhatIsPlayer);

    //            if (isPlayerGetShot)
    //            {
    //                Debug.Log("Damage: "+ hit.transform.name +" ["+AttackDamage+"]");
    //                PlayerHealth target = hit.transform.GetComponent<PlayerHealth>();
    //                target?.TakeDamage(AttackDamage);
    //            }

    //            //TODO: RAYCAST FOR SHOOT AND THEN DAMAGE IF HITT THE PLAYER
    //        }
    //        else
    //        {
    //            KnifeParticle.Play();
    //            //TODO: RAYCAST FOR KNIFE AND THEN DAMAGE IF HITT THE PLAYER
    //            RaycastHit hit;

    //            Vector3 aimDirection = (Player.transform.position - transform.position).normalized;

    //            aimDirection.y += UnityEngine.Random.Range(0, MissShotPercentage);
    //            aimDirection.x += UnityEngine.Random.Range(0, MissShotPercentage);
    //            aimDirection.z += UnityEngine.Random.Range(0, MissShotPercentage);

    //            bool isPlayerGetShot = Physics.Raycast(transform.position, aimDirection, out hit, attackRange + shootOffsetToPlayer, WhatIsPlayer);

    //            if (isPlayerGetShot)
    //            {
    //                Debug.Log("Damage: " + hit.transform.name + " [" + AttackDamage + "]");
    //                PlayerHealth target = hit.transform.GetComponent<PlayerHealth>();
    //                target?.TakeDamage(AttackDamage);
    //            }
    //        }

    //        ShootSFXPlayer.Play();

    //        #endregion

    //        alreadyAttacked = true;
    //        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    //    }
    //}

    protected void ResetAttack()
    {
        isAlreadyInAttackedState = false;
    }

    public void ActivateAIBrainBasedOnDificultyWave(int Wave)
    {
        sightRange = aiBrain.SightRange + (aiBrain.SightRange* aiBrain.SightRangeUpdateStep * Wave);
        attackRange = aiBrain.AttackRange + (aiBrain.AttackRange * aiBrain.AttackRangeUpdateStep * Wave);
        missShotPercentage = Mathf.Clamp(aiBrain.MissShotPercentage + (aiBrain.MissShotPercentage * aiBrain.MissShotPercentageUpdateStep * Wave),0,1);
        agent.acceleration = aiBrain.MaxAcceleration + (aiBrain.MaxAcceleration * aiBrain.MaxAccelerationUpdateStep * Wave);
        agent.speed = aiBrain.MaxSpeed + (aiBrain.MaxSpeed * aiBrain.MaxSpeedUpdateStep * Wave);
        EnemyHealth.health = aiBrain.MaxHealth + (aiBrain.MaxHealth * aiBrain.MaxHealthUpdateStep * Wave);
        AttackDamage = aiBrain.AttackDamage + (aiBrain.AttackDamage * aiBrain.AttackDamageUpdateStep * Wave);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

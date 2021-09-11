using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    #region Fields

    [SerializeField] AIBrain aiBrain;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] int DefaultAIBrainLevel;

    [SerializeField] LayerMask WhatisGround, WhatIsPlayer, obstacle;
    [SerializeField] float GroundRaycastDistance = 0.1f;

    // patroling
    Vector3 walkPoint;
    bool isWalkPointSet;
    [SerializeField] float patrolingDistanceRange;
    [SerializeField] float MaxDurationOfEachPatrolingCycle = 5;
    float nextTimeForChangingWalkPoint = 0;
    const float minimumDistanceToWalkPoint = 5;


    // attacking
    [SerializeField] float timeBetweenAttacks;
    protected bool isAlreadyInAttackedState;
    float AttackDamage;
    float missShotPercentage;
    protected const float attackRangeOffset = 5;

    //States
    float sightRange, attackRange;
    bool playerInSightRange, PlayerInAttackRange;

    Transform playerPosition;
    protected IDamagable playerHealth;
    AIHealth EnemyHealth;
    
    [SerializeField] protected AudioSource AttackAudioPlayer;

    #endregion

    void Awake()
    {
        playerPosition = GameObject.FindObjectOfType<PlayerMovement>().transform;
        playerHealth = playerPosition.GetComponent<IDamagable>();
        EnemyHealth = GetComponent<AIHealth>();
        SetAIBrainBasedOnDificultyLevel(DefaultAIBrainLevel);
    }

    void Update()
    {
        CheckForSightRange();
        CheckForAttackRange();

        RunStateMachine();
    }

    private void RunStateMachine()
    {
        if (!playerInSightRange && !PlayerInAttackRange) PatrolState();
        if (playerInSightRange && !PlayerInAttackRange) ChaseState();
        if (PlayerInAttackRange && playerInSightRange) AttackState();
    }

    private void CheckForSightRange()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
       
        // Check If there is not obstacle between Player and Enemy
        if (playerInSightRange) playerInSightRange = !Physics.Linecast(transform.position, playerPosition.transform.position, obstacle);
    }

    private void CheckForAttackRange()
    {
        PlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);
        
        // Check If there is not obstacle between Player and Enemy
        if (PlayerInAttackRange) playerInSightRange = !Physics.Linecast(transform.position, playerPosition.transform.position, obstacle);
    }

    void PatrolState()
    {
        if (isWalkPointSet) agent.SetDestination(walkPoint);
        else SetAWalkPoint();

        CheckIfAgentReachedToWalkPoint();
    }

    void ChaseState()
    {
        agent.SetDestination(playerPosition.position);
    }

    public virtual void AttackState()
    {
        //ENEMY ATTACK
    }

    private void CheckIfAgentReachedToWalkPoint()
    {
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.sqrMagnitude < minimumDistanceToWalkPoint || nextTimeForChangingWalkPoint < Time.time)
            isWalkPointSet = false;
    }

    private void SetAWalkPoint()
    {
        walkPoint = GetARandomPointInPatrolingRange();

        if (IsWalkPointOnTheGround())
        {
            isWalkPointSet = true;
            nextTimeForChangingWalkPoint = MaxDurationOfEachPatrolingCycle + Time.time;
        }
    }

    private bool IsWalkPointOnTheGround()
    {
        return Physics.Raycast(walkPoint, -transform.up, GroundRaycastDistance, WhatisGround);
    }

    private Vector3 GetARandomPointInPatrolingRange()
    {
        float randomZ = UnityEngine.Random.Range(-patrolingDistanceRange, patrolingDistanceRange);
        float randomX = UnityEngine.Random.Range(-patrolingDistanceRange, patrolingDistanceRange);

        return new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    }

    protected void StandStillAndLookAtPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(playerPosition);
    }

    protected void AimAndAttack()
    {
        Vector3 aimDirection = GetAimDirection();
        Vector3 hitPoint;

        if (isAimingRaycastHittedToPlayer(aimDirection,out hitPoint)) 
            playerHealth?.TakeDamage(AttackDamage, hitPoint);
    }

    protected void UpdateNextAttackTimeBasedOnFireRate()
    {
        isAlreadyInAttackedState = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    Vector3 GetAimDirection()
    {
        Vector3 aimDirection = (playerPosition.transform.position - transform.position).normalized;

        aimDirection.y += UnityEngine.Random.Range(0, missShotPercentage);
        aimDirection.x += UnityEngine.Random.Range(0, missShotPercentage);
        aimDirection.z += UnityEngine.Random.Range(0, missShotPercentage);

        return aimDirection;
    }

    bool isAimingRaycastHittedToPlayer(Vector3 ShootDirection, out Vector3 hitPoint)
    {
        RaycastHit hit;
        bool isRaycastHit = Physics.Raycast(transform.position, ShootDirection, out hit, attackRange + attackRangeOffset, WhatIsPlayer);
        hitPoint = hit.point;
        return isRaycastHit;
    }

    void ResetAttack()
    {
        isAlreadyInAttackedState = false;
    }

    public void SetAIBrainBasedOnDificultyLevel(int DifficulityLevel)
    {
        sightRange = aiBrain.SightRange + (aiBrain.SightRange * aiBrain.SightRangeUpdateStep * DifficulityLevel);
        attackRange = aiBrain.AttackRange + (aiBrain.AttackRange * aiBrain.AttackRangeUpdateStep * DifficulityLevel);
        missShotPercentage = Mathf.Clamp(aiBrain.MissShotPercentage + (aiBrain.MissShotPercentage * aiBrain.MissShotPercentageUpdateStep * DifficulityLevel), 0, 1);
        agent.acceleration = aiBrain.MaxAcceleration + (aiBrain.MaxAcceleration * aiBrain.MaxAccelerationUpdateStep * DifficulityLevel);
        agent.speed = aiBrain.MaxSpeed + (aiBrain.MaxSpeed * aiBrain.MaxSpeedUpdateStep * DifficulityLevel);
        EnemyHealth.health = aiBrain.MaxHealth + (aiBrain.MaxHealth * aiBrain.MaxHealthUpdateStep * DifficulityLevel);
        AttackDamage = aiBrain.AttackDamage + (aiBrain.AttackDamage * aiBrain.AttackDamageUpdateStep * DifficulityLevel);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

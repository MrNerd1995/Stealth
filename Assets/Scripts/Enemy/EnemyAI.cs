using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float chaseWaitTime = 5f;
    public float patrolWaitTime = 1f;
    public Transform[] patrolWayPoints;

    private EnemySight enemySight;
    private NavMeshAgent navMeshAgent;
    private Transform player;
    private PlayerHealth playerHealth;
    private LastPlayerSighting lastPlayerSighting;

    private float chaseTimer;
    private float patrolTimer;
    private int wayPointIndex;

    private void Awake()
    {
        enemySight = GetComponent<EnemySight>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
    }

    private void Start()
    {
        playerHealth = player.gameObject.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (enemySight.playerInSight && playerHealth.health > 0f)
            Shooting();
        else if (enemySight.personalLastSighting != lastPlayerSighting.resetPosition && playerHealth.health > 0f)
            Chasing();
        else
            Patrolling();
    }

    private void Shooting()
    {
        navMeshAgent.isStopped = true;
    }

    private void Chasing()
    {
        Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;

        if (sightingDeltaPos.sqrMagnitude > 4f)
        {
            navMeshAgent.destination = enemySight.personalLastSighting;
        }

        navMeshAgent.speed = chaseSpeed;

        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            chaseTimer += Time.deltaTime;

            if (chaseTimer > chaseWaitTime)
            {
                lastPlayerSighting.position = lastPlayerSighting.resetPosition;
                enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
                chaseTimer = 0f;
            }
        }
        else
            chaseTimer = 0f;
    }

    private void Patrolling()
    {
        navMeshAgent.speed = patrolSpeed;

        if (navMeshAgent.destination == lastPlayerSighting.resetPosition || navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                if (wayPointIndex == patrolWayPoints.Length - 1)
                {
                    wayPointIndex = 0;
                }
                else
                {
                    wayPointIndex++;
                }

                patrolTimer = 0f;
            }
        }
        else
            patrolTimer = 0f;

        navMeshAgent.destination = patrolWayPoints[wayPointIndex].position;
    }
}

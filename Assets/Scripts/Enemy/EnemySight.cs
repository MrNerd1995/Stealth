using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySight : MonoBehaviour
{
    public float fieldOfViewAngle = 110f;
    public bool playerInSight;
    public Vector3 personalLastSighting;

    private NavMeshAgent navMeshAgent;
    private new SphereCollider collider;
    private Animator animator;
    private LastPlayerSighting lastPlayerSighting;
    private GameObject player;
    private Animator playerAnimator;
    private PlayerHealth playerHealth;
    private HashIDs hash;
    private Vector3 prevSighting;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        collider = GetComponent<SphereCollider>();
        animator = GetComponent<Animator>();
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
        player = GameObject.FindGameObjectWithTag(Tags.player);
        playerAnimator = player.GetComponent<Animator>();
        playerHealth = player.GetComponent<PlayerHealth>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
    }

    private void Start()
    {
        personalLastSighting = lastPlayerSighting.resetPosition;
        prevSighting = lastPlayerSighting.resetPosition;
    }

    private void Update()
    {
        if (lastPlayerSighting.position != prevSighting)
        {
            personalLastSighting = lastPlayerSighting.position;
        }
        prevSighting = lastPlayerSighting.position;

        if (playerHealth.health > 0f)
        {
            animator.SetBool(hash.playerInsightBool, playerInSight);
        }
        else
        {
            animator.SetBool(hash.playerInsightBool, false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInSight = false;

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fieldOfViewAngle * 0.5f)
            {
                bool hasHit = Physics.Raycast(transform.position + transform.up, direction.normalized, out RaycastHit hit, collider.radius);

                if (hasHit)
                {
                    if (hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                        lastPlayerSighting.position = player.transform.position;
                    }
                }
            }

            int playerLayerZeroStateHash = playerAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;
            int playerLayerOneStateHash = playerAnimator.GetCurrentAnimatorStateInfo(1).fullPathHash;

            if (playerLayerZeroStateHash == hash.locomotionState || playerLayerOneStateHash == hash.shoutState)
            {
                if (CalPathLength(player.transform.position) <= collider.radius)
                {
                    personalLastSighting = player.transform.position;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInSight = false;
        }
    }

    private float CalPathLength(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();

        if (navMeshAgent.enabled)
        {
            navMeshAgent.CalculatePath(targetPosition, path);
        }

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;

        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }
}

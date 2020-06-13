using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    public float deadZone = 5f;

    private Transform player;
    private EnemySight enemySight;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private HashIDs hash;
    private AnimatorSetup animatorSetup;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        enemySight = GetComponent<EnemySight>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();

        deadZone *= Mathf.Deg2Rad;
    }

    private void Start()
    {
        navMeshAgent.updateRotation = false;
        animatorSetup = new AnimatorSetup(animator, hash);

        animator.SetLayerWeight(1, 1f);
        animator.SetLayerWeight(2, 1f);
    }

    private void Update()
    {
        NavAnimSetup();
    }

    private void OnAnimatorMove()
    {
        navMeshAgent.velocity = animator.deltaPosition / Time.deltaTime;
        transform.rotation = animator.rootRotation;
    }

    private void NavAnimSetup()
    {
        float speed;
        float angle;

        if (enemySight.playerInSight)
        {
            speed = 0f;

            angle = FindAngle(transform.forward, player.transform.position - transform.position, transform.up);
        }
        else
        {
            speed = Vector3.Project(navMeshAgent.desiredVelocity, transform.forward).magnitude;

            angle = FindAngle(transform.forward, navMeshAgent.desiredVelocity, transform.up);

            if (Mathf.Abs(angle) < deadZone)
            {
                transform.LookAt(transform.position + navMeshAgent.desiredVelocity);
                angle = 0f;
            }
        }

        animatorSetup.Setup(speed, angle);
    }

    private float FindAngle(Vector3 fromVector, Vector3 toVector, Vector3 upVector)
    {
        if (toVector == Vector3.zero)
            return 0f;
        
        float angle = Vector3.Angle(fromVector, toVector);
        Vector3 normal = Vector3.Cross(fromVector, toVector);

        angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
        angle *= Mathf.Deg2Rad;

        return angle;
    }
}

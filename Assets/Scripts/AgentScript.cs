using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] bool isPatroling = true;
    [SerializeField] float arrivalDistance;
    [SerializeField] Animator anim;
    [SerializeField] float velocity;
    [SerializeField] Transform currentDestination;
    [SerializeField] int currentPatrolPointIndex;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentDestination = patrolPoints[0];
        currentPatrolPointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.hasPath && agent.remainingDistance <= arrivalDistance)
        {
            if (currentPatrolPointIndex < patrolPoints.Length - 1)
            {
                currentPatrolPointIndex++;
            }
            else
            {
                currentPatrolPointIndex = 0;
            }
            currentDestination = patrolPoints[currentPatrolPointIndex];
        }
        agent.destination = currentDestination.position;
        velocity = agent.velocity.magnitude;
        anim.SetFloat("Speed", velocity);
    }
}



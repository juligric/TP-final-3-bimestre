using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] bool isPatrolling = true;
    [SerializeField] float arrivalDistance = 1f;
    [SerializeField] float damageDistance = 2f;
    [SerializeField] int damageAmount = 10;
    [SerializeField] Animator anim;

    RaycastSight sight;
    int currentPatrolPointIndex;
    Transform currentDestination;

    // --- Agregado ---
    float lostSightTimer = 0f; // Contador para cuando pierde al jugador
    [SerializeField] float lostSightTime = 2f; // 2 segundos antes de volver al patrullaje
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        sight = GetComponentInChildren<RaycastSight>();
    }

    void Start()
    {
        if (patrolPoints.Length > 0)
        {
            currentDestination = patrolPoints[0];
            currentPatrolPointIndex = 0;
        }

        // Inicializamos la velocidad de patrullaje
        agent.speed = walkSpeed;
    }

    void Update()
    {
        if (sight != null && sight.canSeePlayer)
        {
            // --- Estado: persiguiendo ---
            lostSightTimer = 0f;
            isPatrolling = false;

            agent.destination = sight.player.position;
            agent.speed = runSpeed;

            if (anim != null)
                anim.SetFloat("Speed", 1f); // correr

            // Daño al jugador
            if (agent.remainingDistance <= damageDistance && !agent.pathPending)
            {
                player player = sight.player.GetComponent<player>();
                if (player != null)
                    player.TakeDamage(damageAmount);
            }
        }
        else
        {
            // --- Estado: patrullaje ---
            lostSightTimer += Time.deltaTime;

            if (lostSightTimer >= lostSightTime)
            {
                isPatrolling = true;
            }

            if (isPatrolling && patrolPoints.Length > 0)
            {
                agent.speed = walkSpeed; // caminar

                // Cambiar animación a idle o caminar
                if (anim != null)
                    anim.SetFloat("Speed", 0f); // idle mientras patrulla

                // Movimiento de patrullaje
                if (agent.hasPath && agent.remainingDistance <= arrivalDistance && !agent.pathPending)
                {
                    currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
                    currentDestination = patrolPoints[currentPatrolPointIndex];
                }
                agent.destination = currentDestination.position;
            }
            else
            {
                // NPC parado
                agent.destination = transform.position;
                if (anim != null)
                    anim.SetFloat("Speed", 0f); // idle
            }
        }
    }


    // Trigger para FPSController
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player player = other.GetComponent<player>();
            if (player != null)
            {
                player.TakeDamage(1000); // suficiente para morir al instante
            }
        }
    }
}

